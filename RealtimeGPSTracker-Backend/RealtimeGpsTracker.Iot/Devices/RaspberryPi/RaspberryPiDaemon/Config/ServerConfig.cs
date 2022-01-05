using System.IO.Ports;
using Newtonsoft.Json;

namespace RaspberryPiDaemon.Config
{
    public class ServerConfig
    {
        [JsonProperty("SslEnabled", Required = Required.Default)]
        public bool SslEnabled { get; set; } = false;

        [JsonProperty("Url", Required = Required.Always)]
        public string Url { get; set; } = null;

        [JsonProperty("ApiRequest", Required = Required.Always)]
        public string ApiRequest { get; set; } = null;

        [JsonProperty("ApiRequestMethod", Required = Required.Always)]
        public string ApiRequestMethod { get; set; } = "POST";
    }
}
