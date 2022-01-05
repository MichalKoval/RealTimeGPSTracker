using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Services
{
    public class HostedServiceStarter<T> : IHostedService where T : IHostedService
    {
        private readonly T _hostedService;

        public HostedServiceStarter(T hostedService)
        {
            _hostedService = hostedService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _hostedService.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _hostedService.StopAsync(cancellationToken);
        }
    }
}
