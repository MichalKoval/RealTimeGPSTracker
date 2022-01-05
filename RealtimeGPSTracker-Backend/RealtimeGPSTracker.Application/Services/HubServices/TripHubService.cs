using Microsoft.AspNetCore.SignalR;
using RealtimeGpsTracker.Core.Dtos.Responses.HubMessages;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGPSTracker.Application.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.Services.HubServices
{
    public class TripHubService : ITripHubService
    {
        private readonly IHubContext<TripHub> _tripHubContext;

        public TripHubService(IHubContext<TripHub> tripHubContext)
        {
            _tripHubContext = tripHubContext;
        }

        public async Task SendUpdateMessageToUserGroup(string userId)
        {
            await _tripHubContext.Clients.Group(userId).SendAsync(
                "UpdateTripList",
                new TripHubMessage(
                    "UPDATE_TRIP_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, string excludedConnectionId)
        {
            await _tripHubContext.Clients.GroupExcept(userId, excludedConnectionId).SendAsync(
                "UpdateTripList",
                new TripHubMessage(
                    "UPDATE_TRIP_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, IReadOnlyList<string> excludedConnectionIds)
        {
            await _tripHubContext.Clients.GroupExcept(userId.ToString(), excludedConnectionIds).SendAsync(
                "UpdateTripList",
                new TripHubMessage(
                    "UPDATE_TRIP_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }
    }
}
