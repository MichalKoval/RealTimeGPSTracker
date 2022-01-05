using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands.DeviceCommands;
using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.DeviceUseCase
{
    public class DeleteMultipleDevicesCommandHandler : IRequestHandler<DeleteMultipleDevicesCommand, DeleteMultipleDevicesResponse>
    {
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly IDeviceHubService _deviceHubService;
        private readonly IMapper _mapper;

        public DeleteMultipleDevicesCommandHandler(
            IGpsDeviceRepository gpsDeviceRepository,
            IDeviceHubService deviceHubService,
            IMapper mapper

            )
        {
            _gpsDeviceRepository = gpsDeviceRepository;
            _deviceHubService = deviceHubService;
            _mapper = mapper;
        }

        public async Task<DeleteMultipleDevicesResponse> Handle(DeleteMultipleDevicesCommand request, CancellationToken cancellationToken)
        {
            // Will hold result
            DeleteMultipleDevicesResponse deleteMultipleDevicesResponse = new DeleteMultipleDevicesResponse { Success = true };

            // Checking if id of owner who requested is available
            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                foreach (string deviceId in request.DeviceIds)
                {
                    // Checks of device belongs to owner who asked for deletion of a device.
                    var device = await _gpsDeviceRepository.CheckIfDeviceBelongsToOwner(request.OwnerId, deviceId);

                    if (device != null)
                    {
                        // Deleting the device and its trips and gps coordinates.
                        await _gpsDeviceRepository.DeleteDeviceAsync(deviceId);
                    }
                    else
                    {
                        deleteMultipleDevicesResponse.Success = false;
                        deleteMultipleDevicesResponse.Errors.Add("You don't have a permission to delete devices");
                    }
                }

                if (deleteMultipleDevicesResponse.Success)
                {
                    // After deletion signalr notifies all opened websites of the current user to reload data (on a smartphone, tablet, or desktop )
                    // not including the website which fired the deletion (it reloads itself).
                    await _deviceHubService.SendUpdateMessageToUserGroup(request.OwnerId, "");
                }
            }
            else
            {
                deleteMultipleDevicesResponse.Success = false;
                deleteMultipleDevicesResponse.Errors.Add("User was not identified while attempting to delete devices");
            }

            return deleteMultipleDevicesResponse;
        }
    }
}


//foreach (string deviceId in request.DeviceIds)
//{
//    // Skontrolujeme ci zariadenie patri danemu uzivatelovi z http kontextu
//    GpsDevice deviceWhichBelongsToOwner = await _gpsDeviceRepository.CheckIfDeviceBelongsToOwner(deviceId, request.OwnerId);

//    if (deviceWhichBelongsToOwner != null)
//    {
//        // Oznacime zariadenie za InActive, aby pre dane zariadenie uz neprichadzali suradnice
//        deviceWhichBelongsToOwner.Status = GpsDeviceStatus.ForDelete;
//        _gpsDeviceRepository.SetDeviceStatus(deviceWhichBelongsToOwner);

//        // Pre zmazanie zariadenia je najskor potrebne zmazat vsetky trasy,
//        // ktore zariadenie zaznamenalo, a tym padom aj suradnice danych tras.
//        // (Ziaden soft delete GDPR)
//        // Prv vratime zoznam tras pre dane zariadenie
//        IList<Trip> trips = await _tripRepository.GetTripsForGpsDeviceIdAsync(deviceWhichBelongsToOwner.GpsDeviceId);

//        // Pre kazdu trasu zmazeme jej suradnice
//        foreach (Trip trip in trips)
//        {
//            // await _gpsCoordinateRepository.DeleteGpsCoordinatesAsync(trip.TripID, trip.Start, trip.End);

//            // Potom zmazeme aj samotny trip
//            // TODO: !!!
//            // await _tripRepository.DeleteAsync(trip);
//        }

//        // Nakoniec zmazeme dane zariadenie
//        await _gpsDeviceRepository.DeleteAsync(deviceWhichBelongsToOwner);
//    }
//}

//// Kedze doslo k zmazaniu zariadeni, upozornime na to aj vsetkych ostatne otvorene stranky (mobil, tablet, alebo v tom istom prehliadaci viac krat)
//// daneho uzivatela, okrem stranky odkial bol odoslany poziadavok pre zmazanie, kedze si dana stranka obnovi zoznam sama.

//await _deviceHubService.SendUpdateMessageToUserGroup(request.OwnerId, "");
