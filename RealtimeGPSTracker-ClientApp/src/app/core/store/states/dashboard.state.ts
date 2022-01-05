import * as fromStore from './state';

// Dashboard feature key
export const dashboardFeatureKey: string = 'dashboard';

// Dashboard state
export interface DashboardState {
    isLeftSidebarCollapsed: boolean;
    isRealtimeNotification: boolean;
    isHistoryNotification: boolean;
    isDevicesNotification: boolean;
    currentDashboardTitle: string
    // loading: boolean;
    // loaded: boolean;
    // error: string[];
}

// Extending app root state with dashboard state
// export interface State extends fromStore.State {
//     dashboardState: DashboardState
// }

// Initial dashboard state with sidebar collapse state in it
export const initialDashboardState: DashboardState = {
    isLeftSidebarCollapsed: false,
    isRealtimeNotification: false,
    isHistoryNotification: false,
    isDevicesNotification: false,
    currentDashboardTitle: 'Title'

    // loading: false,
    // loaded: false,
    // error: null
}