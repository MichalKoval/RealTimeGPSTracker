using RealtimeGpsTracker.Core.Entities;

namespace RealtimeGpsTracker.Core.Dtos.Responses.UserResponses
{
    public class DetailUserResponse : Response
    {
        public string Id { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string UserName { get; set; } = null;
        public string Email { get; set; } = null;
    }
}
