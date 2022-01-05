using Microsoft.AspNetCore.SignalR;
using RealtimeGpsTracker.Core.Dtos.Responses.HubMessages;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGPSTracker.Application.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.Services.HubServices
{
    public class CoordinateHubService : ICoordinateHubService
    {
        private readonly IHubContext<CoordinateHub> _coordinateHubContext;

        public CoordinateHubService(IHubContext<CoordinateHub> coordinateHubContext)
        {
            _coordinateHubContext = coordinateHubContext;
        }

        public async Task SendUpdateMessageToUserGroup(string userId, string deviceId, Trip trip)
        {
            await _coordinateHubContext.Clients.Group(userId).SendAsync(
                "UpdateCoordinateList",
                new CoordinateHubMessage(
                    "UPDATE_COORDINATE_LIST",
                    deviceId,
                    trip.TripId,
                    trip.Start,
                    trip.End
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, string deviceId, Trip trip, string excludedConnectionId)
        {
            await _coordinateHubContext.Clients.GroupExcept(userId, excludedConnectionId).SendAsync(
                "UpdateCoordinateList",
                new CoordinateHubMessage(
                    "UPDATE_COORDINATE_LIST",
                    deviceId,
                    trip.TripId,
                    trip.Start,
                    trip.End
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, string deviceId, Trip trip, IReadOnlyList<string> excludedConnectionIds)
        {
            await _coordinateHubContext.Clients.GroupExcept(userId, excludedConnectionIds).SendAsync(
                "UpdateCoordinateList",
                new CoordinateHubMessage(
                    "UPDATE_COORDINATE_LIST",
                    deviceId,
                    trip.TripId,
                    trip.Start,
                    trip.End
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId)
        {
            await _coordinateHubContext.Clients.Group(userId).SendAsync(
                "UpdateCoordinateList",
                new CoordinateHubMessage(
                    "UPDATE_COORDINATE_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, string excludedConnectionId)
        {

            await _coordinateHubContext.Clients.GroupExcept(userId, excludedConnectionId).SendAsync(
                "UpdateCoordinateList",
                new CoordinateHubMessage(
                    "UPDATE_COORDINATE_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, IReadOnlyList<string> excludedConnectionIds)
        {
            await _coordinateHubContext.Clients.GroupExcept(userId, excludedConnectionIds).SendAsync(
                "UpdateCoordinateList",
                new CoordinateHubMessage(
                    "UPDATE_COORDINATE_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }
    }
}
