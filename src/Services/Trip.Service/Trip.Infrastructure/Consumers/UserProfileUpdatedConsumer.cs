using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Profile;
using Trip.Application.Interfaces;
using Trip.Domain.ValueObjects;

namespace Trip.Infrastructure.Consumers
{
    public class UserProfileUpdatedConsumer(
        IDriverProfileRepository repository,
        ILogger<UserProfileUpdatedConsumer> logger
    ) : IConsumer<UserProfileUpdatedEvent>
    {
        public async Task Consume(ConsumeContext<UserProfileUpdatedEvent> context)
        {
            var evt = context.Message;

            logger.LogInformation(
                "Synchronisation du profil pour UserId {UserId}", evt.UserId);

            var profile = await repository.GetByUserIdAsync(evt.UserId, context.CancellationToken);

            if (profile is null)
            {
                profile = DriverProfile.CreateFromEvent(
                    evt.UserId,
                    evt.FirstName,
                    evt.LastName,
                    evt.AvatarUrl
                );

                await repository.AddAsync(profile, context.CancellationToken);
            }
            else
            {
                profile.SyncFromEvent(evt.FirstName, evt.LastName, evt.AvatarUrl);
            }

            await repository.SaveChangesAsync(context.CancellationToken);

            logger.LogInformation(
                "Profil de conducteur synchronisé pour UserId {UserId}", evt.UserId);
        }
    }
}
