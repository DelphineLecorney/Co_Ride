using AutoMapper;
using Identity.Application.Interfaces;
using MediatR;
using Shared.Contracts.Profile;

namespace Identity.Application.Queries.GetUserProfile
{
    public class GetUserProfileHandler(
        IUserProfileRepository repository,
        IMapper mapper
    ) : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
    {
        public async Task<UserProfileDto?> Handle(
            GetUserProfileQuery query,
            CancellationToken cancellationToken)
        {
            var profile = await repository.GetByUserIdAsync(query.UserId, cancellationToken);

            if (profile is null)
                return null;

            return mapper.Map<UserProfileDto>(profile);
        }
    }
}
