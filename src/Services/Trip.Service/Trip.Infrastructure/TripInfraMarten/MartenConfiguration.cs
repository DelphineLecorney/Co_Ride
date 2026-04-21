namespace Trip.Infrastructure.TripInfraMarten
{
    using Marten;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Trip.Application.Projections;
    using Trip.Domain.TripDomainEvents;
    using JasperFx.Events.Projections;
    using JasperFx.Events.Daemon;

    /// <summary>
    /// Configure Marten en tant qu'Event Store pour l'application Trip.
    /// - Charge la connection string "TripDatabase"
    /// - Active la création automatique du schéma
    /// - Enregistre les événements du domaine (TripCreated, TripCancelled, SeatsReserved)
    /// - Ajoute la projection TripSearchProjection en mode inline
    /// - Active l'Async Daemon en mode Solo pour les projections
    /// </summary>

    public static class MartenConfiguration
    {
        public static IServiceCollection AddMartenEventStore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TripDatabase")
                ?? throw new InvalidOperationException("TripDatabase connection string n'est pas configurée");

            services.AddMarten(options =>
            {
                options.Connection(connectionString);
                options.AutoCreateSchemaObjects = JasperFx.AutoCreate.All;



                // Event types
                options.Events.AddEventType<TripCreated>();
                options.Events.AddEventType<TripCancelled>();
                options.Events.AddEventType<SeatsReserved>();

                // Projections
                options.Projections.Add<TripSearchProjection>(ProjectionLifecycle.Inline);

            })
            .AddAsyncDaemon(DaemonMode.Solo);

            return services;
        }
    }
}
