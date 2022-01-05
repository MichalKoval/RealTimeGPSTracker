import { IApiRequest } from './request.model';

export interface ITrip {
    id: string;
    datetimeStart: string;
    datetimeEnd: string;
    distance: number;
}

export interface ITripsToDeleteRequest extends IApiRequest {
    tripIds: string[];
}

export enum TripOrderBy {
    START = "Start",
    END = "End"
}