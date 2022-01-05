using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RealtimeGpsTracker.Core.Dtos.Requests
{
    public enum ServerResponseMsg_Deprecated
    {
        NONE, //0
        CHANGE_INTERVAL //1
    }

    public enum ServerResponseStatus_Deprecated
    {
        OK, //0
        ERROR //1
    }


    public class ServerResponse_Deprecated
    {
        [JsonProperty("status")]
        public ServerResponseStatus_Deprecated Status { get; set; }

        [JsonProperty("msg")]
        public ServerResponseMsg_Deprecated Message { get; set; }

        [JsonProperty("settings")]
        public ServerDeviceSettings_Deprecated Settings { get; set; }

        [JsonProperty("error")]
        public String Error { get; set; }
    }

    public class ServerDeviceSettings_Deprecated
    {
        [JsonProperty("interval")]
        public int Interval { get; set; }

        //TODO: other future settings
    }

    public class GpsDataPost_Deprecated
    {
        public GpsDataPost_Deprecated() { }

        [JsonProperty("deviceId")]
        [RegularExpression("(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", ErrorMessage = "Wrong device id format.")]
        [Required(ErrorMessage = "GPS Device Id is required.")]
        public string DeviceId { get; set; }

        //Aby sme vedeli rozlisit ze ide stale o tu istu zacatu cestu
        [JsonProperty("tripId")]
        [RegularExpression("(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", ErrorMessage = "Wrong trip id format.")]
        [Required(ErrorMessage = "Current GPS Device Trip is required.")]
        public string TripId { get; set; }

        [JsonProperty("coords")]
        [Required(ErrorMessage = "GPS Coordinates are required.")]
        public List<GpsCoordinateReceived_Deprecated> Coordinates { get; set; }
    }

    public class GpsCoordinateReceived_Deprecated
    {
        public GpsCoordinateReceived_Deprecated() { }

        [JsonProperty("dt")]
        [Required(ErrorMessage = "DateTime is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Wrong DateTime format. (Required format: yyyy-MM-ddTHH:mm:ss)")]
        public DateTime? Date_Time { get; set; }
                
        [JsonProperty("lt")]
        [Required(ErrorMessage = "Latitude is required.")]
        [RegularExpression("^[-+]?([1-8]?\\d(\\.\\d+)?|90(\\.0+)?)$", ErrorMessage = "Wrong Latitude format.")]
        public string Latitude { get; set; }
                
        [JsonProperty("lg")]
        [Required(ErrorMessage = "Longitude is required.")]
        [RegularExpression("^[-+]?(180(\\.0+)?|((1[0-7]\\d)|([1-9]?\\d))(\\.\\d+)?)$", ErrorMessage = "Wrong Longitude format.")]
        public string Longitude { get; set; }

        [JsonProperty("s")]
        public string Speed { get; set; }
    }
}