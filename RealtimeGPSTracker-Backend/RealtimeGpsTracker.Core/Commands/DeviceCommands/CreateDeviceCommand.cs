using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.DeviceCommands
{
    public class CreateDeviceCommand : AuthorizedCommand<CreateDeviceResponse>
    {
        [Required]
        public string Name { get; set; }
        public string Color { get; set; }
        public int Interval { get; set; }
    }
}