using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using System.Data.SqlClient;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Repositories
{
    public class GpsDeviceRepository : Repository<GpsDevice>, IGpsDeviceRepository
    {
        public GpsDeviceRepository(BaseDbContext baseDbContext) : base(baseDbContext)
        {

        }

        public async Task<IList<GpsDevice>> GetByOwnerIdAsync(string Id)
        {
            return await _baseDbContext.GpsDevices
                .Where(d => d.UserId.Equals(Id))
                .ToListAsync();
        }

        public IQueryable<GpsDevice> GetByOwnerIdQueryable(string Id)
        {
            return _baseDbContext.GpsDevices
                .Where(d => d.UserId.Equals(Id));
        }

        public async Task<GpsDevicesCounts> GetGpsDevicesCounts(string Id)
        {
           
            //IEnumerable<GpsDeviceStatusCounts> gpsDeviceStatusCountsEnumerable =
            //    from d in _applicationDbContext.GpsDevices
            //    where d.UserId.Equals(Id)
            //    group d by d.Status into c
            //    select new GpsDeviceStatusCounts()
            //    {
            //        Status = c.Key,
            //        Count = c.Count()
            //    };


            Dictionary<GpsDeviceStatus, int> gpsDeviceStatusCounts = await _baseDbContext.GpsDevices
                .Where(d => d.UserId.Equals(Id))
                .GroupBy(ds => ds.Status)
                .Select(g => new { status = g.Key, count = g.Count() })
                .ToDictionaryAsync(k => k.status, i => i.count);

            GpsDevicesCounts gpsDevicesCounts = new GpsDevicesCounts()
            {
                OfflineCount = gpsDeviceStatusCounts.GetValueOrDefault(GpsDeviceStatus.Offline, 0),
                OnlineCount = gpsDeviceStatusCounts.GetValueOrDefault(GpsDeviceStatus.Online, 0),
                AwayCount = gpsDeviceStatusCounts.GetValueOrDefault(GpsDeviceStatus.Away, 0)
            };

            gpsDevicesCounts.AllCount = gpsDevicesCounts.OfflineCount + gpsDevicesCounts.OnlineCount + gpsDevicesCounts.AwayCount;

            return gpsDevicesCounts;
        }

        public async Task<GpsDevice> CheckIfDeviceBelongsToOwner(string gpsDeviceId, string userId) {
            IList<GpsDevice> gpsDevicesList = await _baseDbContext.GpsDevices
                .Where(d => d.UserId.Equals(userId) && d.GpsDeviceId.Equals(gpsDeviceId))
                .ToListAsync();


            return (gpsDevicesList.Count == 1) ? gpsDevicesList.First() : null;
        }

        public async Task UpdateDeviceAsync(GpsDevice gpsDevice)
        {

            var deviceToUpdate = _baseDbContext.GpsDevices.Where(d => d.GpsDeviceId.Equals(gpsDevice.GpsDeviceId)).FirstOrDefault();

            // Chceme aby sa aktualizovali iba urcite stlpce
            if (deviceToUpdate != null)
            {
                deviceToUpdate.Name = gpsDevice.Name;
                deviceToUpdate.Color = gpsDevice.Color;
                deviceToUpdate.Interval = gpsDevice.Interval;
                deviceToUpdate.IntervalChanged = gpsDevice.IntervalChanged;
            }

            await _baseDbContext.SaveChangesAsync();
            
        }

        public async Task<int> RefreshStatusesOfDevices()
        {
            var result = await _baseDbContext.Database.ExecuteSqlCommandAsync("EXEC [GPSDevices_RefreshStatus]");

            return result;
        }

        public void SetDeviceStatus(GpsDevice gpsDevice)

        {
            var deviceToUpdate = _baseDbContext.GpsDevices.Where(d => d.GpsDeviceId.Equals(gpsDevice.GpsDeviceId)).FirstOrDefault();

            // Chceme aby sa aktualizovali iba urcite stlpce
            if (deviceToUpdate != null)
            {
                deviceToUpdate.Status = gpsDevice.Status;
            }
            _baseDbContext.SaveChanges();
        }

        public async Task<int> DeleteDeviceAsync(string deviceId)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@DeviceId", deviceId)
                {
                    TypeName = "NVARCHAR(450)"
                }
            };

            var result = await _baseDbContext.Database.ExecuteSqlCommandAsync("EXEC [GpsDevices_Delete] @DeviceId", sqlParameters);

            return result;
        }
    }
}



//public void UpdateDevice(GpsDevice gpsDevice)
//{
//    var gpsDeviceToUpdate = new GpsDevice()
//    {
//        GpsDeviceId = gpsDevice.GpsDeviceId,
//        Name = gpsDevice.Name,
//        Color = gpsDevice.Color,
//        Interval = gpsDevice.Interval,
//        IntervalChanged = gpsDevice.IntervalChanged
//    };

//    try
//    {
//        _applicationDbContext.GpsDevices.Attach(gpsDeviceToUpdate);
//        _applicationDbContext.Entry(gpsDeviceToUpdate).Property("Name").IsModified = true;
//        _applicationDbContext.Entry(gpsDeviceToUpdate).Property("Color").IsModified = true;
//        _applicationDbContext.Entry(gpsDeviceToUpdate).Property("Interval").IsModified = true;
//        _applicationDbContext.Entry(gpsDeviceToUpdate).Property("IntervalChanged").IsModified = true;
//        _applicationDbContext.SaveChanges();
//    }
//    catch (Exception e)
//    {

//        throw;
//    }

//}

// Vratime zariadenia s danym uzivatela, ktore nieco splnuju Where(...)
//public IEnumerable<GpsDevice> FindWithApplicationUser(Func<GpsDevice, bool> predicate)
//{
//    return _applicationDbContext.GpsDevices
//        .Include(d => d.ApplicationUser)
//        .Where(predicate);
//}

// Vratime vsetky zariadenia s danym uzivatela
//    public IEnumerable<GpsDevice> GetAllWithApplicationUser()
//    {
//        return _applicationDbContext.GpsDevices
//            .Include(d => d.ApplicationUser);
//    }