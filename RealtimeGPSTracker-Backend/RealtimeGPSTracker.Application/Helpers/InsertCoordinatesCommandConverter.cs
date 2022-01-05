using AutoMapper;
using RealtimeGpsTracker.Core.Commands.CoordinateCommands;
using RealtimeGpsTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static RealtimeGpsTracker.Core.Commands.CoordinateCommands.InsertCoordinatesCommand;

namespace RealtimeGPSTracker.Application.Helpers
{
    public class InsertCoordinatesCommandConverter : ITypeConverter<InsertCoordinatesCommand, IList<GpsCoordinate>>
    {
        public IList<GpsCoordinate> Convert(InsertCoordinatesCommand source, IList<GpsCoordinate> destination, ResolutionContext context)
        {
            return source.Coordinates.Select(
                    coord => new GpsCoordinate()
                    {
                        GpsCoordinateId = Guid.NewGuid().ToString(),
                        Time = DateTime.SpecifyKind(coord.Date_Time ?? DateTime.Now, DateTimeKind.Utc),
                        Lt = coord.Latitude.ToString(),
                        Lg = coord.Longitude.ToString(),
                        Speed = coord.Speed.ToString(),
                        TripId = source.TripId
                    }
                ).ToList();
        }
    }
}
