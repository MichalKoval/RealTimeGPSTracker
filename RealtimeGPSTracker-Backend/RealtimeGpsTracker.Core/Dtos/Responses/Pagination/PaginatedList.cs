using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RealtimeGpsTracker.Core.Dtos.Responses.Pagination
{
    public class PaginatedList<T>
    {
        private IQueryable<T> _queryableItems;

        public PaginatedList(IQueryable<T> queryableItems, int pageIndex, int pageSize)
        {
            _queryableItems = queryableItems;
            int queryableItemsCount = queryableItems.Count();

            TotalCount = queryableItemsCount;
            SizePerPage = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(queryableItemsCount / (double)pageSize);
        }

        public int TotalCount { get; }
        public int SizePerPage { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }

        public async Task<IList<T>> ListAsync()
        {
            // Nezabudnut posunut PageIndex aby zacinal od nuly, kedze pri prvej stranke nechceme preskocit n prvkov
            int skipRowsCount = SizePerPage * (PageIndex - 1);

            return await _queryableItems
                .Skip(skipRowsCount)
                .Take(SizePerPage)
                .ToListAsync();
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public int NextPageNumber => HasNextPage ? (PageIndex + 1) : TotalPages;
        public int PreviousPageNumber =>HasPreviousPage ? (PageIndex - 1) : 1;

        public PaginationHeader GetPaginationHeader()
        {
            return new PaginationHeader(TotalCount, PageIndex, SizePerPage, TotalPages);
        }

    }
}
