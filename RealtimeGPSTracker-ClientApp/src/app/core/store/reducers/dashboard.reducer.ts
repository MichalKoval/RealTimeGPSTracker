import * as DashboardActions from '../actions/dashboard.actions';
import { Action, createReducer, on } from '@ngrx/store';
import { initialDashboardState, DashboardState } from '../states/dashboard.state';

// Exporting dashboard reducer function, basically it is a switch like statement that returns different states based on an action type.
const dashboardReducer = createReducer(
    initialDashboardState,
    on(DashboardActions.setLeftSidebarCollapsed, (state, { isCollapsed } ) => ({
        ...state,
        isLeftSidebarCollapsed: isCollapsed
    })),
    on(DashboardActions.changeNotificationRealtime, state => ({
        ...state,
        isRealtimeNotification: !state.isRealtimeNotification
    })),
    on(DashboardActions.changeNotificationHistory, state => ({
        ...state,
        isHistoryNotification: !state.isHistoryNotification
    })),
    on(DashboardActions.changeNotificationDevices, state => ({
        ...state,
        isDevicesNotification: !state.isDevicesNotification
    })),
    on(DashboardActions.setCurrentDashboardTitle, (state, { title } ) => ({
        ...state,
        currentDashboardTitle: title
    }))
);

export function reducer(state: DashboardState | undefined, action: Action) {
    return dashboardReducer(state, action);
}