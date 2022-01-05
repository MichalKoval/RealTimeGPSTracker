import { createFeatureSelector, createSelector } from '@ngrx/store';
import { HistoryState, historyFeatureKey } from '../states/history.state';

export const selectHistory = createFeatureSelector<HistoryState>(historyFeatureKey);

export const selectHistoryData = createSelector(
  selectHistory,
  (state: HistoryState) => state.historyData
);

export const selectHistoryDataLoaded = createSelector(
  selectHistory,
  (state: HistoryState) => state.loaded
);

export const selectHistoryDataLoading = createSelector(
  selectHistory,
  (state: HistoryState) => state.loading
);

export const selectHistoryDataError = createSelector(
  selectHistory,
  (state: HistoryState) => state.error
);

export const selectHistoryDataSettings = createSelector(
  selectHistory,
  (state: HistoryState) => state.historyData.settings
);