using MediatR;
using Shared.Contracts.Profile;

namespace Identity.Application.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        Guid UserId,
        string FirstName,
        string LastName,
        string? Bio,
        string? PhoneNumber
    ) : IRequest<UserProfileDto>;
}
