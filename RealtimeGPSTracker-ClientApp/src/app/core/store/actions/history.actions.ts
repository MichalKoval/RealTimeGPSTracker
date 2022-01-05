import { createAction, props } from '@ngrx/store';
import { IQueryProperty } from 'src/app/core/models/url.model';
import { ITripsToDeleteRequest } from 'src/app/core/models/trip.model';
import { IHistoryResponse } from '../../models/history.model';

export enum HistoryActionTypes {
    LOAD_TRIPS = "[Trip] Load Trips",
    LOAD_TRIPS_SUCCESS = "[Trip] Load Trips Success",
    LOAD_TRIPS_FAIL = "[Trip] Load Trips Fail",
    SET_TRIPS_TIME_INTERVAL = "[Trip] Set Trips Time Interval",
    SET_SEARCH_IN_PROGRESS = "[Trip] Set Search In Progress",
    DELETE_TRIPS = "[Trip] Delete Trips",
    DELETE_TRIPS_SUCCESS = "[Trip] Delete Trips Success",
    DELETE_TRIPS_FAIL = "[Trip] Delete Trips Fail",
    // Change history item state
    CHANGE_HISTORY_ITEM = "[Device] Change History Item"
}

export const loadTrips = createAction(
    HistoryActionTypes.LOAD_TRIPS//,
    //props<{ payload: IQueryProperty[] }>()
);

export const loadTripsSuccess = createAction(
    HistoryActionTypes.LOAD_TRIPS_SUCCESS,
    props<{ historyResponse: IHistoryResponse }>()
);

export const setTripsTimeInterval = createAction(
    HistoryActionTypes.SET_TRIPS_TIME_INTERVAL,
    props<{ fromDate: string, toDate: string }>()
);

export const setSearchInProgress = createAction(
    HistoryActionTypes.SET_SEARCH_IN_PROGRESS,
    props<{ searchInProgress: boolean }>()
);

export const loadTripsFail = createAction(
    HistoryActionTypes.LOAD_TRIPS_FAIL,
    props<{ errors: string[] }>()
);

export const deleteTrips = createAction(
    HistoryActionTypes.DELETE_TRIPS,
    props<{ tripsToDeleteRequest: ITripsToDeleteRequest }>()
);

export const deleteTripsSuccess = createAction(
    HistoryActionTypes.DELETE_TRIPS_SUCCESS,
    props<{ tripsToDeleteIds: string[] }>()  
);

export const deleteTripsFail = createAction(
    HistoryActionTypes.DELETE_TRIPS_FAIL,
    props<{ errors: string[] }>()
);