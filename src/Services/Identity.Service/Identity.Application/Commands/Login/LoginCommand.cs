using MediatR;
using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Commands.Login
{
    public sealed record LoginCommand
        (
        string Email,
        string Password
        ) : IRequest<AuthResponse>;
        
}
