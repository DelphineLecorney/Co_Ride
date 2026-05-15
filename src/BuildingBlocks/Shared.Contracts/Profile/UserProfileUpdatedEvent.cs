namespace Shared.Contracts.Profile
{
    public record UserProfileUpdatedEvent(
        Guid UserId,
        string DisplayName,
        string? AvatarUrl,
        DateTime UpdatedAt
    );
}
