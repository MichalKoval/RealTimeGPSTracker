using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;
using System.Collections.Generic;

namespace RealtimeGpsTracker.Core.Dtos.Responses.TripResponses
{
    public class TripsResponse : Response
    {
        public PaginationHeader PaginationHeader { get; set; }
        public IList<NavigationLink> NavigationLinks { get; set; }
        public IList<Trip> Items { get; set; }
    }
}
