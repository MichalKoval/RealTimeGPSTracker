using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealtimeGpsTracker.Infrastructure.Data.EntityFramework;

namespace RealtimeGpsTracker.Api.Middleware
{
    public static class DatabaseContextExtension
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var serviceFactoryScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceFactoryScope.ServiceProvider.GetRequiredService<BaseDbContext>().Database.Migrate();
                //serviceFactoryScope.ServiceProvider.GetRequiredService<ProductionDbContext>().Database.Migrate();
            }

            return app;
        }
    }
}
