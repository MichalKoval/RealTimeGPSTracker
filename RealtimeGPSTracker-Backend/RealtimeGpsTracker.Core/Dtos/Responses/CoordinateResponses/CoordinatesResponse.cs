using RealtimeGpsTracker.Core.Entities;
using System.Collections.Generic;

namespace RealtimeGpsTracker.Core.Dtos.Responses.CoordinateResponses
{
    public class CoordinatesResponse : Response
    {
        public string DeviceId { get; set; } = null;
        public string TripId { get; set; } = null;
        public IList<GpsCoordinate> Coordinates { get; set; } = null;
    }
}
