import { Device } from './device.model';
import { IApiRequest } from './request.model';

export interface IRealtimeData {
    onlineDevices: Device[];
    awayDevices: Device[];
}

export interface IRealtimeDataRequest extends IApiRequest {
    
}