using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        Guid UserId,
        string DisplayName,
        string? Bio,
        string? PhoneNumber
    ) : IRequest<UserProfileDto>;
}
