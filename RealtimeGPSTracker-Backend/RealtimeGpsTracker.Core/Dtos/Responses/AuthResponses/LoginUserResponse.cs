using Newtonsoft.Json;

namespace RealtimeGpsTracker.Core.Dtos.Responses.AuthResponses
{
    public class LoginUserResponse : Response
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = null;
    }
}
