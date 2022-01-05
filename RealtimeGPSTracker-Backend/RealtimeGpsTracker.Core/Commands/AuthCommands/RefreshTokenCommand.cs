using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.AuthResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.AuthCommands
{
    public class RefreshTokenCommand : AnonymousCommand<RefreshTokenResponse>
    {
        public string Token { get; set; } = null;
        public string RefreshToken { get; set; } = null;
    }
}
