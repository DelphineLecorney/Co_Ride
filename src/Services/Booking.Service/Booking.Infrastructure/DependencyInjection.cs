using Booking.Application.Interfaces;
using Booking.Infrastructure.ExternalServices;
using Booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BookingDatabase")
                ?? throw new InvalidOperationException("La chaîne de connexion à la base de données BookingDatabase n'est pas configurée");

            services.AddDbContext<BookingDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IBookingRepository, BookingRepository>();

            var tripServiceUrl = configuration["Services:TripService:Url"]
                ?? "https://localhost:7001";

            services.AddHttpClient<ITripServiceClient, TripServiceClient>(client =>
            {
                client.BaseAddress = new Uri(tripServiceUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}
