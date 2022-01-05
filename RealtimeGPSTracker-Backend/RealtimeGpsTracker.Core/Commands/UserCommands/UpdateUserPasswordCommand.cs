using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.UserCommands
{
    public class UpdateUserPasswordCommand : AuthorizedCommand<UpdateUserPasswordResponse>
    {
        // User password will be changed (password reset token and new password will be generated)
        [Required]
        public string Password { get; set; } = null;
    }
}
