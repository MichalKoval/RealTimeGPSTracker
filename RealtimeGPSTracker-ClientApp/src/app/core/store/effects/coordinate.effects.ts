import { Injectable } from '@angular/core';

import { Actions, Effect, ofType, createEffect } from '@ngrx/effects'
import { Action, createAction } from '@ngrx/store';

import { Observable, of, merge } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';

import { SIGNALR_HUB_UNSTARTED, startSignalRHub } from 'ngrx-signalr-core';
import { ofHub, mergeMapHubToAction } from 'ngrx-signalr-core';

import * as CoordinateActions from '../actions/coordinate.actions';
import { IQueryProperty } from 'src/app/core/models/url.model';
import { IUpdateCoordinateListMessage, ICoordinatesDataResponse } from 'src/app/core/models/coordinate.model';
import { CoordinateService } from 'src/app/core/services/coordinate.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class CoordinateEffects {

       
    constructor(
        private actions$: Actions,
        private coordinateService: CoordinateService
    ) {}

    loadCoordinates$ = createEffect(() => this.actions$.pipe(
            // Type of actions to listen to.
            ofType(CoordinateActions.loadCoordinates),
            map(action => action.coordinatesDataRequest),
            mergeMap((coordinatesListQueryProperties: IQueryProperty[]) =>
                this.coordinateService.getCoordinates(coordinatesListQueryProperties).pipe(
                    map((coordinatesData: ICoordinatesDataResponse) => 
                        CoordinateActions.loadCoordinatesSuccess({ coordinatesDataResponse: coordinatesData })
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(CoordinateActions.loadCoordinatesFail({ errors: errorResponse.error.errors }))
                    )
                )      
            )
        )
    );
    
    initializeCoordinateSignalRHub$ = createEffect(() => this.actions$.pipe(
            ofType(SIGNALR_HUB_UNSTARTED),
            ofHub(this.coordinateService.getCoordinateSignalRHub()),
            mergeMapHubToAction(({action, hub}) => {
                const whenEvent$ = hub.on('UpdateCoordinateList').pipe(
                    map((message: IUpdateCoordinateListMessage) => CoordinateActions.loadCoordinates({
                        coordinatesDataRequest: [
                            { name: 'TripId', value: message.tripId},
                            { name: 'DeviceId', value: message.deviceId},
                            { name: 'StartDt', value: message.startDt},
                            { name: 'EndDt', value: message.endDt}
                        ]
                    }))
                );

                return merge(
                    whenEvent$,
                    of(startSignalRHub(hub))
                );
            })
        )
    );
}