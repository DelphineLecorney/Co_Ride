namespace Shared.Contracts.DTOs.Review
{
    public record ReviewCreatedEvent(
        Guid RevieweeId,
        int Rating,
        DateTime CreatedAt
    );
}
