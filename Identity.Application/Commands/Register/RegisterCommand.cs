using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Commands.Register
{
    public sealed record RegisterCommand
    (
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string? PhoneNumber
    ) : IRequest<AuthResultDto>;
}
