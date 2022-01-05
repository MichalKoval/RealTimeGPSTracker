using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands;
using RealtimeGpsTracker.Core.Commands.DeviceCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.DeviceUseCase
{
    public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, CreateDeviceResponse>
    {
        private readonly IGpsDeviceRepository _gpsDeviceRepository;
        private readonly IDeviceHubService _deviceHubService;
        private readonly IMapper _mapper;

        public CreateDeviceCommandHandler(
            IGpsDeviceRepository gpsDeviceRepository,
            IDeviceHubService deviceHubService,
            IMapper mapper
            )
        {
            _gpsDeviceRepository = gpsDeviceRepository;
            _deviceHubService = deviceHubService;
            _mapper = mapper;
        }

        public async Task<CreateDeviceResponse> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            // Holds device update response
            CreateDeviceResponse createDeviceResponse = new CreateDeviceResponse { Success = true };

            // Checking if id of owner who requested is available
            if (!string.IsNullOrEmpty(request.OwnerId))
            {
                var gpsDeviceToCreate = _mapper.Map<GpsDevice>(request);

                gpsDeviceToCreate.GpsDeviceId = Guid.NewGuid().ToString().ToUpper();
                gpsDeviceToCreate.CreateTime = DateTime.Now.ToUniversalTime();
                gpsDeviceToCreate.UserId = request.OwnerId;

                await _gpsDeviceRepository.AddAsync(gpsDeviceToCreate);

                // Kedze doslo k pridaniu noveho zariadenia, upozornime na to aj vsetkych ostatne otvorene stranky (mobil, tablet, alebo v tom istom prehliadaci viac krat)
                // daneho uzivatela, okrem stranky odkial bol odoslany poziadavok pre pridanie, kedze si dana stranka obnovi zoznam sama.

                await _deviceHubService.SendUpdateMessageToUserGroup(request.OwnerId, "");
            }
            else
            {
                createDeviceResponse.Success = false;
                createDeviceResponse.Errors.Add("User was not identified while attempting to create new device");
            }           

            return createDeviceResponse;
        }
    }
}
