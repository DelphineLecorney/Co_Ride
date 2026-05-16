namespace Shared.Contracts.Profile
{
    public record UpdateUserProfileRequest(
        string DisplayName,
        string? Bio,
        string? PhoneNumber
    );
}
