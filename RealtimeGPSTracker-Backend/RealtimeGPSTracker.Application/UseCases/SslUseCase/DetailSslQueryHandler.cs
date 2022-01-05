using MediatR;
using RealtimeGpsTracker.Core.Dtos.Responses.SslResponses;
using RealtimeGpsTracker.Core.Interfaces.Services;
using RealtimeGpsTracker.Core.Queries.SslQueries;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.SslUseCase
{
    public class DetailSslQueryHandler : IRequestHandler<DetailSslQuery, DetailSslResponse>
    {
        private readonly ISslService _sslService;

        public DetailSslQueryHandler(
            ISslService sslService
            )
        {
            _sslService = sslService;
        }

        public async Task<DetailSslResponse> Handle(DetailSslQuery request, CancellationToken cancellationToken)
        {
            return _sslService.GetSsl(request.FileName);
        }
    }
}
