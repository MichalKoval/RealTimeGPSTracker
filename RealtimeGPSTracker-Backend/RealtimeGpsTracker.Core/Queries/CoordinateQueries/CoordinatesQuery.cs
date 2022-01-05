using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.CoordinateResponses;
using System;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Queries.CoordinateQueries
{
    public class CoordinatesQuery : AuthorizedQuery<CoordinatesResponse>
    {
        [JsonProperty("DeviceId")]
        [Required]
        public string DeviceId { get; set; }

        [JsonProperty("TripId")]
        [Required]
        public string TripId { get; set; }

        [JsonProperty("Start")]
        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Wrong DateTime format. (Required format: yyyy-MM-ddTHH:mm:ss)")]
        public DateTime Start { get; set; }

        [JsonProperty("End")]
        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Wrong DateTime format. (Required format: yyyy-MM-ddTHH:mm:ss)")]
        public DateTime End { get; set; }
    }
}