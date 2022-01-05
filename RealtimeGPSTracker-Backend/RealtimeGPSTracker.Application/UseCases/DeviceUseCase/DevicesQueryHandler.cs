using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Queries.DeviceQueries;
using RealtimeGPSTracker.Application.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.DeviceUseCase
{
    public class DevicesQueryHandler : IRequestHandler<DevicesQuery, DevicesResponse>
    {
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public DevicesQueryHandler(
            IGpsDeviceRepository gpsDeviceRepository,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator,
            IMapper mapper
            )
        {
            _gpsDeviceRepository = gpsDeviceRepository;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        public async Task<DevicesResponse> Handle(DevicesQuery request, CancellationToken cancellationToken)
        {
            // Holds devices response
            DevicesResponse devicesResponse = new DevicesResponse { Success = true };

            // Checking if id of owner who requested is available
            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                // Zistime pocty jednotlivych stavov zariadeni: Offline, Online, Away, ...
                GpsDevicesCounts gpsDevicesCounts = await _gpsDeviceRepository.GetGpsDevicesCounts(request.OwnerId);

                // Znova vytvorime novy queryable pre zariadenia
                IQueryable<GpsDevice> ownerGpsDevicesQueryable = _gpsDeviceRepository.GetByOwnerIdQueryable(request.OwnerId);

                PaginatedList<GpsDevice> paginatedListOfDevices = DevicePaginator.GetPaginatedDevices(
                    ownerGpsDevicesQueryable,
                    request.PaginationParameters
                );

                PaginationHeader paginationHeader = PaginationHeader.CreatePaginationHeader(paginatedListOfDevices);
                IList<NavigationLink> navigationLinks = NavigationLink.CreateNavigationLinks(
                    paginatedListOfDevices,
                    "dashboard/device/list",
                    _httpContextAccessor,
                    _linkGenerator,
                    request.PaginationParameters.Order.ToString(),
                    request.PaginationParameters.OrderBy.ToString()
                );
                IList<GpsDevice> items = await paginatedListOfDevices.ListAsync();

                devicesResponse.PaginationHeader = paginationHeader;
                devicesResponse.NavigationLinks = navigationLinks;
                devicesResponse.Items = items;
                devicesResponse.DeviceCounts = gpsDevicesCounts;
            }
            else
            {
                devicesResponse.Success = false;
                devicesResponse.Errors.Add("User was not identified while attempting to get list of devices");
            }            

            return devicesResponse;
        }
    }
}
