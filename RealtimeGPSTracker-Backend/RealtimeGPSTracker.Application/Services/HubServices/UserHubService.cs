using Microsoft.AspNetCore.SignalR;
using RealtimeGpsTracker.Core.Dtos.Responses.HubMessages;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGPSTracker.Application.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.Services.HubServices
{
    public class UserHubService : IUserHubService
    {
        private readonly IHubContext<UserHub> _userHubContext;

        public UserHubService(IHubContext<UserHub> userHubContext)
        {
            _userHubContext = userHubContext;
        }

        public async Task SendUpdateMessageToUserGroup(string userId)
        {
            await _userHubContext.Clients.Group(userId).SendAsync(
                "UpdateUserDetail",
                new UserHubMessage(
                    "UPDATE_USER_DETAIL: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, string excludedConnectionId)
        {
            await _userHubContext.Clients.GroupExcept(userId, excludedConnectionId).SendAsync(
                "UpdateUserDetail",
                new UserHubMessage(
                    "UPDATE_USER_DETAIL: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, IReadOnlyList<string> excludedConnectionIds)
        {
            await _userHubContext.Clients.GroupExcept(userId.ToString(), excludedConnectionIds).SendAsync(
                "UpdateUserDetail",
                new UserHubMessage(
                    "UPDATE_USER_DETAIL: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }
    }
}
