using AutoMapper;
using Identity.Application.Interfaces;
using MassTransit;
using MediatR;
using Shared.Contracts.Profile;

namespace Identity.Application.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler(
    IUserProfileRepository repository,
    IMapper mapper,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<UpdateUserProfileCommand, UserProfileDto?>
    {

        public async Task<UserProfileDto?> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (profile is null)
                return null;

            profile.Update(request.DisplayName, request.Bio, request.PhoneNumber);

            await repository.SaveChangesAsync(cancellationToken);

            var profileDto = mapper.Map<UserProfileDto>(profile);

            await publishEndpoint.Publish(new UserProfileUpdatedEvent(
                request.UserId,
                request.DisplayName,
                request.Bio,
                request.PhoneNumber,
                profile.AvatarUrl,
                profile.UpdatedAt
            ), cancellationToken);


            return profileDto;
        }
    }
}
