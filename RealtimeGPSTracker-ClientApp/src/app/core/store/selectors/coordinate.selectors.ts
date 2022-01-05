import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CoordinateState, coordinateFeatureKey } from '../states/coordinate.state';

export const selectCoordinate = createFeatureSelector<CoordinateState>(coordinateFeatureKey);

export const selectCoordinateData = createSelector(
  selectCoordinate,
  (state: CoordinateState) => state.coordinatesData
);