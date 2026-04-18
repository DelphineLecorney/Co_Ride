using MediatR;
using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Commands.Refresh
{
    public sealed class RefreshTokenCommand : IRequest<AuthResponse>
    {
        public string RefreshToken { get; init; } = string.Empty;
        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
