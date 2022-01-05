using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspberryPiDaemon.Config;
using RaspberryPiDaemon.Deprecated.Protocols;
using RaspberryPiDaemon.Entities;
using RaspberryPiDaemon.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Services
{
    public class GpsDataCollectorSimulationService : IHostedService, IDisposable
    {
        private readonly ILogger<GpsDataCollectorSimulationService> _logger;
        private readonly DeviceConfig _deviceConfig;
        private readonly Queue<Tuple<double, double>> _simulationData;
        
        private readonly GpsCoordinateService _gpsCoordinateService;

        private Task _collectorTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public GpsDataCollectorSimulationService(
            ILogger<GpsDataCollectorSimulationService> logger,
            IOptions<DeviceConfig> deviceConfig,
            IOptions<Queue<Tuple<double, double>>> simulationData,
            GpsCoordinateService gpsCoordinateService       
            )
        {
            _logger = logger;
            _deviceConfig = deviceConfig.Value;
            _simulationData = simulationData.Value;
            _gpsCoordinateService = gpsCoordinateService;
        }

        public TimeZoneInfo TimeZone { get; set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting gps data collector simulation service ...");

            _collectorTask = RunAsync(_cancellationTokenSource.Token);

            return _collectorTask.IsCompleted ? _collectorTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping gps data collector simulation service ...");


            if (_collectorTask == null)
            {
                return;
            }

            try
            {
                _cancellationTokenSource.Cancel();
            }
            finally
            {

                await Task.WhenAny(_collectorTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reading coordinates");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                Tuple<double, double> coord = _simulationData.Dequeue();
                
                _gpsCoordinateService.Add(
                    new GpsCoordinate
                    {
                        DateTime = DateTime.UtcNow.ToString(),
                        Lt = coord.Item1,
                        Lg = coord.Item2,
                        Speed = 0.0
                    }
                );
                
                // Reading only every N miliseconds
                await Task.Delay(_deviceConfig.CoordinatesSendInterval, cancellationToken);
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing gps data collector simulation service ...");
        }
    }
}
