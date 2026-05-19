namespace Shared.Contracts.Profile
{
    public record UpdateUserProfileRequest(
        string FirstName,
        string LastName,
        string? Bio,
        string? PhoneNumber
    );
}
