using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RealtimeGpsTracker.Core.Dtos.Responses.HubMessages;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGPSTracker.Application.Hubs;

namespace RealtimeGPSTracker.Application.Services.HubServices
{
    public class DeviceHubService : IDeviceHubService
    {
        private readonly IHubContext<DeviceHub> _deviceHubContext;

        public DeviceHubService(IHubContext<DeviceHub> deviceHubContext)
        {
            _deviceHubContext = deviceHubContext;
        }

        public async Task SendNewOnlineDeviceMessageToUserGroup(string userId, string deviceName)
        {
            await _deviceHubContext.Clients.Group(userId).SendAsync(
                "OnlineDeviceAppeared",
                new DeviceHubMessage(
                    deviceName
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId)
        {
            await _deviceHubContext.Clients.Group(userId).SendAsync(
                "UpdateDeviceList",
                new DeviceHubMessage(
                    "UPDATE_DEVICE_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, string excludedConnectionId)
        {
            await _deviceHubContext.Clients.GroupExcept(userId, excludedConnectionId).SendAsync(
                "UpdateDeviceList",
                new DeviceHubMessage(
                    "UPDATE_DEVICE_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }

        public async Task SendUpdateMessageToUserGroup(string userId, IReadOnlyList<string> excludedConnectionIds)
        {
            await _deviceHubContext.Clients.GroupExcept(userId.ToString(), excludedConnectionIds).SendAsync(
                "UpdateDeviceList",
                new DeviceHubMessage(
                    "UPDATE_DEVICE_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                )
            );
        }
    }
}
