using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Messaging;

public static class MassTransitExtensions
{
    /// <summary>
    /// Cette méthode configure MassTransit pour utiliser RabbitMQ comme transport de messages. 
    /// Elle lit les paramètres de connexion à RabbitMQ à partir de la configuration de l'application, 
    /// ce qui permet de centraliser la gestion des paramètres de connexion et de les modifier facilement 
    /// sans avoir à changer le code. De plus, elle accepte une action optionnelle pour configurer les consumers, 
    /// permettant ainsi une flexibilité dans la configuration des consommateurs de messages tout en gardant 
    /// la configuration de base du transport séparée et réutilisable.
    /// </summary>
    /// <param name="services">Le conteneur de services.</param>
    /// <param name="configuration">La configuration de l'application.</param>
    /// <param name="configureConsumers">Une action pour configurer les consumers.</param>
    /// <returns>Le conteneur de services configuré.</returns>
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureConsumers = null)
    {
        services.AddMassTransit(config =>
        {
            // Permet de configurer les consumers de messages en utilisant l'action fournie,
            // ce qui offre une flexibilité pour ajouter des consumers spécifiques à
            // l'application tout en gardant la configuration de base du transport séparée.
            configureConsumers?.Invoke(config);

            config.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqHost = configuration["RabbitMQ:Host"] ?? "localhost";
                var rabbitMqUser = configuration["RabbitMQ:Username"] ?? "guest";
                var rabbitMqPass = configuration["RabbitMQ:Password"] ?? "guest";

                // Configure RabbitMQ comme transport de messages en utilisant les paramètres de connexion
                cfg.Host(rabbitMqHost, "/", h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPass);
                });

                // Configure les endpoints de MassTransit en utilisant le contexte de configuration, ce qui permet
                // de découvrir automatiquement les consumers et de les associer aux queues correspondantes dans RabbitMQ.
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}