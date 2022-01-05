import { Injectable } from '@angular/core';

import { Actions, Effect, ofType, createEffect } from '@ngrx/effects'
import { Action, createAction, Store } from '@ngrx/store';

import { of, merge } from 'rxjs';
import { map, mergeMap, catchError, withLatestFrom, switchMap, tap } from 'rxjs/operators';

import { SIGNALR_HUB_UNSTARTED, startSignalRHub } from 'ngrx-signalr-core';
import { ofHub, mergeMapHubToAction } from 'ngrx-signalr-core';

import * as HistoryActions from '../actions/history.actions';
import { IQueryProperty } from 'src/app/core/models/url.model';
import { HistoryService } from 'src/app/core/services/history.service';
import { ITripsToDeleteRequest } from 'src/app/core/models/trip.model';
import { IUpdateHistoryListMessage, IHistoryDataSettings, IHistoryResponse } from '../../models/history.model';

import * as fromStore from 'src/app/core/store/states/state';
import * as fromHistory from 'src/app/core/store/selectors/history.selectors';
import { HttpErrorResponse } from '@angular/common/http';
import { convertFromUTCToShortUTC } from '../../services/moment.service';

@Injectable()
export class HistoryEffects {

    constructor(
        private actions$: Actions,
        private historyService: HistoryService,
        private store: Store<fromStore.State>,
    ) {}

    // Load list of trips based on filter (start and end datetime of trip) and order effects

    loadTrips$ = createEffect(() => this.actions$.pipe(
            // Type of action to listen to.
            ofType(HistoryActions.loadTrips),
            withLatestFrom(this.store.select(fromHistory.selectHistoryDataSettings)),
            switchMap(([action, historyDataSettings]: [Action, IHistoryDataSettings]) =>
                this.historyService.getTrips([
                    { name: 'PageIndex', value: historyDataSettings.pagination.pageIndex.toString() },
                    { name: 'PageSize', value: historyDataSettings.pagination.pageSize.toString() },
                    { name: 'Order', value: historyDataSettings.sort.order },
                    { name: 'OrderBy', value: historyDataSettings.sort.orderBy },
                    { name: 'Start', value: convertFromUTCToShortUTC(historyDataSettings.fromDate) },
                    { name: 'End', value: convertFromUTCToShortUTC(historyDataSettings.toDate) }
                ]).pipe(                    
                    map((historyResponse: IHistoryResponse) =>
                        HistoryActions.loadTripsSuccess({ historyResponse: historyResponse })
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(HistoryActions.loadTripsFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    loadTripsSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(HistoryActions.loadTripsSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );

    loadTripsFail$ = createEffect(() => this.actions$.pipe(
            ofType(HistoryActions.loadTripsFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Delete trips effects ---------------------------------------------

    deleteTrips$ = createEffect(() => this.actions$.pipe(
            ofType(HistoryActions.deleteTrips),
            map(action => action.tripsToDeleteRequest),
            mergeMap((tripsToDeleteRequest: ITripsToDeleteRequest) =>
                this.historyService.delete<ITripsToDeleteRequest>(tripsToDeleteRequest).pipe(
                    map(() => 
                        HistoryActions.deleteTripsSuccess( { tripsToDeleteIds: tripsToDeleteRequest.tripIds })
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(HistoryActions.deleteTripsFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    deleteTripsSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(HistoryActions.deleteTripsSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );

    deleteTripsFail$ = createEffect(() => this.actions$.pipe(
            ofType(HistoryActions.deleteTripsFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    initializeHistorySignalRHub$ = createEffect(() => this.actions$.pipe(
            ofType(SIGNALR_HUB_UNSTARTED),
            ofHub(this.historyService.getHistorySignalRHub()),
            mergeMapHubToAction(({action, hub}) => {
                const whenEvent$ = hub.on('UpdateTripList').pipe(
                    map((message: IUpdateHistoryListMessage) => HistoryActions.loadTrips())
                );
     
                return merge(
                    whenEvent$,
                    of(startSignalRHub(hub))
                );
            })
        )
    );
}