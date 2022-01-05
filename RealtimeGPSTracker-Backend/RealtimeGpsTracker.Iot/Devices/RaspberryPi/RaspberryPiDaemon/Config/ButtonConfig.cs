using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPiDaemon.Config
{
    public class ButtonConfig
    {
        [JsonProperty("ButtonPin", Required = Required.Default)]
        public int ButtonPin { get; set; } = 26; // GPIO 26
    }
}
