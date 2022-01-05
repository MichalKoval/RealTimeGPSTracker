using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.TripResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Queries.TripQueries
{
    public class DetailTripQuery : AuthorizedQuery<DetailTripResponse>
    {
        [Required]
        public string TripId { get; set; }
    }
}
