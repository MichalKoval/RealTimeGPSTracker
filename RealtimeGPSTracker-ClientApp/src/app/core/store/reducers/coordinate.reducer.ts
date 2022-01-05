import * as CoordinateActions from '../actions/coordinate.actions';
import { Action, createReducer, on } from '@ngrx/store';
import { initialCoordinateState, CoordinateState } from '../states/coordinate.state';
import { compareTwoDateTimes } from '../../services/moment.service';

// Exporting coordinate reducer function, basically it is a switch like statement that returns different states based on an action type.
const coordinateReducer = createReducer(
    initialCoordinateState,
    on(CoordinateActions.loadCoordinates, state => ({
        ...state,
        loading: true
    })),
    on(CoordinateActions.loadCoordinatesSuccess, (state, { coordinatesDataResponse } ) => ({
        ...state,
        coordinatesData: {
            ...state.coordinatesData,
            coordinates: {
                ...state.coordinatesData.coordinates,
                [coordinatesDataResponse.deviceId]: {
                    ...state.coordinatesData.coordinates[coordinatesDataResponse.deviceId],
                    [coordinatesDataResponse.tripId]: {
                        ...state.coordinatesData.coordinates[coordinatesDataResponse.deviceId][coordinatesDataResponse.tripId],
                        // If start Datetime for coordinates do not exist a new trip start Datetime will be set.
                        startDt: (compareTwoDateTimes(
                            state
                            .coordinatesData
                            .coordinates[coordinatesDataResponse.deviceId][coordinatesDataResponse.tripId]
                            .startDt,
                            coordinatesDataResponse.coordinates[0].time
                        ) ) ? 
                            state
                            .coordinatesData
                            .coordinates[coordinatesDataResponse.deviceId][coordinatesDataResponse.tripId]
                            .startDt
                        : coordinatesDataResponse.coordinates[0].time,
                        // End Datetime will be update with latest Datetime (when coordinates data still come from a device).
                        endDt: coordinatesDataResponse.coordinates[coordinatesDataResponse.coordinates.length - 1].time,
                        // If coordinates for a trip do exist then new coordinates will be appended to current ones.
                        coordinates:
                            state
                            .coordinatesData
                            .coordinates[coordinatesDataResponse.deviceId][coordinatesDataResponse.tripId]
                            .coordinates.concat(coordinatesDataResponse.coordinates)
                    }
                }
            }
        },
        loading: false,
        loaded: true
    })),
    on(CoordinateActions.loadCoordinatesFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    }))
);

export function reducer(state: CoordinateState | undefined, action: Action) {
    return coordinateReducer(state, action);
}
