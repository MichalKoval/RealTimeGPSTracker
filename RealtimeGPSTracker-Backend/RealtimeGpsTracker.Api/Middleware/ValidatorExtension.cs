using FluentValidation.AspNetCore;
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
    public static class ValidatorExtension
    {
        public static IServiceCollection AddFluentValidators(this IServiceCollection services)
        {
            //services.AddTransient<IValidator<A>, AValidator>();

            return services;
        }
    }
}
