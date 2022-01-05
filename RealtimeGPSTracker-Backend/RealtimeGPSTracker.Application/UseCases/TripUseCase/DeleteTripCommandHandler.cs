using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands.TripCommands;
using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Dtos.Responses.TripResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.TripUseCase
{
    public class DeleteTripCommandHandler : IRequestHandler<DeleteTripCommand, DeleteTripResponse>
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripHubService _tripHubService;

        public DeleteTripCommandHandler(
            ITripRepository tripRepository,
            ITripHubService tripHubService
            )
        {
            _tripRepository = tripRepository;
            _tripHubService = tripHubService;
        }

        public async Task<DeleteTripResponse> Handle(DeleteTripCommand request, CancellationToken cancellationToken)
        {
            // Will hold result
            DeleteTripResponse deleteTripResponse = new DeleteTripResponse { Success = true };

            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                // Checks if trip belongs to owner who asked for deletion of a trip.
                var trip = await _tripRepository.CheckIfTripBelongsToOwnerAsync(request.OwnerId, request.TripId);

                if (trip != null)
                {
                    // Deleting the trip and its gps coordinates
                    await _tripRepository.DeleteTripAsync(request.TripId);

                    // After deletion signalr notifies all opened websites of the current user to reload data (on a smartphone, tablet, or desktop )
                    // not including the website which fired the deletion (it reloads itself).
                    await _tripHubService.SendUpdateMessageToUserGroup(request.OwnerId, "");
                }
                else
                {
                    deleteTripResponse.Success = false;
                    deleteTripResponse.Errors.Add("You don't have a permission to delete the trip");
                }
            }
            else
            {
                deleteTripResponse.Success = false;
                deleteTripResponse.Errors.Add("User was not identified while attempting to delete the trip");
            }                

            return deleteTripResponse;
        }
    }
}
