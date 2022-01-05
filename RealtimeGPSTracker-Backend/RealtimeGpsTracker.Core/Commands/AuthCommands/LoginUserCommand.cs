using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.AuthResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.AuthCommands
{
    public class LoginUserCommand : AnonymousCommand<LoginUserResponse>
    {
        [Required]
        public string UserName { get; set; } = null;

        public string Email { get; set; } = null;

        /// <summary>
        /// Password is stored as a hash in database!!! There is different User model for it. This is just a DTO.
        /// Password is deserialized from DTO but not serialized to any DTO.
        /// </summary>
        [Required]
        public string Password { get; set; } = null;
    }
}
