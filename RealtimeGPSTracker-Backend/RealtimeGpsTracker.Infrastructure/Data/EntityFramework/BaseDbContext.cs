using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using RealtimeGpsTracker.Core.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework
{
    public class BaseDbContext : IdentityDbContext<User, Role, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public BaseDbContext(DbContextOptions<BaseDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
            //TODO: connection string by env!!!
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDbConnection"));
        }

        public DbSet<GpsDevice> GpsDevices { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<GpsCoordinate> GpsCoordinates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(new LoggerFactory());
            optionsBuilder.UseSqlServer(
                _dbConnection.ConnectionString, soa => soa.MigrationsAssembly("RealtimeGpsTracker.Infrastructure"));
            //optionsBuilder.UseSqlite("Filename=MyDatabase.db", soa => soa.MigrationsAssembly("RealtimeGpsTracker.Infrastructure"));
        }
    }
}