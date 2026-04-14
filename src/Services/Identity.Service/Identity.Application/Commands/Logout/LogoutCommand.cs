using MediatR;

namespace Identity.Application.Commands.Logout
{
    public record LogoutCommand(Guid UserId) : IRequest<Unit>;

}
