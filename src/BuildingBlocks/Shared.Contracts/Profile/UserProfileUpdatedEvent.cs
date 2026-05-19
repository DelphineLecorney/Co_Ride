namespace Shared.Contracts.Profile
{
    public record UserProfileUpdatedEvent(
        Guid UserId,
        string FirstName,
        string LastName,
        string? AvatarUrl,
        DateTime UpdatedAt
    );
}
