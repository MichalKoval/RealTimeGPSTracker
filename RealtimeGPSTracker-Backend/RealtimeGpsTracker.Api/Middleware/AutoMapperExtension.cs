using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RealtimeGPSTracker.Application.Helpers;

namespace RealtimeGpsTracker.Api.Middleware
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations (maps Database models to Data transfer objects)
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
