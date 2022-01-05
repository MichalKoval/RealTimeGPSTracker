using System.Collections.Generic;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;

namespace RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses
{
    public class DevicesResponse : Response
    {
        public PaginationHeader PaginationHeader { get; set; }
        public IList<NavigationLink> NavigationLinks { get; set; }
        public IList<GpsDevice> Items { get; set; }
        public GpsDevicesCounts DeviceCounts { get; set; }
    }
}
