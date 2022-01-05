using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands.DeviceCommands;
using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.DeviceUseCase
{
    public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, DeleteDeviceResponse>
    {
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly IDeviceHubService _deviceHubService;

        public DeleteDeviceCommandHandler(
            IGpsDeviceRepository gpsDeviceRepository,
            IDeviceHubService deviceHubService
            )
        {
            _gpsDeviceRepository = gpsDeviceRepository;
            _deviceHubService = deviceHubService;
        }

        public async Task<DeleteDeviceResponse> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            // Will hold result
            DeleteDeviceResponse deviceDeleteResponse = new DeleteDeviceResponse { Success = true };

            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                // Checks if device belongs to owner who asked for deletion of a device.
                var device = await _gpsDeviceRepository.CheckIfDeviceBelongsToOwner(request.OwnerId, request.DeviceId);

                if (device != null)
                {
                    // Deleting the device and its trips and gps coordinates.
                    await _gpsDeviceRepository.DeleteDeviceAsync(request.DeviceId);

                    // After deletion signalr notifies all opened websites of the current user to reload data (on a smartphone, tablet, or desktop )
                    // not including the website which fired the deletion (it reloads itself).
                    await _deviceHubService.SendUpdateMessageToUserGroup(request.OwnerId, "");
                }
                else
                {
                    deviceDeleteResponse.Success = false;
                    deviceDeleteResponse.Errors.Add("You don't have a permission to delete the device");
                }
            }
            else
            {
                deviceDeleteResponse.Success = false;
                deviceDeleteResponse.Errors.Add("User was not identified while attempting to delete the device");
            }

            return deviceDeleteResponse;
        }
    }
}
