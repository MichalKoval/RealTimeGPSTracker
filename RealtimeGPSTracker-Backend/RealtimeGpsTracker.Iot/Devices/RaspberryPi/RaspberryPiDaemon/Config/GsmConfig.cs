using System;
using System.IO.Ports;
using Newtonsoft.Json;

namespace RaspberryPiDaemon.Config
{
    public class GsmConfig
    {
        [JsonProperty("Apn", Required = Required.Always)]
        public string Apn { get; set; } = string.Empty;

        [JsonProperty("ApnUser", Required = Required.Always)]
        public string ApnUser { get; set; } = string.Empty;

        [JsonProperty("ApnPwd", Required = Required.Always)]
        public string ApnPwd { get; set; } = string.Empty;
    }
}
