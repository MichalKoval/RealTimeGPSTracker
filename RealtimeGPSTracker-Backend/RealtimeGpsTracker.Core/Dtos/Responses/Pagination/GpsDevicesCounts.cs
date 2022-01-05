using Newtonsoft.Json;

namespace RealtimeGpsTracker.Core.Dtos.Responses.Pagination
{
    public class GpsDevicesCounts
    {
        [JsonProperty("offlineCount")]
        public int OfflineCount { get; set; }

        [JsonProperty("onlineCount")]
        public int OnlineCount { get; set; }

        [JsonProperty("awayCount")]
        public int AwayCount { get; set; }

        [JsonProperty("allCount")]
        public int AllCount { get; set; }
    }
}
