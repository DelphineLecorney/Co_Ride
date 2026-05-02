using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Consumers.Booking;
using Notification.Application.Consumers.Trip;

namespace Notification.Infrastructure.Messaging;

public static class MassTransitConfiguration
{
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            // Enregistrer tous les consumers
            x.AddConsumer<TripCreatedConsumer>();
            x.AddConsumer<TripCancelledConsumer>();
            x.AddConsumer<BookingCreatedConsumer>();
            x.AddConsumer<BookingCancelledConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqHost = configuration["RabbitMQ:Host"] ?? "localhost";
                var rabbitMqUser = configuration["RabbitMQ:Username"] ?? "guest";
                var rabbitMqPassword = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(rabbitMqHost, "/", h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPassword);
                });

                // Configurer les endpoints pour chaque consumer
                cfg.ReceiveEndpoint("notification-service-trip-created", e =>
                {
                    e.ConfigureConsumer<TripCreatedConsumer>(context);
                });

                cfg.ReceiveEndpoint("notification-service-trip-cancelled", e =>
                {
                    e.ConfigureConsumer<TripCancelledConsumer>(context);
                });

                cfg.ReceiveEndpoint("notification-service-booking-created", e =>
                {
                    e.ConfigureConsumer<BookingCreatedConsumer>(context);
                });

                cfg.ReceiveEndpoint("notification-service-booking-cancelled", e =>
                {
                    e.ConfigureConsumer<BookingCancelledConsumer>(context);
                });
            });
        });

        return services;
    }
}