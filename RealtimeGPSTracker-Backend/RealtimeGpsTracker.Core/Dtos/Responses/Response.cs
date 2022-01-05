using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RealtimeGpsTracker.Core.Dtos.Responses
{
    public class Response
    {
        [JsonIgnore]
        public bool Success { get; set; } = false;
        public List<string> Errors { get; set; } = new List<string>();

        public bool ShouldSerializeErros()
        {
            return !Success;
        }
    }
}
