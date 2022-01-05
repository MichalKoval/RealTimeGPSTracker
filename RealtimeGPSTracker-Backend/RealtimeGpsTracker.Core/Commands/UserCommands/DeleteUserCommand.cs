using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.UserCommands
{
    public class DeleteUserCommand : AuthorizedCommand<DeleteUserResponse>
    {
    }
}
