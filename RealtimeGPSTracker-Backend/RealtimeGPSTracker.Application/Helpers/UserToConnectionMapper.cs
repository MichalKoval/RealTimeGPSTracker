using RealtimeGpsTracker.Core.Interfaces.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RealtimeGPSTracker.Application.Helpers
{
    /// <summary>
    /// Genericka trieda reprezentuje vztah uzivatelov a ich spojeni. (Pouzite pri mapovani uzivatelov a spojeni v SignalR, Hub).
    /// </summary>
    /// <typeparam name="T">Parameter reprezentujuci uzivatela.</typeparam>
    public class UserToConnectionMapper<T> : IUserToConnectionMapper<T>
    {
        // Slovnik: 
        //             key -> uzivatel,
        //           value -> zoznam id pripojeni pre daneho uzivatela
        private readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>();

        /// <summary>
        /// Metoda vrati celkovy pocet uzivatelov, ktoruy nadviazali nejake spojenie.
        /// </summary>
        public int Count
        {
            get { return _connections.Count; }
        }

        /// <summary>
        /// Metoda vrati spojenia pre konkretneho uzivatela, ak uzivatel je v zozname a ma pridelene spojenia. Inac vratime prazdny zoznam spojeni.
        /// </summary>
        public IEnumerable<string> GetConnections(T key)
        {
            // Uzivatel ma pridelene aspon jedno spojenie
            if (_connections.TryGetValue(key, out HashSet<string> connections))
            {
                return connections;
            }

            // Uzivatel nema pridelene ziadne spojenia
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Metoda prida k uzivatelovi jeho pripojenie. Ak uzivatel este nie je v zozname, prida sa prv uzivatel a potom jeho pripojenie. Jeden uzivatel moze nazdviazat viacero spojeni (viacero otvorenych okien v prehliadaci, rozne zariadenia z ktorych je uzivatel prihlaseni, atd...) 
        /// </summary>
        /// <param name="key">Typ reprezentujuci uzivatela.</param>
        /// <param name="connectionId">Id spojenia, ktore chceme priradit k uzivatelovi.</param>
        public void Add(T key, string connectionId)
        {
            // Zamedzime konfliktu pri paralenom pristupe k zoznamu uzivatelov a ich spojeni
            lock (_connections)
            {
                HashSet<string> connections;

                // Ak uzivatel este nie je v zozname pridame ho. Ak uz je v zozname a vrati sa nam out parametrom jeho spojenia
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();

                    // Mapujeme uzivatela a jeho zoznam spojeni
                    _connections.Add(key, connections);
                }

                // Zamedzime konfliktu pri paralenom spracovani zoznamu spojeni daneho uzivatela
                lock (connections)
                {
                    // Pridavame spojenie do zoznamu spojeni konkretneho uzivatela, ktory sa uz v zozname nachadza.
                    connections.Add(connectionId);
                }
            }
        }

        /// <summary>
        /// Metoda odoberie spojenie uzivatela. Ak ma este nejake spojenia v zozname.
        /// </summary>
        /// <param name="key">Typ reprezentujuci uzivatela.</param>
        /// <param name="connectionId">Id spojenia, ktore chceme uzivatelovi odobrat.</param>
        public void Remove(T key, string connectionId)
        {
            // Zamedzime konfliktu pri paralenom pristupe k zoznamu uzivatelov a ich spojeni
            lock (_connections)
            {
                HashSet<string> connections;

                // Pokusime sa zistit ci su uzivatelovi pridelene nejake spojenia. AK nie vtratime sa z metody.
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                // Uzivatel ma stale pridelene nejake spojenia. Medzi nimi najdeme to, ktore chceme zmazat, ak existuje.
                // Zamedzime konfliktu pri paralenom spracovani zoznamu spojeni daneho uzivatela
                lock (connections)
                {
                    connections.Remove(connectionId);

                    // Ak uz uzivatel nema zozname ziadne spojenia, odoberie ho zo zoznamu uzivatelov a ich spojeni.
                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}
