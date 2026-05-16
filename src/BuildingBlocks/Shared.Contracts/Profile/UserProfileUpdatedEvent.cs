namespace Shared.Contracts.Profile
{
    public record UserProfileUpdatedEvent(
        Guid UserId,
        string DisplayName,
        string? Bio,
        string? PhoneNumber,
        string? AvatarUrl,
        DateTime UpdatedAt
    );
}
