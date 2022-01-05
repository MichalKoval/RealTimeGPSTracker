using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Dtos.Responses.TripResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Queries.TripQueries;
using RealtimeGPSTracker.Application.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.TripUseCase
{
    public class TripsQueryHandler : IRequestHandler<TripsQuery, TripsResponse>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public TripsQueryHandler(
            ITripRepository tripRepository,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator,
            IMapper mapper
            )
        {
            _tripRepository = tripRepository;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        public async Task<TripsResponse> Handle(TripsQuery request, CancellationToken cancellationToken)
        {
            // Holds trips response
            TripsResponse tripsResponse = new TripsResponse { Success = true };

            // Checking if id of owner who requested is available
            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                IQueryable<Trip> ownerTripsQueryable = _tripRepository.GetByOwnerIdQueryable(request.OwnerId);

                PaginatedList<Trip> paginatedListOfTrips = TripPaginator.GetPaginatedTrips(
                    ownerTripsQueryable,
                    request.PaginationParameters
                );

                PaginationHeader paginationHeader = PaginationHeader.CreatePaginationHeader(paginatedListOfTrips);
                IList<NavigationLink> navigationLinks = NavigationLink.CreateNavigationLinks(
                    paginatedListOfTrips,
                    "dashboard/history/list",
                    _httpContextAccessor,
                    _linkGenerator,                    
                    request.PaginationParameters.Order.ToString(),
                    request.PaginationParameters.OrderBy.ToString()
                );
                IList<Trip> items = await paginatedListOfTrips.ListAsync();

                tripsResponse.PaginationHeader = paginationHeader;
                tripsResponse.NavigationLinks = navigationLinks;
                tripsResponse.Items = items;
            }
            else
            {
                tripsResponse.Success = false;
                tripsResponse.Errors.Add("User was not identified while attempting to get list of trips");
            }            

            return tripsResponse;
        }
    }
}
