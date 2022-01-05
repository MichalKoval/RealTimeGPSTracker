using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealtimeGPSTracker.Application.UseCases.AuthUseCase;
using RealtimeGPSTracker.Application.UseCases.CoordinateUseCase;
using RealtimeGPSTracker.Application.UseCases.DeviceUseCase;
using RealtimeGPSTracker.Application.UseCases.SslUseCase;
using RealtimeGPSTracker.Application.UseCases.TripUseCase;
using RealtimeGPSTracker.Application.UseCases.UserUseCase;
using System.Reflection;

namespace RealtimeGpsTracker.Api.Middleware
{
    public static class HandlerExtension
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(new Assembly[] {
                // User handlers
                typeof(LoginUserCommandHandler).GetTypeInfo().Assembly,
                typeof(RegisterUserCommandHandler).GetTypeInfo().Assembly,
                typeof(DetailUserQueryHandler).GetTypeInfo().Assembly,
                typeof(UpdateUserDetailsCommandHandler).GetTypeInfo().Assembly,
                typeof(UpdateUserPasswordCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteUserCommandHandler).GetTypeInfo().Assembly,

                // Coordinate handlers
                typeof(InsertCoordinatesCommandHandler).GetTypeInfo().Assembly,

                // Trip handlers
                typeof(TripsQueryHandler).GetTypeInfo().Assembly,
                typeof(DeleteTripCommandHandler).GetTypeInfo().Assembly,

                // Device handlers
                typeof(DevicesQueryHandler).GetTypeInfo().Assembly,
                typeof(DetailDeviceQueryHandler).GetTypeInfo().Assembly,                
                typeof(CreateDeviceCommandHandler).GetTypeInfo().Assembly,
                typeof(UpdateDeviceCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteDeviceCommandHandler).GetTypeInfo().Assembly,

                // SSL Handlers
                typeof(DetailSslQueryHandler).GetTypeInfo().Assembly

            });

            return services;
        }
    }
}
