using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.CoordinateResponses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.CoordinateCommands
{
    public class InsertCoordinatesCommand : AnonymousCommand<InsertCoordinatesResponse>
    {
        public class GpsCoordinateReceived
        {
            [JsonProperty("Dt")]
            [Required(ErrorMessage = "DateTime is required.")]
            [DataType(DataType.DateTime, ErrorMessage = "Wrong DateTime format. (Required format: yyyy-MM-ddTHH:mm:ss)")]
            public DateTime? Date_Time { get; set; }

            [JsonProperty("Lt")]
            [Required(ErrorMessage = "Latitude is required.")]
            [RegularExpression("^[-+]?([1-8]?\\d(\\.\\d+)?|90(\\.0+)?)$", ErrorMessage = "Wrong Latitude format.")]
            public string Latitude { get; set; }

            [JsonProperty("Lg")]
            [Required(ErrorMessage = "Longitude is required.")]
            [RegularExpression("^[-+]?(180(\\.0+)?|((1[0-7]\\d)|([1-9]?\\d))(\\.\\d+)?)$", ErrorMessage = "Wrong Longitude format.")]
            public string Longitude { get; set; }

            [JsonProperty("S")]
            public string Speed { get; set; }
        }

        [JsonProperty("DeviceId")]
        [RegularExpression("(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", ErrorMessage = "Wrong device id format.")]
        [Required(ErrorMessage = "GPS Device Id is required.")]
        public string DeviceId { get; set; }

        // To be able to distinguish if it is still the same trip.
        [JsonProperty("TripId")]
        [RegularExpression("(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", ErrorMessage = "Wrong trip id format.")]
        [Required(ErrorMessage = "Current GPS Device Trip is required.")]
        public string TripId { get; set; }

        [JsonProperty("Coords")]
        [Required(ErrorMessage = "GPS Coordinates are required.")]
        public IList<GpsCoordinateReceived> Coordinates { get; set; }
    }
}