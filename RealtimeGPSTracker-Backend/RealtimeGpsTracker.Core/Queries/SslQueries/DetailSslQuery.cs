using RealtimeGpsTracker.Core.Dtos.Responses.SslResponses;

namespace RealtimeGpsTracker.Core.Queries.SslQueries
{
    public class DetailSslQuery : AnonymousQuery<DetailSslResponse>
    {
        public string FileName { get; set; }
    }
}