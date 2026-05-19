using AutoMapper;
using Identity.Application.Interfaces;
using MassTransit;
using MediatR;
using Shared.Contracts.Profile;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

            profile.Update(request.FirstName, request.LastName, request.Bio, request.PhoneNumber);

            await repository.SaveChangesAsync(cancellationToken);

            var profileDto = mapper.Map<UserProfileDto>(profile);

            await publishEndpoint.Publish(new UserProfileUpdatedEvent(
                request.UserId,
                profile.FirstName,
                profile.LastName,
                profile.AvatarUrl,
                profile.UpdatedAt
            ), cancellationToken);


            return profileDto;
        }
    }
}
