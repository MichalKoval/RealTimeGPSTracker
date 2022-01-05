using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using RealtimeGpsTracker.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RealtimeGpsTracker.Core.Dtos.Responses.Pagination
{
    public class PaginationParameters
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum OrderDirection
        {
            Asc,
            Desc
        }

        private const int MAX_SIZE_PER_PAGE = 100;
        private int _sizePerPage = MAX_SIZE_PER_PAGE;

        [FromQuery(Name = "PageIndex")]
        public int PageIndex { get; set; } = 1;

        // Kolko riadkov sa zobrazi vramci jednej stranky z n stranok, poces nepresiahne max velkost na jednu stranku
        [FromQuery(Name = "PageSize")]
        public int PageSize
        {
            get { return _sizePerPage; }
            set { _sizePerPage = (value > MAX_SIZE_PER_PAGE) ? MAX_SIZE_PER_PAGE : value; }
        }

        // Default hodnota pre smer zoradenia je vzostupne
        [FromQuery(Name = "Order")]
        public OrderDirection Order { get; set; } = OrderDirection.Asc;
    }

    public class TripPaginationParameters : PaginationParameters
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TripOrderBy
        {
            Start,
            End
        }

        // Default hodnota pre zoradenie podla ...
        [FromQuery(Name = "OrderBy")]
        public TripOrderBy OrderBy { get; set; } = TripOrderBy.Start;

        [Required]
        [FromQuery(Name = "Start")]
        public DateTime? Start { get; set; }

        [Required]
        [FromQuery(Name = "End")]
        public DateTime? End { get; set; }
    }

    public class DevicePaginationParameters : PaginationParameters
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DeviceOrderBy
        {
            Name,
            CreateTime,
            Color,
            Interval,
            Status
        }

        // Order by device details field. By default it is sorted by name.
        [FromQuery(Name = "OrderBy")]
        public DeviceOrderBy OrderBy { get; set; } = DeviceOrderBy.Name;

        // Status value is optional.
        // More statuses can be specified.
        // If no statuses are specified all status types of devices will be returned.
        [FromQuery(Name = "Status")]
        public GpsDeviceStatus[]? Statuses { get; set; }
    }
}
