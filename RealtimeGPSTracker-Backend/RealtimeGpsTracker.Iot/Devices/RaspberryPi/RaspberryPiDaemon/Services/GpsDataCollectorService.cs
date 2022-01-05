using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspberryPiDaemon.Config;
using RaspberryPiDaemon.Deprecated.Protocols;
using RaspberryPiDaemon.Entities;
using RaspberryPiDaemon.Interfaces.Services;
using System;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Services
{
    public class GpsDataCollectorService : IHostedService, IDisposable
    {
        private readonly ILogger<GpsDataCollectorService> _logger;
        private readonly DeviceConfig _deviceConfig;
        
        private readonly ATCommandService _atCommandService;
        private readonly GpsCoordinateService _gpsCoordinateService;

        private Task _collectorTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public GpsDataCollectorService(
            ILogger<GpsDataCollectorService> logger,
            IOptions<DeviceConfig> deviceConfig,
            ATCommandService atCommandService,
            GpsCoordinateService gpsCoordinateService         
            )
        {
            _logger = logger;
            _deviceConfig = deviceConfig.Value;
            _atCommandService = atCommandService;
            _gpsCoordinateService = gpsCoordinateService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting gps data collector service ...");

            // Give other services a time to warm up.
            Task.Delay(100, cancellationToken);

            _collectorTask = RunAsync(_cancellationTokenSource.Token);

            return _collectorTask.IsCompleted ? _collectorTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping gps data collector service ...");


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

            //try
            //{
            //    while (!cancellationToken.IsCancellationRequested)
            //    {
            //        // While loop musi pokracovat aj napriek tomu,
            //        // ze sa nepodarilo nacitat nejaky z prichadzajucich riadkov korektne a bola vyvolana exception
            //        // V tomto pripade je lepsie ponechat try catch vo vnutri while loop, mozny exception overhead pocas behu programu

            //        string lineWithData = _nmeaSerialPort.ReadLine();

            //        // Pokusime sa vyparsovat GNRMC spravu prichadzajucu zo serial portu
            //        NMEA_Protocol.GNRMC parsed_GNRMC;

            //        if (lineWithData.StartsWith("$GNRMC"))
            //        {
            //            if ((parsed_GNRMC = NMEA_Protocol.TryParse_GNRMC(lineWithData)) != null)
            //            {
            //                GpsCoordinate gpsCoord;
            //                // Ak GNRMC sprava obsahuje dostatocne informacie o polohe, tak danu polohu pridame do zoznamu nazbieranych poloh
            //                if ((gpsCoord = GpsCoordinate.GetFrom(parsed_GNRMC)) != null)
            //                {
            //                    // _gpsCoordinateService reprezetuje Thread-Safe ConcurrentQueue do ktorej pridavame a odoberame suradnice asynchronne

            //                    _gpsCoordinateService.Add(gpsCoord);
            //                }
            //            }
            //            else
            //            {
            //                Console.WriteLine("Corrupted GNRMC_NMEA message.");
            //            }

            //            // Reading only every N miliseconds
            //            await Task.Delay(_deviceConfig.CoordinatesSendInterval, cancellationToken);
            //        }

            //        Console.WriteLine(lineWithData);                    
            //    }
            //}
            //catch (TimeoutException toe)
            //{
            //    //Nacitanie dat po najblizsi newline nesmie prekrocit urcity casovy interval
            //    Console.WriteLine(toe.Message);
            //}

            
            // Powering up GNSS device to collect coordinates
            if (_atCommandService.Send(new ATCommand(ATCommandType.CGNSPWR, ATResponseType.OK, "1")) != null)
            {
                string coordStr = null;
                string[] coordStrSplitted = null;

                while (!cancellationToken.IsCancellationRequested)
                {
                    coordStr = _atCommandService.Send(new ATCommand(ATCommandType.CGNSINF, ATResponseType.OK));

                    if (coordStr != null)
                    {
                        coordStrSplitted = coordStr
                            .Trim()
                            .Substring(10)
                            .Split(',');

                        if (coordStrSplitted[0].Equals("0"))
                        {
                            _logger.LogError("GNSS is not powered up.");
                            break;
                        }
                        else if (coordStrSplitted[0].Equals("1"))
                        {
                            if (coordStrSplitted[1].Equals("0"))
                            {
                                _logger.LogInformation("Waiting for GNSS fix ...");
                            }
                            else if (coordStrSplitted[1].Equals("1"))
                            {
                                if (DateTime.TryParseExact(coordStrSplitted[2], "yyyyMMddHHmmss.sss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime) &&
                                    double.TryParse(coordStrSplitted[3], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double latitude) &&
                                    double.TryParse(coordStrSplitted[4], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double longitude) &&
                                    double.TryParse(coordStrSplitted[6], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double speed)
                                    )
                                {
                                    _gpsCoordinateService.Add(
                                        new GpsCoordinate(
                                            dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                            latitude,
                                            longitude,
                                            speed
                                            )
                                        );
                                }
                                else
                                {
                                    _logger.LogError("GNSS coordinate has a wrong format");
                                    break;
                                }
                            }
                            else
                            {
                                _logger.LogError("Couldn't read GNSS coordinate.");
                                break;
                            }
                        }
                        else
                        {
                            _logger.LogError("Couldn't read GNSS coordinate.");
                            break;
                        }
                    }
                    else
                    {
                        _logger.LogError("Couldn't read GNSS coordinate.");
                        break;
                    }

                    // Reading only every N miliseconds
                    await Task.Delay(_deviceConfig.CoordinatesSendInterval, cancellationToken);
                }

                // Powering down the GNSS device
                if (_atCommandService.Send(new ATCommand(ATCommandType.CGNSPWR, ATResponseType.OK, "0")) == null)
                {
                    _logger.LogError("Couldn't power down GNSS.");
                }
            }
            else
            {
                _logger.LogError("Couldn't power up GNSS.");
            }

        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing gps data collector service ...");
        }
    }
}
