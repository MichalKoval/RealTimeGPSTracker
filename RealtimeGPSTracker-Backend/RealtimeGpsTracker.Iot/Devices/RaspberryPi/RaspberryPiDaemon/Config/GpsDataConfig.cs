using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPiDaemon.Config
{
    public class GpsDataConfig
    {
        /// <summary>
        /// Maximum number of coordinates sent in one message.
        /// </summary>
        [JsonProperty("PayloadSize", Required = Required.Always)]
        public int PayloadSize { get; set; } = 100;

        [JsonProperty("PersistDataLocally", Required = Required.Default)]
        public bool PersistDataLocally { get; set; } = false;

        [JsonProperty("LocalDataStorageFileName", Required = Required.AllowNull)]
        public string LocalDataStorageFileName { get; set; } = null;
    }
}
