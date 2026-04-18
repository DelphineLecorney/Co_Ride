using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Contracts.DTOs.Identity;
using System.Security;

namespace Identity.Application.Commands.Refresh
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;


        public RefreshTokenCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetActiveAsync(request.RefreshToken);

            if (storedToken == null)
                throw new SecurityException("Refresh token invalide ou expiré");

            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user == null || user.IsDeleted)
                throw new SecurityException("Utilisateur introuvable");

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _refreshTokenRepository.RevokeAsync(
                request.RefreshToken,
                ipAddress: null,
                reason: "Token remplacé",
                replacedByToken: newRefreshToken
            );

            await _refreshTokenRepository.SaveAsync(
                user.Id,
                newRefreshToken,
                DateTime.UtcNow.AddDays(7)
            );

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto(
                user.Id,
                user.Email ?? string.Empty,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                roles.ToList(),
                user.IsEmailVerified,
                user.ReputationScore,
                user.ReviewCount,
                user.CreatedAt
            );

            return new AuthResponse(
                AccessToken: newAccessToken,
                RefreshToken: newRefreshToken,
                ExpiresAt: DateTime.UtcNow.AddMinutes(15),
                User: userDto
            );
        }
    }
}