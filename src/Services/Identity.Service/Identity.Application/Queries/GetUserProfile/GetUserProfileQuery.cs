using MediatR;
using Shared.Contracts.Profile;

namespace Identity.Application.Queries.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId)
    : IRequest<UserProfileDto>;
}
