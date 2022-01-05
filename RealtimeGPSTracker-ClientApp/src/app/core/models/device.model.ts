import { INavigationLinks } from "../../core/models/url.model";
import { IPaginationHeader, IPaginationSettings } from "../../core/models/pagination.model";
import { IUpdateMessage } from "../../core/models/signalr.model";
import { IApiRequest } from 'src/app/core/models/request.model';
import { ISortSettings } from './sort.model';
import { IApiResponse } from './response.model';
import { IFilterSettings } from './filter.model';

export interface IDevicesDataSettings {
    pagination: IPaginationSettings;
    filter: IDevicesFilterSettings;
    sort: IDevicesSortSettings;

    count: { [deviceStatus: number]: number; };
}

export interface IDevicesItem {
    // Device details
    id: string;
    name: string;
    createTime: string;
    color: string;
    interval: number;
    status: DeviceStatus;

    // flags
    isSelected: boolean;
    isEdited: boolean;
    isShownOnMap: boolean;
    toAdd: boolean;
    toUpdate: boolean;
    toDelete: boolean;
}

export enum DeviceOrderBy {
    NAME = 'Name',
    CREATETIME = 'CreateTime',
    COLOR = 'Color',
    INTERVAL = 'Interval',
    STATUS = 'Status'
}

export enum DeviceStatus {
    OFFLINE = 0,
    ONLINE = 1,
    AWAY = 2,
    DISABLED = 3,
    ALL = 4
}

export class DeviceFilterCriterion {
    static readonly STATUS_OFFLINE = new DeviceFilterCriterion(
        'Status',
        DeviceStatus.OFFLINE.toString(),
        'Offline'
    );
    static readonly STATUS_ONLINE = new DeviceFilterCriterion(
        'Status',
        DeviceStatus.ONLINE.toString(),
        'Online'
    );
    static readonly STATUS_AWAY = new DeviceFilterCriterion(
        'Status',
        DeviceStatus.AWAY.toString(),
        'Away'
    );

    private constructor(
        public fieldName: string,
        public fieldValue: string,
        public description?: string
    ) {
        
    }
}

export interface IDevicesSortSettings extends ISortSettings {
    orderBy: DeviceOrderBy
}

export interface IDevicesFilterSettings extends IFilterSettings {
    criteria: DeviceFilterCriterion[]
}

export interface IDevicesResponse extends IApiResponse {
    paginationHeader: IPaginationHeader;
    navigationLinks: INavigationLinks[];
    items: Device[];
    deviceCounts: IDeviceCounts;
}

export interface IDeviceToDelete {
    deviceId: string;
}

export interface IDevicesToDeleteRequest extends IApiRequest {
    deviceIds: string[];
}

export interface IDeviceRequest extends IApiRequest {
    deviceId: string;
}

export class Device {
    id: string = "00000000-0000-0000-0000-000000000000";
    name: string = "";
    createTime: string = "1/1/0001 12:00:00 AM";
    color: string = "#000000";
    interval: number = 500;
    status: DeviceStatus = DeviceStatus.OFFLINE;
}

export class DeviceBadge {
    constructor (
        public style: string,
        public text: string
    ) {}
}

export interface IDeviceCounts {
    offlineCount: string;
    onlineCount: string;
    awayCount: string;
    allCount: string;
}

export interface IUpdateDeviceListMessage extends IUpdateMessage {

}

export class DeviceMessages {
    static msgs = {
        'AllDevices': [
            { message: 'There are no devices. Please add some.' }
        ],
        'OnlineDevices': [
            { message: 'Currently, there are no Online devices.' }
        ],
        'AwayDevices': [
            { message: 'Currently, there are no Away devices.' }
        ],
        'OfflineDevices': [
            { message: 'Currently, there are no Offline devices.' }
        ]
    }
}

export class DeviceValidationErrorMessages {
    static errors = {
        'name': [
            { type: 'required', message: 'Device name is required' },
            { type: 'minlength', message: 'Device name must be at least 3 characters long' },
            { type: 'maxlength', message: 'Device name must be at most 128 characters long' }
        ],
        'interval': [
            { type: 'required', message: 'Interval is required (in ms)' },
            { type: 'min', message: 'Interval must be at least 500 ms' },
            { type: 'pattern', message: 'Interval must contain only numbers' }
        ],
        'color': [
            { type: 'required', message: 'Please select color' },
        ]
    }
}

export class DeviceHexColorValues {
    static values = [
        '#000000',
        '#ffc0cb',
        '#ffffff',
        '#008080',
        '#ffe4e1',
        '#ff0000',
        '#ffd700',
        '#00ffff',
        '#40e0d0',
        '#ff7373',
        '#e6e6fa',
        '#d3ffce',
        '#0000ff',
        '#ffa500',
        '#f0f8ff',
        '#b0e0e6',
        '#7fffd4',
        '#c6e2ff',
        '#faebd7',
        '#800080',
        '#eeeeee',
        '#cccccc',
        '#fa8072',
        '#ffb6c1',
        '#800000',
        '#00ff00',
        '#333333',
        '#003366',
        '#ffff00',
        '#20b2aa',
        '#c0c0c0',
        '#ffc3a0',
        '#f08080',
        '#fff68f',
        '#f6546a',
        '#468499',
        '#66cdaa',
        '#ff6666',
        '#666666',
        '#c39797',
        '#00ced1',
        '#ffdab9',
        '#ff00ff',
        '#660066',
        '#008000',
        '#088da5',
        '#f5f5f5',
        '#c0d6e4',
        '#8b0000',
        '#0e2f44',
        '#ff7f50',
        '#afeeee',
        '#808080',
        '#990000',
        '#b4eeb4',
        '#dddddd',
        '#daa520',
        '#ffff66',
        '#cbbeb5',
        '#00ff7f',
        '#f5f5dc',
        '#8a2be2',
        '#81d8d0',
        '#ff4040',
        '#b6fcd5',
        '#66cccc',
        '#3399ff',
        '#a0db8e',
        '#cc0000',
        '#794044',
        '#ccff00',
        '#000080',
        '#3b5998',
        '#999999',
        '#191970',
        '#0099cc',
        '#fef65b',
        '#ff4444',
        '#31698a',
        '#6897bb',
        '#ff1493',
        '#f7f7f7',
        '#191919',
        '#6dc066'
    ]
}