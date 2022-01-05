using System.IO.Ports;
using Newtonsoft.Json;

namespace RaspberryPiDaemon.Config
{
    public class DeviceConfig
    {
        [JsonProperty("Id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("CoordinatesSendInterval", Required = Required.Always)]
        public int CoordinatesSendInterval { get; set; } = 1000;

        [JsonProperty("GpsDataCollectorSimulated", Required = Required.Default)]
        public bool GpsDataCollectorSimulated { get; set; } = false;

        [JsonProperty("GpsDataSenderSimulated", Required = Required.Default)]
        public bool GpsDataSenderSimulated { get; set; } = false;
    }
}
