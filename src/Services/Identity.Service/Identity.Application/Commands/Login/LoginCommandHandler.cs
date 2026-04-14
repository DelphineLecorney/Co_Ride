using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtService;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                Email = request.Email
            };

            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                throw new Exception("Identifiants non valides.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(existingUser, request.Password);
            if (!passwordValid)
            {
                throw new Exception("IIdentifiants non valides.");
            }

            var token = _jwtService.GenerateToken(existingUser);
            var roles = await _userManager.GetRolesAsync(existingUser);

            var userDto = new UserDto(
                existingUser.Id,
                existingUser.Email ?? string.Empty,
                existingUser.FirstName,
                existingUser.LastName,
                existingUser.PhoneNumber,
                roles.ToList(),
                existingUser.IsEmailVerified,
                existingUser.ReputationScore,
                existingUser.ReviewCount,
                existingUser.CreatedAt
            );


            return new AuthResponse(
                token.AccessToken,
                token.RefreshToken,
                token.ExpiresAt,
                userDto
            );

        }
    }
}