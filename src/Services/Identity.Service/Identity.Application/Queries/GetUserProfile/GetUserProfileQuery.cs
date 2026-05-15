using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Queries.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId)
    : IRequest<UserProfileDto>;
}
