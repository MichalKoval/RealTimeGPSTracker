using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RealtimeGpsTracker.Core.Dtos.Responses.Pagination
{
    public class PaginationHeader
    {
        public static PaginationHeader CreatePaginationHeader<T>(PaginatedList<T> paginatedList)
        {
            return new PaginationHeader(
                paginatedList.TotalCount,
                paginatedList.PageIndex,
                paginatedList.SizePerPage,
                paginatedList.TotalPages
            );
        }

        public PaginationHeader(int totalCount, int pageIndex, int sizePerPage, int totalPages)
        {
            TotalItemsCount = totalCount;
            PageSize = sizePerPage;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }

        public string GenerateJson() {
            return JsonConvert.SerializeObject(
                this,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );
        }

        [JsonProperty("totalItemsCount")]
        public int TotalItemsCount { get; }

        [JsonProperty("pageSize")]
        public int PageSize { get; }

        [JsonProperty("pageIndex")]
        public int PageIndex { get; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; }
    }
}
