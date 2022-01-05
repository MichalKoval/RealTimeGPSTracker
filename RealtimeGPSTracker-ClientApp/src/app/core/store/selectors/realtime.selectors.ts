import { createFeatureSelector, createSelector } from '@ngrx/store';
import { RealtimeState, realtimeFeatureKey } from '../states/realtime.state';

export const selectRealtime = createFeatureSelector<RealtimeState>(realtimeFeatureKey);

export const selectRealtimeData = createSelector(
    selectRealtime,
    (state: RealtimeState) => state.realtimeData
);

export const selectRealtimeOnlineDevices = createSelector(
    selectRealtime,
    (state: RealtimeState) => state.realtimeData.onlineDevices
);

export const selectRealtimeAwayDevices = createSelector(
    selectRealtime,
    (state: RealtimeState) => state.realtimeData.awayDevices
);