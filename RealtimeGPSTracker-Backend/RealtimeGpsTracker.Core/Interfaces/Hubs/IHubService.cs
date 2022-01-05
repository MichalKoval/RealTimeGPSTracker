using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Hubs
{
    public interface IHubService
    {
        Task SendUpdateMessageToUserGroup(string userId);

        Task SendUpdateMessageToUserGroup(string userId, string excludedConnectionId);

        Task SendUpdateMessageToUserGroup(string userId, IReadOnlyList<string> excludedConnectionIds);
    }
}
