using System.IO.Ports;
using Newtonsoft.Json;

namespace RaspberryPiDaemon.Config
{
    public class SerialPortConfig
    {
        [JsonProperty("Name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("BaudRate", Required = Required.Always)]
        public int BaudRate { get; set; } = 9600;

        [JsonProperty("Parity", Required = Required.Default)]
        public Parity Parity { get; set; } = Parity.None;

        [JsonProperty("DataBits", Required = Required.Default)]
        public int DataBits { get; set; } = 8;

        [JsonProperty("StopBits", Required = Required.Default)]
        public StopBits StopBits { get; set; } = StopBits.One;

        [JsonProperty("Handshake", Required = Required.Default)]
        public Handshake Handshake { get; set; } = Handshake.None;

        [JsonProperty("ReadTimeOut", Required = Required.Default)]
        public int ReadTimeOut { get; set; } = 1500;

        [JsonProperty("WriteTimeOut", Required = Required.Default)]
        public int WriteTimeOut { get; set; } = 500;
    }
}
