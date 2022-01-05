import { createAction, props } from '@ngrx/store';
import { IDashboardSettings } from 'src/app/core/models/dashboard.model';

export enum DashboardActionTypes {
    SET_LEFT_SIDEBAR_COLLAPSED = "[Dasboard] Set Sidebar Collapsed",
    CHANGE_NOTIFICATION_REALTIME = "[Dashboard] Change Notification Realtime",
    CHANGE_NOTIFICATION_HISTORY = "[Dashboard] Change Notification History",
    CHANGE_NOTIFICATION_DEVICES = "[Dashboard] Change Notification Devices",
    SET_CURRENT_DASHBOARD_TITLE = "[Dasboard] Set Current Dashboard Title"
}

export const setLeftSidebarCollapsed = createAction(
    DashboardActionTypes.SET_LEFT_SIDEBAR_COLLAPSED,
    props<{ isCollapsed: boolean }>()
);


export const changeNotificationRealtime = createAction(
    DashboardActionTypes.CHANGE_NOTIFICATION_REALTIME
);

export const changeNotificationHistory = createAction(
    DashboardActionTypes.CHANGE_NOTIFICATION_HISTORY
);

export const changeNotificationDevices = createAction(
    DashboardActionTypes.CHANGE_NOTIFICATION_DEVICES
);

export const setCurrentDashboardTitle = createAction(
    DashboardActionTypes.SET_CURRENT_DASHBOARD_TITLE,
    props<{ title: string }>()
);