using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspberryPiDaemon.Config;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Services
{
    public class ButtonService : IHostedService, IDisposable
    {
        private readonly ILogger<ButtonService> _logger;
        private readonly ButtonConfig _buttonConfig;

        public ButtonService(
            ILogger<ButtonService> logger,
            IOptions<ButtonConfig> buttonConfig

            )
        {
            _logger = logger;
            _buttonConfig = buttonConfig.Value;
        }        

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting shutdown button service and listening for button push ...");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping shutdown button service ...");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing shutdown button service ...");
        }
    }
}
