
import { ActionReducerMap, MetaReducer } from '@ngrx/store';
import { environment } from 'src/environments/environment';

import { AuthState } from './auth.state';
import { CoordinateState } from './coordinate.state';
import { DashboardState } from './dashboard.state';
import { DeviceState } from './device.state';
import { HistoryState } from './history.state';
import { RealtimeState } from './realtime.state';
import { UserState } from './user.state';

import * as authReducer from '../reducers/auth.reducer';
import * as coordinateReducer from '../reducers/coordinate.reducer';
import * as dashboardReducer from '../reducers/dashboard.reducer';
import * as deviceReducer from '../reducers/device.reducer';
import * as historyReducer from '../reducers/history.reducer';
import * as realtimeReducer from '../reducers/realtime.reducer';
import * as userReducer from '../reducers/user.reducer';

// SignalR reducers
import { signalrReducer, BaseSignalRStoreState } from 'ngrx-signalr-core';


export interface State {
    auth: AuthState;
    coordinate: CoordinateState;
    dashboard: DashboardState;
    device: DeviceState;
    history: HistoryState;    
    realtime: RealtimeState;
    user: UserState;
    signalR: BaseSignalRStoreState;
}

export const reducers: ActionReducerMap<State> = {
    auth: authReducer.reducer,
    coordinate: coordinateReducer.reducer,
    dashboard: dashboardReducer.reducer,
    device: deviceReducer.reducer,
    history: historyReducer.reducer,
    realtime: realtimeReducer.reducer,
    user: userReducer.reducer,
    signalR: signalrReducer

};

export const metaReducers: MetaReducer<State>[] = !environment.production ? [] : [];