using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RealtimeGpsTracker.Core.Entities
{
    public class GpsCoordinate
    {
        public GpsCoordinate()
        {
        }

        [JsonPropertyName("id")]
        [Key]
        public string GpsCoordinateId { get; set; }

        public DateTime Time { get; set; }

        [MaxLength(64)]
        public string Lt { get; set; }

        [MaxLength(64)]
        public string Lg { get; set; }

        [MaxLength(64)]
        public string Speed { get; set; }

        [JsonIgnore]
        [ForeignKey("Trip")]
        public string TripId { get; set; }

        [JsonIgnore]
        public virtual Trip Trip { get; set; }
    }
}