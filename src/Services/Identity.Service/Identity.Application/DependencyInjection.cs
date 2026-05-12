using Identity.Application.Commands.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(RegisterCommand).Assembly));

            //services.AddValidatorsFromAssembly(
            //    typeof(RegisterCommand).Assembly);

            return services;

        }
    }
}