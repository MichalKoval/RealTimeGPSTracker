import { Injectable } from '@angular/core';
import { Actions, Effect, ofType, createEffect } from '@ngrx/effects';
import * as DashboardActions from '../actions/realtime.actions';

@Injectable()
export class DashboardEffects {

    constructor(
        private actions$: Actions,
        //private dashboardService: IDashboardService
    ) {}
}