using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Commands;
using RealtimeGpsTracker.Core.Commands.DeviceCommands;
using RealtimeGpsTracker.Core.Dtos.Responses;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.DeviceUseCase
{
    public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, UpdateDeviceResponse>
    {
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly IDeviceHubService _deviceHubService;
        private readonly IMapper _mapper;

        public UpdateDeviceCommandHandler(
            IGpsDeviceRepository gpsDeviceRepository,
            IDeviceHubService deviceHubService,
            IMapper mapper
            )
        {
            _gpsDeviceRepository = gpsDeviceRepository;
            _deviceHubService = deviceHubService;
            _mapper = mapper;
        }

        public async Task<UpdateDeviceResponse> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            // Holds device update response
            UpdateDeviceResponse updateDeviceResponse = new UpdateDeviceResponse { Success = true };

            if (string.IsNullOrEmpty(request.OwnerId))
            {
                updateDeviceResponse.Success = false;
                updateDeviceResponse.Errors.Add("User was not identified while attempting to update the device");
                return updateDeviceResponse;
            }

            GpsDevice deviceWhichBelongsToOwner = await _gpsDeviceRepository.CheckIfDeviceBelongsToOwner(request.Id, request.OwnerId);
            if (deviceWhichBelongsToOwner == null)
            {
                updateDeviceResponse.Success = false;
                updateDeviceResponse.Errors.Add("You don't have a permission to change this device");
                return updateDeviceResponse;
            }

            // If device interval was changed flag is set to the device knows that it needs to be update in next connection to the server
            // TODO: fix device interval to be number !!!
            if (deviceWhichBelongsToOwner.Interval != request.Interval)
            {
                deviceWhichBelongsToOwner.IntervalChanged = true;
            }

            deviceWhichBelongsToOwner = _mapper.Map<UpdateDeviceCommand, GpsDevice>(request, deviceWhichBelongsToOwner);

            try
            {
                await _gpsDeviceRepository.UpdateDeviceAsync(deviceWhichBelongsToOwner);
                //_gpsDeviceRepository.UpdateDevice(gpsDevice);

                // Kedze doslo k uprave zariadenia, upozornime na to aj vsetkych ostatne otvorene stranky (mobil, tablet, alebo v tom istom prehliadaci viac krat)
                // daneho uzivatela, okrem stranky odkial bol odoslany poziadavok pre upravu, kedze si dana stranka obnovi zoznam sama.

                await _deviceHubService.SendUpdateMessageToUserGroup(request.OwnerId, "");

            }
            catch (DbUpdateConcurrencyException dbuce)
            {
                updateDeviceResponse.Success = false;
                updateDeviceResponse.Errors.Add(dbuce.Message);
            }
            catch (DbUpdateException dbue)
            {
                updateDeviceResponse.Success = false;
                updateDeviceResponse.Errors.Add(dbue.Message);
            }

            return updateDeviceResponse;
        }
    }
}
