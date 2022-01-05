using MediatR;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Queries.DeviceQueries;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.DeviceUseCase
{
    public class DetailDeviceQueryHandler : IRequestHandler<DetailDeviceQuery, DetailDeviceResponse>
    {
        private readonly IGpsDeviceRepository _gpsDeviceRepository;

        public DetailDeviceQueryHandler(
            IGpsDeviceRepository gpsDeviceRepository
            )
        {
            _gpsDeviceRepository = gpsDeviceRepository;
        }

        public async Task<DetailDeviceResponse> Handle(DetailDeviceQuery request, CancellationToken cancellationToken)
        {
            // Holds device detail response
            DetailDeviceResponse detailDeviceResponse = new DetailDeviceResponse { Success = true };


            return detailDeviceResponse;
        }
    }
}
