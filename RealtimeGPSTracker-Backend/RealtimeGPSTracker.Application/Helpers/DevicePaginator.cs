using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;
using System.Linq;
using System.Reflection;
using static RealtimeGpsTracker.Core.Dtos.Responses.Pagination.PaginationParameters;

namespace RealtimeGPSTracker.Application.Helpers
{
    public class DevicePaginator
    {
        public static PaginatedList<GpsDevice> GetPaginatedDevices(IQueryable<GpsDevice> gpsDevicesQueryable, DevicePaginationParameters paginationParams)
        {
            GpsDeviceStatus[] deviceStatuses;

            if (paginationParams.Statuses != null)
            {
                deviceStatuses = paginationParams.Statuses;

                // device status:
                //  0 --> Offline
                //  1 --> Online
                //  2 --> Away

                

                gpsDevicesQueryable = gpsDevicesQueryable.Where(d => deviceStatuses.Contains(d.Status));

                //foreach (GpsDeviceStatus deviceStatus in deviceStatuses)
                //{
                //    // device status:
                //    //  0 --> Offline
                //    //  1 --> Online
                //    //  2 --> Away

                //    if (deviceStatus.Equals(GpsDeviceStatus.Online)
                //        || deviceStatus.Equals(GpsDeviceStatus.Away)
                //        || deviceStatus.Equals(GpsDeviceStatus.Offline)
                //    )
                //    {
                //        gpsDevicesQueryable = gpsDevicesQueryable.Where(d => d.Status.Equals(deviceStatus));
                //    }
                //}

                //gpsDevicesQueryable = gpsDevicesQueryable.FromSql('')
            }
            else
            {
                //Ak chceme zoznam vsetkych dostupnych zariadeni, okrem tych, ktore boli oznacene ako neaktivne / na zmazanie
                gpsDevicesQueryable = gpsDevicesQueryable.Where(d => !d.Status.Equals(GpsDeviceStatus.Disabled));
            }


            // Specialne pre 'devices' smer zoradenia, zoradit podla
            if (paginationParams.Order == OrderDirection.Asc)
            {
                switch (paginationParams.OrderBy)
                {
                    case DevicePaginationParameters.DeviceOrderBy.Name:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderBy(d => d.Name);
                        break;
                    case DevicePaginationParameters.DeviceOrderBy.CreateTime:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderBy(d => d.CreateTime);
                        break;
                    case DevicePaginationParameters.DeviceOrderBy.Color:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderBy(d => d.Color);
                        break;
                    case DevicePaginationParameters.DeviceOrderBy.Interval:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderBy(d => d.Interval);
                        break;
                    default:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderBy(d => d.CreateTime);
                        break;
                }
            }
            else
            {
                switch (paginationParams.OrderBy)
                {
                    case DevicePaginationParameters.DeviceOrderBy.Name:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderByDescending(d => d.Name);
                        break;
                    case DevicePaginationParameters.DeviceOrderBy.CreateTime:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderByDescending(d => d.CreateTime);
                        break;
                    case DevicePaginationParameters.DeviceOrderBy.Color:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderByDescending(d => d.Color);
                        break;
                    case DevicePaginationParameters.DeviceOrderBy.Interval:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderByDescending(d => d.Interval);
                        break;
                    default:
                        gpsDevicesQueryable = gpsDevicesQueryable.OrderByDescending(d => d.CreateTime);
                        break;
                }
            }

            return new PaginatedList<GpsDevice>(
                gpsDevicesQueryable,
                paginationParams.PageIndex,
                paginationParams.PageSize
            );
        }
    }
}
