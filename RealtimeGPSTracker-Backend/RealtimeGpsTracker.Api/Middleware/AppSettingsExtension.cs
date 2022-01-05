using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealtimeGPSTracker.Application.Services;
using static RealtimeGpsTracker.Application.Services.JwtService;
using static RealtimeGpsTracker.Application.Services.SslService;

namespace RealtimeGpsTracker.Api.Middleware
{
    public static class AppSettingsExtension
    {
        public static IServiceCollection AddJwtSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)));

            return services;
        }

        public static IServiceCollection AddSslSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SslSettings>(config.GetSection(nameof(SslSettings)));

            return services;
        }

        public static IServiceCollection AddDeviceStatusWorkerSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<DeviceStatusWorkerService>(config.GetSection(nameof(DeviceStatusWorkerService)));

            return services;
        }
    }
}
