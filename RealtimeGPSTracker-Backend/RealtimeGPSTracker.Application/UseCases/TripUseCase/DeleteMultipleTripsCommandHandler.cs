using AutoMapper;
using MediatR;
using RealtimeGpsTracker.Core.Commands.TripCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.TripUseCase
{
    public class DeleteMultipleTripsCommandHandler : IRequestHandler<DeleteMultipleTripsCommand, DeleteMultipleTripsResponse>
    {
        private readonly IGpsCoordinateRepository _gpsCoordinateRepository;
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly ITripRepository _tripRepository;
        private readonly ITripHubService _tripHubService;
        private readonly IMapper _mapper;

        public DeleteMultipleTripsCommandHandler(
            IGpsCoordinateRepository gpsCoordinateRepository,
            IGpsDeviceRepository gpsDeviceRepository,
            ITripRepository tripRepository,
            ITripHubService tripHubService,
            IMapper mapper

            )
        {
            _gpsCoordinateRepository = gpsCoordinateRepository;
            _gpsDeviceRepository = gpsDeviceRepository;
            _tripRepository = tripRepository;
            _tripHubService = tripHubService;
            _mapper = mapper;
        }

        public async Task<DeleteMultipleTripsResponse> Handle(DeleteMultipleTripsCommand request, CancellationToken cancellationToken)
        {
            // Will hold result
            DeleteMultipleTripsResponse deleteMultipleTripsResponse = new DeleteMultipleTripsResponse { Success = true };

            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                foreach (string tripId in request.TripIds)
                {
                    // Checks if trip belongs to owner who asked for deletion of a trip.
                    var trip = await _tripRepository.CheckIfTripBelongsToOwnerAsync(request.OwnerId, tripId);

                    if (trip != null)
                    {
                        // Deleting the trip and its gps coordinates
                        await _tripRepository.DeleteTripAsync(tripId);
                    }
                    else
                    {
                        deleteMultipleTripsResponse.Success = false;
                        deleteMultipleTripsResponse.Errors.Add("You don't have a permission to delete trips");
                    }
                }

                if (deleteMultipleTripsResponse.Success)
                {
                    // After deletion signalr notifies all opened websites of the current user to reload data (on a smartphone, tablet, or desktop )
                    // not including the website which fired the deletion (it reloads itself).
                    await _tripHubService.SendUpdateMessageToUserGroup(request.OwnerId, "");
                }                
            }
            else
            {
                deleteMultipleTripsResponse.Success = false;
                deleteMultipleTripsResponse.Errors.Add("User was not identified while attempting to delete trips");
            }

            return deleteMultipleTripsResponse;
        }
    }
}
