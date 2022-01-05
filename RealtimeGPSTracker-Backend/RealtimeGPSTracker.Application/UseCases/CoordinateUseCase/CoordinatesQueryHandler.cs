using MediatR;
using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Dtos.Responses.CoordinateResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Queries.CoordinateQueries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.CoordinateUseCase
{
    public class CoordinatesQueryHandler : IRequestHandler<CoordinatesQuery, CoordinatesResponse>
    {
        private readonly IGpsCoordinateRepository _coordinateRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IGpsDeviceRepository _gpsDeviceRepository;

        public CoordinatesQueryHandler(
            IGpsCoordinateRepository coordinateRepository,
            ITripRepository tripRepository,
            IGpsDeviceRepository gpsDeviceRepository
            )
        {
            _coordinateRepository = coordinateRepository;
            _tripRepository = tripRepository;
            _gpsDeviceRepository = gpsDeviceRepository;
        }

        public async Task<CoordinatesResponse> Handle(CoordinatesQuery request, CancellationToken cancellationToken)
        {
            // Holds coordinates response
            CoordinatesResponse coordinatesResponse = new CoordinatesResponse { Success = true };

            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                // Checking if start datetime is less then or equal to end datime
                int resultOfDatimeComparison;
                if ((resultOfDatimeComparison = DateTime.Compare(request.Start, request.End)) <= 0)
                {
                    // Checking if device belongs to owner
                    GpsDevice device = await _gpsDeviceRepository.CheckIfDeviceBelongsToOwner(request.DeviceId, request.OwnerId);
                    
                    //TODO: check if device has trip with given id


                    // Checking if trip with coordinates belongs to owner
                    Trip tripWhichBelongsToOwner = await _tripRepository.CheckIfTripBelongsToOwnerAsync(request.OwnerId, request.TripId);

                    if (tripWhichBelongsToOwner != null)
                    {
                        var coordinates = await _coordinateRepository.GetGpsCoordinates(request.TripId, request.Start, request.End).ToListAsync();

                        if (coordinates != null)
                        {
                            coordinatesResponse.Coordinates = coordinates;
                        }
                        else
                        {
                            coordinatesResponse.Success = false;
                            coordinatesResponse.Errors.Add("There are no coordinates for a given trip, time interval and the owner");
                        }
                    }
                    else
                    {
                        coordinatesResponse.Success = false;
                        coordinatesResponse.Errors.Add("Trip with given time interval doesn't exists for the owner");
                    }
                }
                else
                {
                    coordinatesResponse.Success = false;
                    coordinatesResponse.Errors.Add("Start datetime must be less then or equal to End datetime");
                }
            }
            else
            {
                coordinatesResponse.Success = false;
                coordinatesResponse.Errors.Add("User was not identified while attempting to get list of coordinates");
            }

            return coordinatesResponse;
        }
    }
}
