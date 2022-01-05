using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RaspberryPiDaemon.Config;
using RaspberryPiDaemon.Interfaces.Services;
using RaspberryPiDaemon.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RaspberryPiDaemon
{
    ///TODO:
    // GPSDataCollector pouziva vlastne vlakno ==> zbieranie GPS suradnic
    //gPSDataCollector.Start();

    // TODO: pockat, kym nastane GPS FIX a az potom odosielat data

    // GPSDataSender pouziva vlastny BackGroundWorker ==> odosielanie GPS suradnic v predefinovam intervale
    //gPSDataSender.Start();

    //TODO: co robit ak dlhodobo nebudu ziadne data k odoslaniu (GPS modul nezachytil suradnice)

    class Program
    {
        public async static Task Main(string[] args)
        {
            for (; ; )
            {
                Console.WriteLine("waiting for debugger attach");
                if (Debugger.IsAttached) break;

                System.Threading.Thread.Sleep(1000);
            }

            var builder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }

                config.AddJsonFile("appsettings.json", optional: true);
                //config.SetFileProvider();
                config.AddJsonFile("SimulationData/coordinates_data_" + new Random().Next(1, 31) + ".json", optional: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();

                IConfigurationSection deviceConfigSection = hostContext.Configuration.GetSection(nameof(DeviceConfig));
                DeviceConfig deviceConfig = deviceConfigSection.Get<DeviceConfig>();

                services.Configure<DeviceConfig>(deviceConfigSection);

                // Checking if device is in the simulation mode in order to choose right server configuration.
                if (deviceConfig.GpsDataSenderSimulated)
                {
                    services.Configure<ServerSimulationConfig>(hostContext.Configuration.GetSection(nameof(ServerSimulationConfig)));
                }
                else
                {
                    services.Configure<ServerConfig>(hostContext.Configuration.GetSection(nameof(ServerConfig)));
                }

                services.Configure<GsmConfig>(hostContext.Configuration.GetSection(nameof(GsmConfig)));
                services.Configure<SerialPortConfig>(hostContext.Configuration.GetSection(nameof(SerialPortConfig)));
                //services.Configure<ATSerialPortConfig>(hostContext.Configuration.GetSection(nameof(ATSerialPortConfig)));
                //services.Configure<NmeaSerialPortConfig>(hostContext.Configuration.GetSection(nameof(NmeaSerialPortConfig)));
                services.Configure<GpsDataConfig>(hostContext.Configuration.GetSection(nameof(GpsDataConfig)));
                services.Configure<ButtonConfig>(hostContext.Configuration.GetSection(nameof(ButtonConfig)));


                /// Hosted services will be started in the same order as they are added.
                // Adding AT Commands service. Service makes initial communication with a GNSS/GSM device.
                if ((!deviceConfig.GpsDataCollectorSimulated && deviceConfig.GpsDataSenderSimulated) ||
                    (deviceConfig.GpsDataCollectorSimulated && !deviceConfig.GpsDataSenderSimulated) ||
                    (!deviceConfig.GpsDataCollectorSimulated && !deviceConfig.GpsDataSenderSimulated)
                )
                {
                    services.AddSingleton<ATCommandService>();
                    services.AddHostedService<HostedServiceStarter<ATCommandService>>();
                }

                // Adding GPS Coordinates file storage. (In case that device crashes or is out of the power source data will persist.)
                services.AddSingleton<GpsCoordinateFileStorageService>();
                services.AddHostedService<HostedServiceStarter<GpsCoordinateFileStorageService>>();

                // Added GPS Coordinates concurrent queue which holds coordinates that need to be processed.
                services.AddSingleton<GpsCoordinateService>();
                services.AddHostedService<HostedServiceStarter<GpsCoordinateService>>();             

                // Checking if device is in the simulation mode in order to choose right data collector and sender service.
                // In one case AT command service is not necessary.
                if (deviceConfig.GpsDataCollectorSimulated && deviceConfig.GpsDataSenderSimulated)
                {
                    // Simulated data collector
                    services.AddSingleton<GpsDataCollectorSimulationService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataCollectorSimulationService>>();

                    // Simulated data sender
                    services.AddSingleton<GpsDataSenderSimulationService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataSenderSimulationService>>();
                }
                else if (deviceConfig.GpsDataCollectorSimulated && !deviceConfig.GpsDataSenderSimulated)
                {
                    // Simulated data collector
                    services.AddSingleton<GpsDataCollectorSimulationService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataCollectorSimulationService>>();

                    // Real data sender
                    services.AddSingleton<GpsDataSenderService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataSenderService>>();

                }
                else if (!deviceConfig.GpsDataCollectorSimulated && deviceConfig.GpsDataSenderSimulated)
                {
                    // Real data collector
                    services.AddSingleton<GpsDataCollectorService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataCollectorService>>();

                    // Simulated data sender
                    services.AddSingleton<GpsDataSenderSimulationService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataSenderSimulationService>>();
                }
                else
                {
                    // Real data collector
                    services.AddSingleton<GpsDataCollectorService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataCollectorService>>();

                    // Real data sender
                    services.AddSingleton<GpsDataSenderService>();
                    services.AddHostedService<HostedServiceStarter<GpsDataSenderService>>();
                }

                services.AddSingleton<ButtonService>();
                services.AddHostedService<HostedServiceStarter<ButtonService>>();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
            });            

            try
            {
                await builder.RunConsoleAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
