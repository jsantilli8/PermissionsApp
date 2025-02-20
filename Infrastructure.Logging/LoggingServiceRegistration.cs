using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Logging
{
    public static class LoggingServiceRegistration
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
            return services;
        }
    }
}
