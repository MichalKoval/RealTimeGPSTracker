using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RealtimeGpsTracker.Core.Interfaces.Helpers;

namespace RealtimeGPSTracker.Application.Hubs
{
    /// <summary>
    /// Trieda deklaruje metody(odvodene od SignalR Hub) OnConnectedAsync(), OnDisconnectedAsync() pre spravu spojenia s klientom.
    /// Ulohou tejto triedy je udrziavat informacie a klientoch a ich spojeniach vramci SignalR Hubu.
    /// </summary>
    public class BaseHub : Hub
    {
        // Uzivatelia budu reprezentovani podla Id v databaze (string, hash)
        // K spojeniu daneho uzivatela(klienta) bolo pridelene ConnectionId automaticky pri vytvoreni spojenia.
        private readonly IUserToConnectionMapper<string> _connections;


        public BaseHub(IUserToConnectionMapper<string> connections)
        {
            _connections = connections;
        }

        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;

            _connections.Add(userId, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier;

            _connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
