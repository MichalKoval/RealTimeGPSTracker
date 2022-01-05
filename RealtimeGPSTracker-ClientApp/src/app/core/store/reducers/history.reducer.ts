import * as HistoryActions from '../actions/history.actions';
import { createReducer, on, Action } from '@ngrx/store';
import { initialHistoryState, HistoryState } from '../states/history.state';
import { ITrip } from '../../models/trip.model';
import { durationBetweenDateTimes } from '../../services/moment.service';

// Exporting history reducer function, basically it is a switch like statement that returns different states based on an action type.
const historyReducer = createReducer(
    initialHistoryState,
    // Load list of trips ---------------------------------------------
    on(HistoryActions.loadTrips, state => ({
        ...state,
        loading: true
    })),
    on(HistoryActions.loadTripsSuccess, (state, { historyResponse } ) => ({
        ...state,
        historyData: {
            ...state.historyData,
            historyItems: historyResponse.items.map((trip: ITrip) => ({
                // trip id
                id: trip.id,
                // when trip started
                datetimeStart: trip.datetimeStart,
                // when trip ended
                datetimeEnd: trip.datetimeEnd,
                // title of the trip - deprecated
                title: "",
                // duration in seconds. Can be viewed as d, h, min or s
                duration: durationBetweenDateTimes(trip.datetimeStart, trip.datetimeEnd),
                // distance in meters, Can be viewed as m or km
                distance: trip.distance.toString(),
                
                // flags
                isSelected: false,
                isShownOnMap: false,
                toDelete: false,
                
                device$: null,
                coordinates$: null
            }))
        },
        loading: false,
        loaded: true
    })),
    on(HistoryActions.loadTripsFail, (state, { errors } ) => ({
        ...state,
        historyData: {
            ...state.historyData,
            historyItems: null
        },
        loading: false,
        loaded: false,
        error: errors
    })),
    on(HistoryActions.setTripsTimeInterval, (state, { fromDate, toDate } ) => ({
        ...state,
        historyData: {
            ...state.historyData,
            settings: {
                ...state.historyData.settings,
                fromDate: fromDate,
                toDate: toDate
            }
        }
    })),
    on(HistoryActions.setSearchInProgress, (state, { searchInProgress } ) => ({
        ...state,
        historyData: {
            ...state.historyData,
            settings: {
                ...state.historyData.settings,
                searchInProgress: searchInProgress
            }
        }
    })),
    // Delete specified trips ---------------------------------------------
    on(HistoryActions.deleteTrips, state => ({
        ...state,
        loading: true
    })),
    on(HistoryActions.deleteTripsSuccess, state => ({
        ...state,
        // historyData: {
        //     ...state.historyData,
        //     historyItems: 
        // },
        loading: false,
        loaded: true
    })),
    on(HistoryActions.deleteTripsFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    }))
);

export function reducer(state: HistoryState | undefined, action: Action) {
    return historyReducer(state, action);
}