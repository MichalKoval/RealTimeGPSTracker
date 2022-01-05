using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Repositories
{
    public interface IGpsDeviceRepository : IRepository<GpsDevice>
    {
        Task<IList<GpsDevice>> GetByOwnerIdAsync(string Id);

        IQueryable<GpsDevice> GetByOwnerIdQueryable(string Id);

        Task<GpsDevice> CheckIfDeviceBelongsToOwner(string gpsDeviceId, string userId);

        Task<GpsDevicesCounts> GetGpsDevicesCounts(string Id);

        Task UpdateDeviceAsync(GpsDevice gpsDevice);

        Task<int> RefreshStatusesOfDevices();

        Task<int> DeleteDeviceAsync(string deviceId);

        void SetDeviceStatus(GpsDevice gpsDevice);
    }
}
