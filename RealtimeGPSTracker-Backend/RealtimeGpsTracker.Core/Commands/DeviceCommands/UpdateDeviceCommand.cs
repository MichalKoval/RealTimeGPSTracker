using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RealtimeGpsTracker.Core.Commands.DeviceCommands
{
    public class UpdateDeviceCommand : AuthorizedCommand<UpdateDeviceResponse>
    {
        [Required]
        public string Id { get; set; } = null;
        [Required]
        public string Name { get; set; }
        public string Color { get; set; }
        public int Interval { get; set; }
    }
}