using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace RealtimeGpsTracker.Core.Dtos.Responses.Pagination
{
    public class NavigationLink
    {
        public static IList<NavigationLink> CreateNavigationLinks<T>(PaginatedList<T> paginatedList, string queryRoute, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, string orderDirection, string orderBy)
        {
            IList<NavigationLink> navigationLinks = new List<NavigationLink>();

            if (paginatedList.HasPreviousPage) {
                navigationLinks.Add(
                    new NavigationLink(
                        queryRoute,
                        httpContextAccessor,
                        linkGenerator,
                        paginatedList.PreviousPageNumber,
                        paginatedList.SizePerPage,
                        orderDirection,
                        orderBy,
                        "previousPage",
                        "GET"
                    )
                );
            }

            navigationLinks.Add(
                new NavigationLink(
                    queryRoute,
                    httpContextAccessor,
                    linkGenerator,
                    paginatedList.PageIndex,
                    paginatedList.SizePerPage,
                    orderDirection,
                    orderBy,
                    "self",
                    "GET"
                )
            );

            if (paginatedList.HasNextPage) {
                navigationLinks.Add(
                    new NavigationLink(
                        queryRoute,
                        httpContextAccessor,
                        linkGenerator,
                        paginatedList.NextPageNumber,
                        paginatedList.SizePerPage,
                        orderDirection,
                        orderBy,
                        "nextPage",
                        "GET"
                    )
                );
            }

            return navigationLinks;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public NavigationLink(string queryRoute, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, int pageIndex, int sizePerPage, string orderDirect, string orderBy, string rel, string method)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;

            HRef = _linkGenerator.GetUriByPage(
                _httpContextAccessor.HttpContext,
                handler: null,
                values: new { PageIndex = pageIndex, PageSize = sizePerPage, Order = orderDirect, OrderBy = orderBy }
            );
            Rel = rel;
            Method = method;
        }

        [JsonProperty("href")]
        public string HRef { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
