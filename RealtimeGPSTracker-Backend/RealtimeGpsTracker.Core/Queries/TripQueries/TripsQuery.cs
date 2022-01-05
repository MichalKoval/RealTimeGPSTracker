using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Dtos.Responses.TripResponses;

namespace RealtimeGpsTracker.Core.Queries.TripQueries
{
    public class TripsQuery : AuthorizedQuery<TripsResponse>
    {
        public TripPaginationParameters PaginationParameters { get; set; } = null;
    }
}