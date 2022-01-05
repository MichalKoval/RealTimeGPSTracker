using RealtimeGpsTracker.Core.Entities;
using System;

namespace RealtimeGpsTracker.Core.Dtos.Responses.HubMessages
{ 
    public class CoordinateHubMessage : HubMessage
    {
        public CoordinateHubMessage() { }

        public CoordinateHubMessage(string message) : base(message) { }

        public CoordinateHubMessage(string message, string deviceId, string tripId, DateTime startDt, DateTime endDt)
        {
            this.Message = message;
            this.DeviceId = deviceId;
            this.TripId = tripId;
            this.StartDt = startDt;
            this.EndDt = endDt;
        }

        public string DeviceId { get; set; } = null;
        public string TripId { get; set; } = null;
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
    }
}
