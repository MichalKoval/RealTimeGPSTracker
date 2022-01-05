using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.UserCommands
{
    public class UpdateUserDetailsCommand : AuthorizedCommand<UpdateUserDetailsResponse>
    {
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        [Required]
        public string Email { get; set; } = null;
    }
}
