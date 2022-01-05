using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RaspberryPiDaemon.Config;
using RaspberryPiDaemon.Dtos;
using RaspberryPiDaemon.Entities;
using RaspberryPiDaemon.Interfaces.Services;

namespace RaspberryPiDaemon.Services
{
    public class GpsDataSenderService : IHostedService, IDisposable
    {
        private readonly ILogger<GpsDataSenderService> _logger;
        private readonly DeviceConfig _deviceConfig;
        private readonly ServerConfig _serverConfig;
        private readonly GsmConfig _gsmConfig;
        private readonly GpsDataConfig _gpsDataConfig;

        private readonly ATCommandService _atCommandService;
        private readonly GpsCoordinateService _gpsCoordinateService;

        private BackgroundWorker _backgroundWorker;
        private System.Timers.Timer _sendTimer;

        public GpsDataSenderService(
            ILogger<GpsDataSenderService> logger,
            IOptions<DeviceConfig> deviceConfig,
            IOptions<ServerConfig> serverConfig,
            IOptions<GsmConfig> gsmConfig,
            IOptions<GpsDataConfig> gpsDataConfig,

            ATCommandService atCommandService,
            GpsCoordinateService gpsCoordinateService
            )
        {
            _logger = logger;
            _deviceConfig = deviceConfig.Value;
            _serverConfig = serverConfig.Value;
            _gsmConfig = gsmConfig.Value;
            _gpsDataConfig = gpsDataConfig.Value;

            _atCommandService = atCommandService;
            _gpsCoordinateService = gpsCoordinateService;
            
            _backgroundWorker = new BackgroundWorker();
            _sendTimer = new System.Timers.Timer();

            // Handlers pre uplynuty casovac a pre worker vlakno na pozadi
            _backgroundWorker.DoWork += BackgroundWorkerDoWork_Handler;
            _sendTimer.Elapsed += SendTimerElapsed_Handler;

        }

        private void SendTimerElapsed_Handler(object sender, ElapsedEventArgs e)
        {
            //if (!_backgroundWorker.IsBusy)
            //    _backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorkerDoWork_Handler(object sender, DoWorkEventArgs e)
        {
            // Ak sa nepodari odoslat data na server tak to skusime znova, dalsi backgroundWorker nezacne, kym sa neskonci tento
            //while (!SendData()) { }
        }

        private void Start()
        {
            _sendTimer.Start();
        }

        private void Stop()
        {
        
        }

        private bool SendData()
        {
            List<GpsCoordinate> coordsToSend;

            if ((coordsToSend = _gpsCoordinateService.GetCoordinates(_gpsDataConfig.PayloadSize)) != null)
            {
                GpsCoordinatesToSend gPSDataToSend = new GpsCoordinatesToSend();

                gPSDataToSend.DeviceID = _deviceConfig.Id;
                gPSDataToSend.Coordinates = coordsToSend;

                string dataStringToSend = JsonConvert.SerializeObject(gPSDataToSend);
                string serverResponse;

                if ((serverResponse = SendHTTPPOST(dataStringToSend)) != null)
                {
                    Console.WriteLine("Server response: {serverResponse}");

                    //TODO: evaluate response, temporary solution
                    return false;
                }
                else return false;

            }
            // Zatial neboli ziadne data na odoslanie
            else return true;
        }

        private string SendHTTPPOST(string postData)
        {
            //SendPOSTAsync().GetAwaiter().GetResult();

            string serverAddress = "";

            if (_serverConfig.SslEnabled) { serverAddress += "https://"; }
            else { serverAddress += "http://"; }

            serverAddress += _serverConfig.Url;
            serverAddress += _serverConfig.ApiRequest;
            serverAddress += _deviceConfig.Id;
            serverAddress += "/post";

            var request = (HttpWebRequest)WebRequest.Create(serverAddress);

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = _serverConfig.ApiRequestMethod;
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting gps data sender service ...");
            Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping gps data sender service ...");
            Stop();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing gps data sender service ...");
        }
    }
}









// OLD Code -----------------------------------------------------------------------
//private static readonly HttpClient client = new HttpClient();

//private async Task SendPOSTAsync()
//{
//    Stopwatch sw = new Stopwatch();

//    sw.Start();

//    var values = new Dictionary<string, string>
//        {
//           { "thing1", "hello" },
//           { "thing2", "world" }
//        };

//    var content = new FormUrlEncodedContent(values);

//    var response = await client.PostAsync("http://mkwp.cz/GPSDevices/729d4fed-5cb7-4255-87af-50ef7cbe5b80/websocket", content);

//    var serverResponse = await response.Content.ReadAsStringAsync();

//    sw.Stop();

//    Console.WriteLine("\nElapsed time={0} seconds", sw.Elapsed.Seconds);
//    Console.WriteLine("Server response:\n{0}", serverResponse);
//}
