namespace Shared.Contracts.Profile
{
    public record UserProfileDto(
        Guid UserId,
        string DisplayName,
        string? Bio,
        string? AvatarUrl,
        string? PhoneNumber,
        DateTime UpdatedAt
    );
}
