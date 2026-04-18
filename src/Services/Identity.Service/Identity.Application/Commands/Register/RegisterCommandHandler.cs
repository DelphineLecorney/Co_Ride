using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Commands.Register
{
    /// <summary>
    /// Cette classe gère la logique métier pour l'inscription d'un nouvel utilisateur.
    /// elle utilise UserManager pour créer l'utilisateur et IJwtTokenService pour générer 
    /// un token JWT après la création réussie de l'utilisateur.
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtService;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var token = _jwtService.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            (
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                roles.ToList(),
                user.IsEmailVerified,
                user.ReputationScore,
                user.ReviewCount,
                user.CreatedAt
                );

            return new AuthResponse
                (
                    AccessToken: token.AccessToken,
                    RefreshToken: token.RefreshToken,
                    ExpiresAt: token.ExpiresAt,
                    User: userDto
                );
        }
    }
}