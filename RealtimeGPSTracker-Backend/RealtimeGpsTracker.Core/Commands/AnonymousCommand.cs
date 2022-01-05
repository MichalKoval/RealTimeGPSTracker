using MediatR;

namespace RealtimeGpsTracker.Core.Commands
{
    public class AnonymousCommand<T> : IRequest<T> where T : class
    {
    }
}
