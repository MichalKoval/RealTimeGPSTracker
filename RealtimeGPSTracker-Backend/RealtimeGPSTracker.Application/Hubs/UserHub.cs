using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RealtimeGpsTracker.Core.Dtos.Responses.HubMessages;

namespace RealtimeGPSTracker.Application.Hubs
{
    /// <summary>
    /// Trieda reprezentuje UserHub pre spojenia s uzivatelom. Je odvodena od triedy Hub a doplna metodu pre odosielanie sprav zvolenemu uzivatelovi, ktory nadviazal spojenie s Hubom.
    /// Trieda zaznamenava vsetky uzivatelove spojenia pomocou Groups, kde groupName je id uzivatela, a obsahom Group su vsetky spojenia, ktore uzivatel pre dany hub nadviazal. Moze ist o viacere otvorene tabs v prehliadaci, alebo rozne zariadenia s tym istym uzivatelom pristupuju k danemu hubu.
    /// </summary>
    public class UserHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        // Pri nadviazani spojenia zistime, komu dane spojenie patri a pridame ho do zoznamu
        public override Task OnConnectedAsync()
        {
            // Id uzivatela, ktory vyvolal dane spojenie
            string userId = Context.UserIdentifier;

            Groups.AddToGroupAsync(Context.ConnectionId, userId);

            return base.OnConnectedAsync();
        }
    }
}
