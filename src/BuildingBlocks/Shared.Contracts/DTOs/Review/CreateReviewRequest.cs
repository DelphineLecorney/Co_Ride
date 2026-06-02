namespace Shared.Contracts.DTOs.Review
{
    public record CreateReviewRequest(
        Guid TripId,
        Guid RevieweeId,
        int Rating,
        string? Comment
    );
}
