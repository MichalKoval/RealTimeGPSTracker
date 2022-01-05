using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspberryPiDaemon.Config;
using RaspberryPiDaemon.Interfaces.Services;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Services
{
    public class ATCommandService : IHostedService, IDisposable
    {
        private readonly ILogger<ATCommandService> _logger;
        private readonly SerialPortConfig _atSerialPortConfig;
        private static AdvancedSerialPort _atSerialPort;

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Debug.WriteLine(((AdvancedSerialPort)sender).ReadExisting());
        }

        private AdvancedSerialPort InitializeSerialPort(SerialPortConfig config)
        {
            AdvancedSerialPort serialPort = new AdvancedSerialPort();

            serialPort.PortName = config.Name;
            serialPort.BaudRate = config.BaudRate;
            serialPort.Parity = config.Parity;
            serialPort.DataBits = config.DataBits;
            serialPort.StopBits = config.StopBits;
            serialPort.Handshake = config.Handshake;            
            serialPort.ReadTimeout = config.ReadTimeOut;
            serialPort.WriteTimeout = config.WriteTimeOut;
            
            // For debug purposes
            //serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

            return serialPort;
        }

        public ATCommandService(
            ILogger<ATCommandService> logger,
            IOptions<SerialPortConfig> atSerialPortConfig
            )
        {
            _logger = logger;
            _atSerialPortConfig = atSerialPortConfig.Value;
            _atSerialPort = InitializeSerialPort(_atSerialPortConfig);
        }

        private bool TryToWakeUpDevice()
        {
            string result = null;
            int numberOfTries = 0;

            while (result == null && numberOfTries < 2)
            {
                result = Send(new ATCommand(ATCommandType.EMPTY, ATResponseType.OK));
                numberOfTries++;
            }

            return (result != null) ? true : false;
        }

        public string Send(ATCommand command)
        {
            // If read/write operation exceeds ReadTimeOut/WriteTimeOut
            // the send operation is then stopped to avoid hang of an entire application.
            try
            {
                _atSerialPort.Write(command.CommandStr);
                return _atSerialPort.ReadTo(command.ExpectedResponseStr);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (TimeoutException)
            {
                // Didn't get expected response
                return null;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting AT command service ...");

            if (_atSerialPort.Start())
            {
                // Testing if device responds to AT commands
                if (TryToWakeUpDevice())
                {
                    return Task.CompletedTask;
                }
                else
                {
                    return Task.FromException(new Exception("Device does not respond to AT commands. Can't start AT command service."));
                }
            }
            else
            {
                return Task.FromException(new Exception("AT command service couldn't start."));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping AT command service ...");
            _atSerialPort.Stop();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing AT command service ...");
        }
    }
}
