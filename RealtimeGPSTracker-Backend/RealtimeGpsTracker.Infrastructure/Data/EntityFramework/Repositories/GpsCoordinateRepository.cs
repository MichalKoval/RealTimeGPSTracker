using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Repositories
{
    public class GpsCoordinateRepository : Repository<GpsCoordinate>, IGpsCoordinateRepository
    {
        public GpsCoordinateRepository(BaseDbContext baseDbContext) : base(baseDbContext)
        {

        }
        
        public async Task<int> InsertGpsCoordinatesAsync(IList<GpsCoordinate> gpsCoordinates)
        {
            //NOT WORKING: SqlParameter sqlParameter = new SqlParameter("@Coordinates", gPSCoordinates);

            //NOT WORKING: SqlParameter sqlParameter = new SqlParameter("@Coordinates", gPSCoordinates.ToArray());

            DataTable gpsCoordinatesDataTable = ConvertToDatatable(gpsCoordinates);
            SqlParameter sqlParameter = new SqlParameter("@Coordinates", gpsCoordinatesDataTable);
            sqlParameter.TypeName = "GpsCoordinatesType";

            var result = await _baseDbContext.Database.ExecuteSqlCommandAsync("EXEC GpsCoordinates_Insert @Coordinates", sqlParameter);

            return result;
        }

        public IQueryable<GpsCoordinate> GetGpsCoordinates(string tripId, DateTime start, DateTime end)
        {
            // Obselete
            //return _baseDbContext.GpsCoordinates.FromSql<GpsCoordinate>(
            //  "EXEC [GpsCoordinates_GetData] @TripId, @From, @To",
            //  new SqlParameter("@TripId", tripId),
            //  new SqlParameter("@From", start),
            //  new SqlParameter("@To", end)
            //);

            var result =  _baseDbContext.GpsCoordinates.FromSqlInterpolated<GpsCoordinate>(
              $"SELECT * FROM [GpsCoordinates_GetData] {tripId}, {start}, {end}"
            );

            return result;
        }

        public async Task<int> DeleteGpsCoordinatesAsync(string tripId, DateTime start, DateTime end)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@TripId", tripId)
                {
                    TypeName = "NVARCHAR(450)"
                },
                new SqlParameter("@From", start)
                {
                    TypeName = "DATETIME2"
                },
                new SqlParameter("@To", end)
                {
                    TypeName = "DATETIME2"
                }
            };

            var result = await _baseDbContext.Database.ExecuteSqlCommandAsync("EXEC [GpsCoordinates_Delete] @TripId, @From, @To", sqlParameters);

            return result;
        }

        public async Task<int> DeleteGpsCoordinatesByTripAsync(string tripId)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@TripId", tripId)
                {
                    TypeName = "NVARCHAR(450)"
                }
            };

            var result = await _baseDbContext.Database.ExecuteSqlCommandAsync("EXEC [GpsCoordinates_DeleteByTrip] @TripId", sqlParameters);

            return result;
        }
    }
}
