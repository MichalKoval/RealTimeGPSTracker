using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Hubs
{
    public interface IDeviceHubService : IHubService
    {
        Task SendNewOnlineDeviceMessageToUserGroup(string userId, string deviceName);
    }
}
