import { INavigationLinks } from "../../core/models/url.model";
import { IPaginationHeader, IPaginationSettings } from "../../core/models/pagination.model";
import { IUpdateMessage } from "../../core/models/signalr.model";
import { ICoordinate } from './coordinate.model';
import { Observable } from 'rxjs';
import { Device } from './device.model';
import { ISortSettings } from './sort.model';
import { IApiResponse } from './response.model';
import { ITrip } from './trip.model';

export interface IHistoryData {
    settings: IHistoryDataSettings;
    historyItems: IHistoryItem[];
}

export interface IHistoryDataSettings {
    pagination: IPaginationSettings;
    sort: ITripSortSettings;
    fromDate: string;
    toDate: string;
    searchInProgress: boolean;
}

// History item pozostava z informacii o trase a zariadeni, ktore ich zaznamenalo
export interface IHistoryItem {
    // trip id
    id: string;
    // when trip started
    datetimeStart: string;
    // when trip ended
    datetimeEnd: string;
    // title of the trip - deprecated
    title: string;
    // duration zapisana ako d, h, min, s
    duration: string;
    // distance in m(default), km
    distance: string;
    // zoznam suradnic danej trasy, lazyloaded
    distanceUnit: DistanceUnit;
    
    // flags
    isSelected: boolean;
    isShownOnMap: boolean;
    toDelete: boolean;
    
    device$: Observable<Device>;
    coordinates$: Observable<ICoordinate[]>
}

export interface IHistoryResponse extends IApiResponse {
    paginationHeader: IPaginationHeader;
    navigationLinks: INavigationLinks[];
    items: ITrip[];
}

export interface IHistoryDateTimeInterval {
    dateFrom: Date;
    dateTo: Date;
}

export enum DistanceUnit {
    M = 'm',
    KM = 'km'
}

export enum TripOrderBy {
    START = 'Start',
    END = 'End'
}

export interface ITripSortSettings extends ISortSettings {
    orderBy: TripOrderBy
}

export interface IUpdateHistoryListMessage extends IUpdateMessage {

}

export class HistoryMessages {
    static msgs = {
        'searchInfo': [
            { message: 'Please choose date interval for trips you are interested in.' }
        ],
        'noTrips': [
            { message: 'No trips were found for date interval.' }
        ]
    }
}

export class HistoryValidationErrorMessages {
    static errors = {
        'from': { 
            type: 'error', message: 'Please choose "From" date'
        },
        'to': {
            type: 'error', message: 'Please choose "To" date'
        },
        'less': {
            type: 'error', message: '"From" must be less than "To"'
        }
    }
}