using MediatR;
using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery(Guid UserId) : IRequest<UserDto>;

}
