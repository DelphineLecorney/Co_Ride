using AutoMapper;
using Identity.Application.Interfaces;
using MediatR;
using Shared.Contracts.Profile;

namespace Identity.Application.Commands.SetUserAvatar
{
    public class SetUserAvatarCommandHandler(
        IUserProfileRepository repository,
        IMapper mapper
    ) : IRequestHandler<SetUserAvatarCommand, UserProfileDto?>
    {
        public async Task<UserProfileDto?> Handle(
            SetUserAvatarCommand command,
            CancellationToken cancellationToken)
        {
            var profile = await repository.GetByUserIdAsync(command.UserId, cancellationToken);

            if (profile is null)
                return null;

            profile.SetAvatar(command.AvatarUrl);

            await repository.SaveChangesAsync(cancellationToken);

            return mapper.Map<UserProfileDto>(profile);
        }

    }
}
