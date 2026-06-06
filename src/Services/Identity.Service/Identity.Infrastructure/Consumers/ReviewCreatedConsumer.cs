using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Contracts.DTOs.Review;

public class ReviewCreatedConsumer(
    UserManager<ApplicationUser> userManager,
    ILogger<ReviewCreatedConsumer> logger
) : IConsumer<ReviewCreatedEvent>
{
    public async Task Consume(ConsumeContext<ReviewCreatedEvent> context)
    {
        var evt = context.Message;
        var user = await userManager.FindByIdAsync(evt.RevieweeId.ToString());

        if (user is null)
        {
            logger.LogWarning("User {UserId} not found for review update", evt.RevieweeId);
            return;
        }

        // Recalcul de la note moyenne
        var newReviewCount = user.ReviewCount + 1;
        var newScore = ((user.ReputationScore * user.ReviewCount) + evt.Rating)
                             / newReviewCount;

        user.UpdateReputation(Math.Round(newScore, 2), newReviewCount);

        await userManager.UpdateAsync(user);

        logger.LogInformation(
            "ReputationScore updated for {UserId} → {Score}",
            evt.RevieweeId, newScore);
    }
}