using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Dtos.Responses.SslResponses;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Services
{
    public interface ISslService
    {
        DetailSslResponse GetSsl(string fileName);
    }
}
