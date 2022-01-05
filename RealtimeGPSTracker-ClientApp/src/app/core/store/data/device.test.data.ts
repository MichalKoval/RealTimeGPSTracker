import { IDevicesItem, DeviceStatus, IDeviceCounts } from '../../models/device.model';

export const devicesItems: IDevicesItem[] = [
    {
        id: "00000000-0000-0000-0000-000000000000",
        name: "Device0",
        createTime: "1/1/2020 12:00:00 AM",
        color: "#000000",
        interval: 1000,
        status: DeviceStatus.OFFLINE,
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,
        toAdd: false,
        toUpdate: false,
        toDelete: false
    },
    {
        id: "00000000-0000-0000-0000-000000000001",
        name: "Device1",
        createTime: "2/1/2020 12:01:00 AM",
        color: "#b63d3d",
        interval: 1500,
        status: DeviceStatus.ONLINE,
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,
        toAdd: false,
        toUpdate: false,
        toDelete: false
    },
    {
        id: "00000000-0000-0000-0000-000000000002",
        name: "Device2",
        createTime: "3/1/2020 12:02:00 AM",
        color: "#3ca56f",
        interval: 1000,
        status: DeviceStatus.ONLINE,
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,
        toAdd: false,
        toUpdate: false,
        toDelete: false
    },
    {
        id: "00000000-0000-0000-0000-000000000003",
        name: "Device3",
        createTime: "4/1/2020 12:03:00 AM",
        color: "#3545d6",
        interval: 2000,
        status: DeviceStatus.OFFLINE,
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,
        toAdd: false,
        toUpdate: false,
        toDelete: false
    },
    {
        id: "00000000-0000-0000-0000-000000000004",
        name: "Device4",
        createTime: "5/1/2020 12:04:00 AM",
        color: "#9fd21f",
        interval: 5000,
        status: DeviceStatus.OFFLINE,
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,
        toAdd: false,
        toUpdate: false,
        toDelete: false
    },
    {
        id: "00000000-0000-0000-0000-000000000005",
        name: "Device5",
        createTime: "6/1/2020 12:05:00 AM",
        color: "#ca5ca3",
        interval: 2500,
        status: DeviceStatus.AWAY,
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,
        toAdd: false,
        toUpdate: false,
        toDelete: false
    },
    {
        id: "00000000-0000-0000-0000-000000000006",
        name: "Device6",
        createTime: "7/1/2020 12:06:00 AM",
        color: "#872fc6",
        interval: 4000,
        status: DeviceStatus.OFFLINE,
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,
        toAdd: false,
        toUpdate: false,
        toDelete: false
    }    
]

export const deviceCounts = {
    // Offline devices count setting
    //0: 0,
    [DeviceStatus.OFFLINE]: 0,
    // Online devices count setting
    //1: 0,
    [DeviceStatus.ONLINE]: 0,
    // Away devices count setting
    //2: 0,
    [DeviceStatus.AWAY]: 0,
    // All devices count setting
    //4: 0
    [DeviceStatus.ALL]: 0
}