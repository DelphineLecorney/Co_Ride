namespace Identity.Application.DTOs
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
