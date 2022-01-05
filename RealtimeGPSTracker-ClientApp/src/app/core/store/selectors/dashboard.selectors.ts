import { createFeatureSelector, createSelector } from '@ngrx/store';
import { DashboardState, dashboardFeatureKey } from '../states/dashboard.state';

export const selectDashboard = createFeatureSelector<DashboardState>(dashboardFeatureKey);

export const selectDashboardLeftSidebarCollapsed = createSelector(
  selectDashboard,
  (state: DashboardState) => state.isLeftSidebarCollapsed
);

export const selectDashboardNotificationRealtime = createSelector(
  selectDashboard,
  (state: DashboardState) => state.isRealtimeNotification
);

export const selectDashboardNotificationHistory = createSelector(
  selectDashboard,
  (state: DashboardState) => state.isHistoryNotification
);

export const selectDashboardNotificationDevices = createSelector(
  selectDashboard,
  (state: DashboardState) => state.isDevicesNotification
);

export const selectDashboardCurrentTitle = createSelector(
  selectDashboard,
  (state: DashboardState) => state.currentDashboardTitle
);