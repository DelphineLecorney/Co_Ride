using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.DTOs.Common;
using Shared.Contracts.DTOs.Identity;

/// <summary>
/// Contrôleur d'authentification pour gérer les inscriptions et les connexions des utilisateurs.
/// Expose des endpoints pour l'inscription et la connexion, en utilisant UserManager pour gérer 
/// les utilisateurs et IJwtTokenService pour générer les tokens JWT.
/// </summary>

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _tokenService;
        public AuthController(UserManager<ApplicationUser> userManager, IJwtTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        /// Endpoint pour l'inscription d'un nouvel utilisateur.
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new ApiResponse<AuthResponse>(
                    false,
                    null,
                    "Adresse e-mail déjà enregistrée",
                    new[] { "Un utilisateur avec cette adresse e-mail existe déjà" }
                ));
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse<AuthResponse>(
                    false,
                    null,
                    "L'inscription a échoué.",
                    result.Errors.Select(e => e.Description)
                ));
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var authResponse = new AuthResponse(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(60),
                new UserDto(
                    user.Id,
                    user.Email!,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    user.CreatedAt
                )
            );

            return Ok(new ApiResponse<AuthResponse>(true, authResponse, "L'inscription a réussi."));
        }

        /// Endpoint pour la connexion d'un utilisateur existant.
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<AuthResponse>(
                    false,
                    null,
                    "Identifiants non valides."
                ));
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new ApiResponse<AuthResponse>(
                    false,
                    null,
                    "Identifiants non valides."
                ));
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var authResponse = new AuthResponse(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(60),
                new UserDto(
                    user.Id,
                    user.Email!,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    user.CreatedAt
                )
            );

            return Ok(new ApiResponse<AuthResponse>(true, authResponse, "Connexion réussie."));
        }

    }
}
