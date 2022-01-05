using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.Services
{
    public class DeviceStatusWorkerService : IHostedService, IDisposable
    {
        public class DeviceStatusWorkerSettings
        {
            // Constructor needed for binding
            public DeviceStatusWorkerSettings() { }

            /// <summary>
            /// Interval which indicates how often statuses of devices should be refreshed (in minutes).
            /// </summary>
            public int Interval { get; set; } = 20;
        }


        private readonly ILogger<DeviceStatusWorkerService> _logger;
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly DeviceStatusWorkerSettings _deviceStatusWorkerSettings;
        private Task _deviceStatusTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public DeviceStatusWorkerService(
            ILogger<DeviceStatusWorkerService> logger,
            IGpsDeviceRepository gpsDeviceRepository,
            IOptions<DeviceStatusWorkerSettings> deviceStatusWorkerSettings
            )
        {
            _logger = logger;
            _gpsDeviceRepository = gpsDeviceRepository;
            _deviceStatusWorkerSettings = deviceStatusWorkerSettings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting device status worker service ...");

            _deviceStatusTask = RunAsync(_cancellationTokenSource.Token);

            return _deviceStatusTask.IsCompleted ? _deviceStatusTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping device status worker service ...");


            if (_deviceStatusTask == null)
            {
                return;
            }

            try
            {
                _cancellationTokenSource.Cancel();
            }
            finally
            {

                await Task.WhenAny(_deviceStatusTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // Repeatedly gets all devices and checks for how long they didn't send any new data.
            // Based on how much time passed since last coordinate came in a status of each device will be defined (Online, Away, Offline).

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Refreshing statuses of devices ...");

                await _gpsDeviceRepository.RefreshStatusesOfDevices();

                // Refreshing only every N milliseconds
                // Interval value is converted from minutes to milliseconds
                await Task.Delay((int)TimeSpan.FromMinutes(_deviceStatusWorkerSettings.Interval).TotalMilliseconds, cancellationToken);
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing device status worker service ...");
        }
    }
}
