using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RaspberryPiDaemon.Entities;
using RaspberryPiDaemon.Extensions;
using RaspberryPiDaemon.Interfaces.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Services
{
    public class GpsCoordinateService : IHostedService, IDisposable
    {
        private readonly ILogger<GpsCoordinateService> _logger;
        private readonly GpsCoordinateFileStorageService _gpsCoordinateFileStorageService;

        /// <summary>
        /// Thread-Safe FIFO Datova struktura pre pridavanie a odoberianie suradnic asychronne
        /// </summary>
        private readonly ConcurrentQueue<GpsCoordinate> _alreadyCollectedCoords;

        public GpsCoordinateService(
            ILogger<GpsCoordinateService> logger,
            GpsCoordinateFileStorageService gpsCoordinateFileStorageService

            )
        {
            _logger = logger;
            _gpsCoordinateFileStorageService = gpsCoordinateFileStorageService;
            _alreadyCollectedCoords = new ConcurrentQueue<GpsCoordinate>();
        }

        /// <summary>
        /// Pridaj na koniec FIFO Thread-Safe
        /// </summary>
        /// <param name="coord">Suradnica, ktoru chceme pridat nakoniec FIFO</param>
        public void Add(GpsCoordinate coord)
        {
            _alreadyCollectedCoords.Enqueue(coord);
        }

        /// <summary>
        /// Metoda vyberie z doposial nazbieranych suradnic urceny pocet. Vyberaju sa vzdy suradnice, ktore prisli drive.
        /// </summary>
        /// <param name="count">Aky pocet dat chceme vybrat z doposial nazbieranych</param>
        /// <returns></returns>
        public List<GpsCoordinate> GetCoordinates(int count)
        {
            if (_alreadyCollectedCoords.Count == 0)
                return null;

            lock (_alreadyCollectedCoords)
            {
                if (_alreadyCollectedCoords.Count > 0)
                {
                    // DequeueMoreItems metoda bola umelo pridana k standartnym metodam kolekcie ConcurrentQueue, vid: 'BetterConcurrentQueue.cs'
                    // Ak pozadovany pocet prvkov je vacsi a realny pocet vo fronte, metoda DequeueMoreItems vrati iba tolko prvkov kolko ich tam je.
                    return _alreadyCollectedCoords.DequeueMoreItems(count).ToList();
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Metoda vyberie vsetky nazbierane data
        /// </summary>
        /// <returns></returns>
        public List<GpsCoordinate> GetAllCoordinates()
        {
            if (_alreadyCollectedCoords.Count == 0)
                return null;

            lock (_alreadyCollectedCoords)
            {
                int allCount;
                if ((allCount = _alreadyCollectedCoords.Count) > 0)
                {
                    return _alreadyCollectedCoords.DequeueMoreItems(allCount).ToList();
                }
                else
                    return null;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting gps coordinates concurrent queue service ...");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping gps coordinates concurrent queue service ...");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing coordinates concurrent queue service ...");
        }
    }
}
