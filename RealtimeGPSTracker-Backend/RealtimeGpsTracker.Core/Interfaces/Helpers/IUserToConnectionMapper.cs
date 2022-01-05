using System.Collections.Generic;

namespace RealtimeGpsTracker.Core.Interfaces.Helpers
{
    public interface IUserToConnectionMapper<T>
    {
        int Count { get; }
        void Add(T key, string connectionId);
        IEnumerable<string> GetConnections(T key);
        void Remove(T key, string connectionId);
    }
}