using AutoMapper;
using Identity.Application.Commands.Login;
using Identity.Application.Commands.Logout;
using Identity.Application.Commands.Refresh;
using Identity.Application.Commands.Register;
using Identity.Application.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.DTOs.Common;
using Shared.Contracts.DTOs.Identity;
using System.Security.Claims;

/// <summary>
/// Contrôleur d'authentification pour gérer les opérations
/// liées à l'inscription, la connexion et la gestion des utilisateurs.
/// </summary>
namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// Endpoint pour l'inscription d'un nouvel utilisateur.
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<AuthResponse>.SuccessResponse(result));
        }

        /// Endpoint pour la connexion d'un utilisateur existant.
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(LoginRequest request)
        {
            var command = _mapper.Map<LoginCommand>(request);
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<AuthResponse>.SuccessResponse(result));
        }

        // Endpoint pour rafraîchir le token d'authentification.
        [HttpPost("refresh")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Refresh(RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<AuthResponse>.SuccessResponse(result));
        }

        // Endpoint pour récupérer les informations de l'utilisateur actuellement connecté.
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDto>>> Me()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Impossible d'identifier l'utilisateur.");
            }

            var query = new GetCurrentUserQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(ApiResponse<UserDto>.SuccessResponse(result));
        }


        // Endpoint pour la déconnexion de l'utilisateur actuellement connecté.
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Impossible d'identifier l'utilisateur.");
            }

            await _mediator.Send(new LogoutCommand(userId));

            return NoContent();
        }
    }
}
