using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Messaging;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Services;

namespace Notification.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // MongoDB
        var host = configuration["MONGODB_HOST"] ?? "localhost";
        var port = configuration["MONGODB_PORT"] ?? "27017";
        var user = configuration["MONGODB_ROOT_USERNAME"] ?? "root";
        var pass = configuration["MONGODB_ROOT_PASSWORD"] ?? "example";
        var db = configuration["MONGODB_DATABASE"] ?? "notifications";

        var mongoConnectionString = $"mongodb://{user}:{pass}@{host}:{port}";

        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(db);
        });


        // Repository
        services.AddScoped<INotificationRepository, NotificationRepository>();

        // Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPushNotificationService, PushNotificationService>();

        // MassTransit + RabbitMQ
        services.AddMassTransitWithRabbitMq(configuration);

        return services;
    }
}