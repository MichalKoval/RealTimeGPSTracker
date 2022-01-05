using MediatR;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Queries
{
    public class AuthorizedQuery<T> : IRequest<T> where T : class
    {
        [Required]
        public string OwnerId { get; set; } = null;
    }
}
