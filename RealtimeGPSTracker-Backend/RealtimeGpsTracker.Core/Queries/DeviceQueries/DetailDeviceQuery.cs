using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Queries.DeviceQueries
{
    public class DetailDeviceQuery : AuthorizedQuery<DetailDeviceResponse>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}