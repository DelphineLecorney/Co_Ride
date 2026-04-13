using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
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

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
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

            var token = _jwtService.GenerateToken(user);
            return new AuthResultDto
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpiresAt = token.ExpiresAt,
                UserId = existingUser.Id,
                Email = existingUser.Email!
            };
        }
    }
}