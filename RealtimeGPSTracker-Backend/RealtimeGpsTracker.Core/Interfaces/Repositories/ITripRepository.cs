using RealtimeGpsTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Repositories
{
    public interface ITripRepository : IRepository<Trip>
    {
        IQueryable<Trip> GetByOwnerIdQueryable(string Id);

        Task<Trip> CheckIfTripBelongsToOwnerAsync(string userId, string tripId);

        Task<Trip> CheckIfTripBelongsToDeviceAsync(string deviceId, string tripId);

        Task<Trip> CheckIfTripExistsAsync(string tripId);

        Task<IList<Trip>> GetTripsForGpsDeviceIdAsync(string gpsDeviceId);

        Task<int> DeleteTripAsync(string tripId);
    }
}
