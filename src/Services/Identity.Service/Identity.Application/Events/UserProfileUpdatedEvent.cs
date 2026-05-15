namespace Identity.Application.Events
{
    public record UserProfileUpdatedEvent(
        Guid UserId,
        string DisplayName,
        string? AvatarUrl,
        DateTime UpdatedAt
    );
}
