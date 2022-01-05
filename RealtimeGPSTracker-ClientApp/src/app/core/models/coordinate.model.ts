import { IUpdateMessage, SignalRHub } from './signalr.model';
import { IApiResponse } from './response.model';

export interface ICoordinatesData {
    coordinates: {
        [deviceId: string]: {
            [tripId: string]: {
                startDt: string;
                endDt: string;
                coordinates: ICoordinate[];
            } 
        }
    };
}

export interface ICoordinatesDataResponse extends IApiResponse {
    deviceId: string;
    tripId: string;
    coordinates: ICoordinate[];
}

export interface ICoordinate {
    id: string;
    time: string;
    lat: number;
    lng: number;
    speed: number | 0;
}

export interface IUpdateCoordinateListMessage extends IUpdateMessage {
    deviceId: string;
    tripId: string;
    startDt: string;
    endDt: string;
}

export interface CoordinateHub extends SignalRHub {

}