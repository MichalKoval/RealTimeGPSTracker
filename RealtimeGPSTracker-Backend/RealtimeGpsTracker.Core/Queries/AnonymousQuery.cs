using MediatR;

namespace RealtimeGpsTracker.Core.Queries
{
    public class AnonymousQuery<T> : IRequest<T> where T : class
    {
    }
}
