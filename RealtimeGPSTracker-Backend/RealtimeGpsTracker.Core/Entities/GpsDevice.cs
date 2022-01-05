using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RealtimeGpsTracker.Core.Entities
{
    public class GpsDevice
    {
        public GpsDevice()
        {
        }

        [JsonPropertyName("id")]
        [Key]
        public string GpsDeviceId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        [MaxLength(16)]
        public string Color { get; set; }

        public int Interval { get; set; }

        [JsonIgnore]
        public bool IntervalChanged { get; set; } = false;

        public GpsDeviceStatus Status { get; set; } = GpsDeviceStatus.Offline;

        [JsonIgnore]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Trip> Trips { get; set; }
    }
}