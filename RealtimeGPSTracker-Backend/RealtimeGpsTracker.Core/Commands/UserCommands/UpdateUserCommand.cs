using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;

namespace RealtimeGpsTracker.Core.Commands.UserCommands
{
    public class UpdateUserCommand : AuthorizedCommand<UpdateUserResponse>
    {
        public UpdateUserDetailsCommand UserDetails { get; set; } = null;
        public UpdateUserPasswordCommand UserPassword { get; set; } = null;
    }
}
