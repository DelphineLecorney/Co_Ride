using MediatR;
using Shared.Contracts.Profile;

namespace Identity.Application.Commands.SetUserAvatar
{
    public record SetUserAvatarCommand(
        Guid UserId,
        string AvatarUrl
    ) : IRequest<UserProfileDto>;
}
