using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Repositories
{
    public class TripRepository : Repository<Trip>, ITripRepository
    {
        public TripRepository(BaseDbContext baseDbContext) : base(baseDbContext)
        {

        }

        public IQueryable<Trip> GetByOwnerIdQueryable(string Id)
        {
            return _baseDbContext.Trips
                .Include(t => t.GpsDevice)
                .Where(t => t.GpsDevice.UserId.Equals(Id));
        }

        public async Task<Trip> CheckIfTripBelongsToOwnerAsync(string userId, string tripId)
        {
            return await _baseDbContext.Trips
                .Include(t => t.GpsDevice)
                .Where(t =>
                    t.GpsDevice.UserId.Equals(userId) &&
                    t.TripId.Equals(tripId)
                ).FirstOrDefaultAsync();
        }

        public async Task<Trip> CheckIfTripBelongsToDeviceAsync(string deviceId, string tripId)
        {
            return await _baseDbContext.Trips
                .Where(t =>
                    t.TripId.Equals(tripId) &&
                    t.GpsDeviceId.Equals(deviceId))
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Trip>> GetTripsForGpsDeviceIdAsync(string gpsDeviceId)
        {
            return await _baseDbContext.Trips
                .Where(t => t.GpsDeviceId.Equals(gpsDeviceId))
                .ToListAsync();
        }

        public async Task<int> DeleteTripAsync(string tripId)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@TripId", tripId)
                {
                    TypeName = "NVARCHAR(450)"
                }
            };

            var result = await _baseDbContext.Database.ExecuteSqlCommandAsync("EXEC [Trips_Delete] @TripId", sqlParameters);

            return result;
        }

        public async Task<Trip> CheckIfTripExistsAsync(string tripId)
        {
            return await _baseDbContext.Trips
                .Where(t => t.TripId.Equals(tripId))
                .FirstOrDefaultAsync();
        }
    }
}
