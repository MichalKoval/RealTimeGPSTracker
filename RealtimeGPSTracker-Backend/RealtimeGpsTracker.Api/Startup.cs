using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealtimeGpsTracker.Api.Middleware;
using RealtimeGpsTracker.Application.Services;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Helpers;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Interfaces.Services;
using RealtimeGpsTracker.Infrastructure.Data.EntityFramework;
using RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Repositories;
using RealtimeGPSTracker.Application.Helpers;
using RealtimeGPSTracker.Application.Hubs;
using RealtimeGPSTracker.Application.Services;
using RealtimeGPSTracker.Application.Services.HubServices;
using System;

namespace RealtimeGpsTracker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);

            // Logging settings
            services.AddLogging(config =>
            {
                // clear out default configuration
                config.ClearProviders();

                config.AddConfiguration(Configuration.GetSection("Logging"));
                config.AddEventLog();
                config.AddEventSourceLogger();

                if (Environment.IsDevelopment())
                {
                    config.AddConsole();
                    config.AddDebug();
                }
            });

            if (Environment.IsDevelopment())
            {
                //services.AddEntityFrameworkSqlite().AddDbContext<BaseDbContext>();

                services.AddDbContext<BaseDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DevelopmentDbConnection")));
            }
            else
            {
                //services.AddEntityFrameworkSqlite().AddDbContext<BaseDbContext>();

                services.AddDbContext<BaseDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("ProductionDbConnection")));
            }
            
            // Adds user identity (database model) with user role (Roles are not used)
            services.AddIdentity<User, Role>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<BaseDbContext>().AddDefaultTokenProviders();

            // Adds (injects into dependency injection) strongly typed Jwt settings from app configuration file (appsettings.json)
            services.AddJwtSettings(Configuration);

            // Adds strongly typed SSL settings from app configuration file (appsettings.json)
            services.AddSslSettings(Configuration);

            // Adds strongly typed device status worker settings from app configuration file (appsettings.json)
            services.AddDeviceStatusWorkerSettings(Configuration);

            // Adds custom JWT Authentication Middleware.
            services.AddJwtAuthentication();

            // Adds custom JWT Authorization Middleware.
            services.AddJwtAuthorization();

            // Adds Cors policy
            services.AddCors(o => o.AddPolicy("EnableCorsPolicy", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build();
            }));

            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<ISslService, SslService>();

            // Add http context accessor
            services.AddHttpContextAccessor();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            
            // Adding SignalRServices.
            services.AddSingleton<IUserToConnectionMapper<string>, UserToConnectionMapper<string>>();
            services.AddSingleton<IDeviceHubService, DeviceHubService>();
            services.AddSingleton<ITripHubService, TripHubService>();
            services.AddSingleton<ICoordinateHubService, CoordinateHubService>();
            services.AddSingleton<IUserHubService, UserHubService>();

            // Adding repositories
            services.AddTransient<IGpsCoordinateRepository, GpsCoordinateRepository>();
            services.AddTransient<ITripRepository, TripRepository>();
            services.AddTransient<IGpsDeviceRepository, GpsDeviceRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Adds device status worker service which will be running at the background for an app life
            //services.AddTransient<IHostedService, DeviceStatusWorkerService>();

            // Adds object mapping middleware between database models and DTOs.
            services.AddAutoMapper();

            // Adds MediatR middleware with handlers to support CQRS pattern.
            services.AddMediatRHandlers();

            // Adds custom DateTime model binder to controllers
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            });

            // Adds controllers with views + Fluent validators for MediatR commands
            services.AddControllersWithViews().AddFluentValidation();
            services.AddFluentValidators();

            // In production, the Angular files will be served from this directory.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Do initial database migration if there wasn't any before.
            app.InitializeDatabase();

            app.UseCors("EnableCorsPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<CoordinateHub>("/coordinatehub");
                endpoints.MapHub<TripHub>("/historyhub");
                endpoints.MapHub<DeviceHub>("/devicehub");
                endpoints.MapHub<UserHub>("/userhub");
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints
            //        .MapControllerRoute(
            //            name: "default",
            //            pattern: "{controller}/{action=Index}/{id?}");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    // Authorized user is required to access any API route except for auth/login and auth/register (attribute [AllowAnonymous] is used).
                    .RequireAuthorization();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
