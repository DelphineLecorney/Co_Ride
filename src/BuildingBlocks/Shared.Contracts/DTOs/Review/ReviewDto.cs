namespace Shared.Contracts.DTOs.Review
{
    public record ReviewDto(
        Guid Id,
        Guid TripId,
        Guid ReviewerId,
        Guid RevieweeId,
        string ReviewerType,
        int Rating,
        string? Comment,
        DateTime CreatedAt
    );
}
