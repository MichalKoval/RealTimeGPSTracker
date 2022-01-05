using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.TripResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.TripCommands
{
    public class DeleteTripCommand : AuthorizedCommand<DeleteTripResponse>
    {
        [Required]
        public string TripId { get; set; } = null;
    }
}