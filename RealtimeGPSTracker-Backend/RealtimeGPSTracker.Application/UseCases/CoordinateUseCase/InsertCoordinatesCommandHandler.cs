using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Commands.CoordinateCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.CoordinateResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static RealtimeGpsTracker.Core.Commands.CoordinateCommands.InsertCoordinatesCommand;

namespace RealtimeGPSTracker.Application.UseCases.CoordinateUseCase
{
    public class InsertCoordinatesCommandHandler : IRequestHandler<InsertCoordinatesCommand, InsertCoordinatesResponse>
    {
        private readonly IGpsCoordinateRepository _coordinateRepository;
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;
        private readonly IDeviceHubService _deviceHubService;
        private readonly ICoordinateHubService _coordinateHubService;

        private List<GpsCoordinate> ProcessGpsCoordinates(List<GpsCoordinateReceived> gpsCoordinateReceived, string tripId)
        {
            // Before putting any data to database the List<GpsCoordinateReceived> of received coordinates need to be processed
            // Each new coordinate gets new unique Id and Id of the Trip to which it belongs.
            List<GpsCoordinate> coordinates = new List<GpsCoordinate>();
            int gpsCoordsCount = gpsCoordinateReceived.Count;
            for (int i = 0; i < gpsCoordsCount; i++)
            {
                GpsCoordinate gpsCoord = new GpsCoordinate()
                {
                    GpsCoordinateId = Guid.NewGuid().ToString(),
                    Time = (DateTime)gpsCoordinateReceived[i].Date_Time,
                    Lt = gpsCoordinateReceived[i].Latitude.ToString(),
                    Lg = gpsCoordinateReceived[i].Longitude.ToString(),
                    Speed = gpsCoordinateReceived[i].Speed.ToString(),
                    TripId = tripId
                };

                coordinates.Add(gpsCoord);
            }

            return coordinates;
        }

        public InsertCoordinatesCommandHandler(
            IGpsCoordinateRepository coordinateRepository,
            IGpsDeviceRepository gpsDeviceRepository,
            ITripRepository tripRepository,
            IMapper mapper,
            IDeviceHubService deviceHubService,
            ICoordinateHubService coordinateHubService
            )
        {
            _coordinateRepository = coordinateRepository;
            _gpsDeviceRepository = gpsDeviceRepository;
            _tripRepository = tripRepository;
            _mapper = mapper;
            _deviceHubService = deviceHubService;
            _coordinateHubService = coordinateHubService;
        }

        public async Task<InsertCoordinatesResponse> Handle(InsertCoordinatesCommand request, CancellationToken cancellationToken)
        {
            // Holds coordinates insert response.
            InsertCoordinatesResponse insertCoordinatesResponse = new InsertCoordinatesResponse { Success = true };

            // Checking if device with Id exists.
            var device = await _gpsDeviceRepository.GetByIdAsync(request.DeviceId);

            try
            {
                if (device != null)
                {
                    if (device.Status != GpsDeviceStatus.Disabled)
                    {
                        // Checking if the trip for a device is already in progress or
                        // a new trip for a device will be created.
                        Trip tripInProgressForDevice = await _tripRepository.CheckIfTripBelongsToDeviceAsync(request.DeviceId, request.TripId);

                        if (tripInProgressForDevice == null)
                        {
                            // There is no such a trip for a device so a new trip will be created.
                            if (_tripRepository.CheckIfTripExistsAsync(request.TripId) == null)
                            {
                                // Pair a new trip with device.
                                await _tripRepository.AddAsync(
                                        new Trip
                                        {
                                            TripId = request.TripId,
                                            GpsDeviceId = request.DeviceId,
                                            Start = (DateTime)request.Coordinates[0].Date_Time,
                                            End = (DateTime)request.Coordinates[request.Coordinates.Count - 1].Date_Time
                                        }
                                    );
                            }
                            else
                            {
                                insertCoordinatesResponse.Success = false;
                                insertCoordinatesResponse.Errors.Add("Trip is obsolete");
                                return insertCoordinatesResponse;
                            }
                        }

                        tripInProgressForDevice = await _tripRepository.CheckIfTripBelongsToDeviceAsync(request.DeviceId, request.TripId);
                        
                        if (tripInProgressForDevice != null)
                        {
                            // There is a trip in progress for a device.
                            // Inserting received coordinates into specific trip defined in insert coordinates command.
                            await _coordinateRepository.InsertGpsCoordinatesAsync(
                                _mapper.Map<InsertCoordinatesCommand, IList<GpsCoordinate>>(request)
                            );
                            
                            if (device.IntervalChanged)
                            {
                                // Telling the device that is should change the Interval of data sending.
                                insertCoordinatesResponse.Message = InsertCoordinatesResponse.DeviceMessage.CHANGE_INTERVAL;
                                insertCoordinatesResponse.Settings = new InsertCoordinatesResponse.DeviceSettings { Interval = device.Interval };

                                // Changing device IntervalChanged value.
                                device.IntervalChanged = false;
                            }

                            if (device.Status != GpsDeviceStatus.Online)
                            {
                                // Changing device status to online.
                                device.Status = GpsDeviceStatus.Online;
                            }

                            // If device values were updated then update database.
                            if (device.IntervalChanged)
                            {
                                // Updating device Status and IntervalChanged values.
                                await _gpsDeviceRepository.UpdateDeviceAsync(device);
                            }                           

                            // Sending signalr message that new data were acquired for device and a its trip.
                            await _coordinateHubService.SendUpdateMessageToUserGroup(device.UserId, device.GpsDeviceId, tripInProgressForDevice, "");

                            // Sending signalr message that device is online again.
                            await _deviceHubService.SendUpdateMessageToUserGroup(device.UserId, "");
                            
                            return insertCoordinatesResponse;
                        }
                        else
                        {
                            insertCoordinatesResponse.Success = false;
                            insertCoordinatesResponse.Errors.Add("Invalid operation");
                            return insertCoordinatesResponse;
                        }
                    }
                    else
                    {
                        insertCoordinatesResponse.Success = false;
                        insertCoordinatesResponse.Message = InsertCoordinatesResponse.DeviceMessage.DISABLE;
                        insertCoordinatesResponse.Errors.Add("Device with Id: '" + request.DeviceId + "' is not allowed to use");
                        return insertCoordinatesResponse;
                    }
                }
                else
                {
                    insertCoordinatesResponse.Success = false;
                    insertCoordinatesResponse.Errors.Add("Device with Id: '" + request.DeviceId + "' doesn't exist");
                    return insertCoordinatesResponse;
                }
            }
            catch (InvalidOperationException ioe)
            {
                insertCoordinatesResponse.Success = false;
                insertCoordinatesResponse.Errors.Add("Invalid operation");
                return insertCoordinatesResponse;
            }
            catch (DbUpdateException dbue)
            {
                insertCoordinatesResponse.Success = false;
                insertCoordinatesResponse.Errors.Add("Operation failed while updating data");
                return insertCoordinatesResponse;
            }
        }
    }
}
