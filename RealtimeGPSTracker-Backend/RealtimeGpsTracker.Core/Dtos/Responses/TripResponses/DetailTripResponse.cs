using RealtimeGpsTracker.Core.Entities;
using System;

namespace RealtimeGpsTracker.Core.Dtos.Responses.TripResponses
{
    public class DetailTripResponse : Response
    {
        public string Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Distance { get; set; } = 0;

        public bool InProgress { get; set; } = false;
    }
}
