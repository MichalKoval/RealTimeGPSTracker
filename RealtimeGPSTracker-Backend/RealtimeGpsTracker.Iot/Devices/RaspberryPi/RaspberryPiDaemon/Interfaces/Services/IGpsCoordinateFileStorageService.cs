﻿using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPiDaemon.Interfaces.Services
{
    public interface IGpsCoordinateFileStorageService : IHostedService, IDisposable
    {
    }
}
