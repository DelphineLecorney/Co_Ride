using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trip.Infrastructure.Messaging.Behaviors;
using Trip.Infrastructure.TripInfraMarten;
using Shared.Messaging;

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

        return services;
    }
}