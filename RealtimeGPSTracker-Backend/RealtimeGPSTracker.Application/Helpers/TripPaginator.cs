using System.Linq;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;
using static RealtimeGpsTracker.Core.Dtos.Responses.Pagination.PaginationParameters;

namespace RealtimeGPSTracker.Application.Helpers
{
    public class TripPaginator
    {
        public static PaginatedList<Trip> GetPaginatedTrips(IQueryable<Trip> tripsQueryable, TripPaginationParameters paginationParams)
        {
            // Zaciatok trasy musi byt v intervale [startDT, ..., endDT] vratane
            if (paginationParams.Start != null && paginationParams.End != null)
            {
                tripsQueryable = tripsQueryable.Where(t => t.InProgress.Equals(false) && t.Start >= paginationParams.Start && t.Start <= paginationParams.End);
            }

            // Specialne pre 'trips' smer zoradenia, zoradit podla OrderBy property
            if (paginationParams.Order == OrderDirection.Asc)
            {
                switch (paginationParams.OrderBy)
                {
                    case TripPaginationParameters.TripOrderBy.Start:
                        tripsQueryable = tripsQueryable.OrderBy(t => t.Start);
                        break;
                    case TripPaginationParameters.TripOrderBy.End:
                        tripsQueryable = tripsQueryable.OrderBy(t => t.End);
                        break;
                    default:
                        tripsQueryable = tripsQueryable.OrderBy(t => t.End);
                        break;
                }
            }
            else
            {
                switch (paginationParams.OrderBy)
                {
                    case TripPaginationParameters.TripOrderBy.Start:
                        tripsQueryable = tripsQueryable.OrderByDescending(t => t.Start);
                        break;
                    case TripPaginationParameters.TripOrderBy.End:
                        tripsQueryable = tripsQueryable.OrderByDescending(t => t.End);
                        break;
                    default:
                        tripsQueryable = tripsQueryable.OrderByDescending(t => t.End);
                        break;
                }
            }

            return new PaginatedList<Trip>(
                tripsQueryable,
                paginationParams.PageIndex,
                paginationParams.PageSize
            );
        }
    }
}
