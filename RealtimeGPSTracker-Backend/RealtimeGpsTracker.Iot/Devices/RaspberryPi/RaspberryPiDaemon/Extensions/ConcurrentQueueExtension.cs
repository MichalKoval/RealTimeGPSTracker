using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RaspberryPiDaemon.Extensions
{
    public static class ConcurrentQueueExtension
    {
        /// Metoda z FIFO fronty vysype n prvkov(ide o Thread-Safe frontu). Ak sa nenachadza n prkov vo fronte, tak vysypeme iba tolko kolko ich je
        public static IEnumerable<T> DequeueMoreItems<T>(this ConcurrentQueue<T> concurrentQueue, int itemsCount)
        {
            int concurrentQueueCount = concurrentQueue.Count;

            //Vyberieme len tolko prvkov kolko pojde. Ak je pocet vo fronte mensi ako pozadovany
            if (itemsCount > concurrentQueueCount)
            {
                itemsCount = concurrentQueueCount;
            }

            for (int i = 0; i < itemsCount && concurrentQueueCount > 0; i++)
            {
                // Skusime vybrat prvok z Thread-Safe queue
                while (!concurrentQueue.TryDequeue(out T item))

                    yield return item;
            }
        }
    }
}
