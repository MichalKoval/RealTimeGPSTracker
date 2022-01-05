using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspberryPiDaemon.Config;
using RaspberryPiDaemon.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Services
{
    public class GpsCoordinateFileStorageService : IHostedService, IDisposable
    {
        private readonly ILogger<GpsCoordinateService> _logger;
        private readonly GpsDataConfig _gpsDataConfig;

        public GpsCoordinateFileStorageService(
            ILogger<GpsCoordinateService> logger,
            IOptions<GpsDataConfig> gpsDataConfig
            )
        {
            _logger = logger;
            _gpsDataConfig = gpsDataConfig.Value;
        }        

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting gps coordinates file storage service ...");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping gps coordinates file storage service ...");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing coordinates file storage service ...");
        }
    }
}
