using RealtimeGpsTracker.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Hubs
{
    public interface ICoordinateHubService : IHubService
    {
        Task SendUpdateMessageToUserGroup(string userId, string deviceId, Trip trip);

        Task SendUpdateMessageToUserGroup(string userId, string deviceId, Trip trip, string excludedConnectionId);

        Task SendUpdateMessageToUserGroup(string userId, string deviceId, Trip trip, IReadOnlyList<string> excludedConnectionIds);
    }
}
