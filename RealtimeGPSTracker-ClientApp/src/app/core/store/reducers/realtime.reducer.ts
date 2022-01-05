import * as RealtimeActions from '../actions/realtime.actions';
import { Action, createReducer, on } from '@ngrx/store';
import { initialRealtimeState, RealtimeState } from '../states/realtime.state';

// Exporting realtime reducer function, basically it is a switch like statement that returns different states based on an action type.
const realtimeReducer = createReducer(
    initialRealtimeState,
    on(RealtimeActions.loadRealtime, state => ({
        ...state,
        loading: true
    })),
    on(RealtimeActions.loadRealtimeSuccess, (state, { payload } ) => ({
        ...state,
        //realtimeData: payload,
        loading: false,
        loaded: true
    })),
    on(RealtimeActions.loadRealtimeFail, (state, { errors } ) => ({
        ...state,
        //realtimeData: null,
        loading: false,
        loaded: false,
        error: errors
    }))
);

export function reducer(state: RealtimeState | undefined, action: Action) {
    return realtimeReducer(state, action);
}
