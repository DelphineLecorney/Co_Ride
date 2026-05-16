using MediatR;
using Shared.Contracts.Profile;

namespace Identity.Application.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        Guid UserId,
        string DisplayName,
        string? Bio,
        string? PhoneNumber
    ) : IRequest<UserProfileDto>;
}
