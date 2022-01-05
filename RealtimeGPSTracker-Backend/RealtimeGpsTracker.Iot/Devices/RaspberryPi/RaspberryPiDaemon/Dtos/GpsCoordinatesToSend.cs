using System.Collections.Generic;
using Newtonsoft.Json;
using RaspberryPiDaemon.Entities;

namespace RaspberryPiDaemon.Dtos
{
    public class GpsCoordinatesToSend
    {
        [JsonProperty("id")]
        public string DeviceID { get; set; }

        [JsonProperty("coords")]
        public List<GpsCoordinate> Coordinates { get; set; }
    }
}
