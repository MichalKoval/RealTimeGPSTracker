using MediatR;
using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Queries.UserQueries
{
    public class DetailUserQuery : AuthorizedQuery<DetailUserResponse>
    {
    }
}
