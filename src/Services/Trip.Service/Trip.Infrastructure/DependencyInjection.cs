using Marten;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging;
using Trip.Application.Interfaces;
using Trip.Domain.ValueObjects;
using Trip.Infrastructure.Consumers;
using Trip.Infrastructure.Messaging.Behaviors;
using Trip.Infrastructure.Repositories;
using Trip.Infrastructure.TripInfraMarten;

namespace Trip.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Marten Event Store
        services.AddMartenEventStore(configuration);

        // MassTransit + RabbitMQ
        services.AddMassTransitWithRabbitMq(configuration);

        // Behavior MediatR pour publier les événements de domaine
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MartenEventPublisherBehavior<,>));

        services.AddScoped<IDriverProfileRepository, DriverProfileRepository>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserProfileUpdatedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.ReceiveEndpoint("trip-user-profile-updated", e =>
                {
                    e.ConfigureConsumer<UserProfileUpdatedConsumer>(ctx);
                });
            });
        });

        services.AddMarten(opts =>
        {
            opts.Connection(configuration.GetConnectionString("TripDb")!);

            opts.Schema.For<DriverProfile>()
                .Index(x => x.UserId, idx => idx.IsUnique = true);
        })
        .UseLightweightSessions();
        return services;
    }
}