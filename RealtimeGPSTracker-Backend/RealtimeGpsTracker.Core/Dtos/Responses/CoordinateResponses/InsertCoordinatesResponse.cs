using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses;

namespace RealtimeGpsTracker.Core.Dtos.Responses.CoordinateResponses
{
    public class InsertCoordinatesResponse : Response
    {
        public enum DeviceMessage
        {
            NONE, //0
            CHANGE_INTERVAL, //1
            DISABLE //2
        }

        public class DeviceSettings
        {
            [JsonProperty("interval")]
            public int Interval { get; set; }

            //TODO: other future settings
        }

        [JsonProperty("msg")]
        public DeviceMessage Message { get; set; } = DeviceMessage.NONE;

        [JsonProperty("settings")]
        public DeviceSettings Settings { get; set; } = null;  
    }
}