import { Injectable } from '@angular/core';
import { Actions, Effect, ofType, createEffect } from '@ngrx/effects';
import * as RealtimeActions from '../actions/realtime.actions';
import { RealtimeService } from 'src/app/core/services/realtime.service';

@Injectable()
export class RealtimeEffects {
    
    constructor(
        private actions$: Actions,
        private realtimeService: RealtimeService
    ) {}

    
}