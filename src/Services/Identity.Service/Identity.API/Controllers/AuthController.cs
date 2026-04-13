using AutoMapper;
using Identity.Application.Commands.Login;
using Identity.Application.Commands.Register;
using Identity.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.DTOs.Common;
using Shared.Contracts.DTOs.Identity;

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
            return Ok(ApiResponse<AuthResultDto>.SuccessResponse(result));
        }

        /// Endpoint pour la connexion d'un utilisateur existant.
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(LoginRequest request)
        {
            var command = _mapper.Map<LoginCommand>(request);

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<AuthResultDto>.SuccessResponse(result));
        }

    }
}
