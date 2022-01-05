using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;

namespace RealtimeGpsTracker.Core.Queries.DeviceQueries
{
    public class DevicesQuery : AuthorizedQuery<DevicesResponse>
    {
        public DevicePaginationParameters PaginationParameters { get; set; } = null;
    }
}