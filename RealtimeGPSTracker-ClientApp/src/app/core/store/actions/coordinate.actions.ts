import { createAction, props } from '@ngrx/store';
import { IQueryProperty } from 'src/app/core/models/url.model';
import { ICoordinatesDataResponse } from 'src/app/core/models/coordinate.model';

export enum CoordinateActionTypes {
    LOAD_COORDINATES = "[Coordinate] Load Coordinates",
    LOAD_COORDINATES_SUCCESS = "[Coordinate] Load Coordinates Success",
    LOAD_COORDINATES_FAIL = "[Coordinate] Load Coordinates Fail"
}

export const loadCoordinates = createAction(
    CoordinateActionTypes.LOAD_COORDINATES,
    props<{ coordinatesDataRequest: IQueryProperty[] }>()
);

export const loadCoordinatesSuccess = createAction(
    CoordinateActionTypes.LOAD_COORDINATES_SUCCESS,
    props<{ coordinatesDataResponse: ICoordinatesDataResponse }>()
);

export const loadCoordinatesFail = createAction(
    CoordinateActionTypes.LOAD_COORDINATES_FAIL,
    props<{ errors: string[] }>()
);