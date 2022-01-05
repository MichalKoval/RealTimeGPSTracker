using RealtimeGpsTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Repositories
{
    public interface IGpsCoordinateRepository : IRepository<GpsCoordinate>
    {
        Task<int> InsertGpsCoordinatesAsync(IList<GpsCoordinate> gpsCoordinates);

        Task<int> DeleteGpsCoordinatesAsync(string tripId, DateTime start, DateTime end);

        Task<int> DeleteGpsCoordinatesByTripAsync(string tripId);

        IQueryable<GpsCoordinate> GetGpsCoordinates(string tripId, DateTime start, DateTime end);
    }
}
