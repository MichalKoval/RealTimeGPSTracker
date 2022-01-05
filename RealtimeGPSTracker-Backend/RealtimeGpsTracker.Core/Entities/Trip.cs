using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RealtimeGpsTracker.Core.Entities
{
    public class Trip
    {
        public Trip()
        {
        }

        [JsonPropertyName("id")]
        [Key]
        public string TripId { get; set; }

        [JsonPropertyName("datetimeStart")]
        public DateTime Start { get; set; }

        [JsonPropertyName("datetimeEnd")]
        public DateTime End { get; set; }

        public int Distance { get; set; } = 0;

        [JsonIgnore]
        public bool InProgress  { get; set; } = false;

        [JsonIgnore]
        public virtual ICollection<GpsCoordinate> GpsCoordinates { get; set; }

        // Chceme vediet, ktorym zariadenim boli data namerane
        [JsonIgnore]
        [ForeignKey("GpsDevice")]
        public string GpsDeviceId { get; set; }

        [JsonIgnore]
        public virtual GpsDevice GpsDevice { get; set; }
    }
}