using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using static RealtimeGpsTracker.Application.Services.JwtService;

namespace RealtimeGpsTracker.Api.Middleware
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            // Getting strongly typed Jwt Issuer, Audience, Key and MinutesValid settings
            JwtSettings jwtSettings = services.BuildServiceProvider().GetService<IOptions<JwtSettings>>().Value;

            // Getting Jwt Key from jwt settings to create Issuer (Server) sign in key
            byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtSettings.Key);
            SymmetricSecurityKey issuerSignInKey = new SymmetricSecurityKey(signingKeyBytes);
                      

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
                {
                    configureOptions.ClaimsIssuer = jwtSettings.Issuer;
                    configureOptions.RequireHttpsMetadata = false;
                    configureOptions.SaveToken = true;
                    configureOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = issuerSignInKey,
                        ValidateIssuerSigningKey = true,
                        
                        ValidateLifetime = true,                    
                        ClockSkew = TimeSpan.Zero
                    };

                });

            return services;
        }
    }
}
