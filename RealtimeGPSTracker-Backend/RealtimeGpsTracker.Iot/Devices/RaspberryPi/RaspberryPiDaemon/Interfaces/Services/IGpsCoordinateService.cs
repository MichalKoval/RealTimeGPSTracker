using Microsoft.Extensions.Hosting;
using RaspberryPiDaemon.Entities;
using System;
using System.Collections.Generic;

namespace RaspberryPiDaemon.Interfaces.Services
{
    public interface IGpsCoordinateService : IHostedService, IDisposable
    {
        void Add(GpsCoordinate coord);
        List<GpsCoordinate> GetAllCoordinates();
        List<GpsCoordinate> GetCoordinates(int count);
    }
}
