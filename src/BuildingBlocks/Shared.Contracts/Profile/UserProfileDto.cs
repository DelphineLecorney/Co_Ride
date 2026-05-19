namespace Shared.Contracts.Profile
{
    public record UserProfileDto(
        Guid UserId,
        string FirstName,
        string LastName,
        string FullName,
        string? Bio,
        string? AvatarUrl,
        string? PhoneNumber,
        DateTime UpdatedAt
    );
}
