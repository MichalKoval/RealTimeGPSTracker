import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthState, authFeatureKey } from '../states/auth.state';

export const selectAuth = createFeatureSelector<AuthState>(authFeatureKey);

export const selectIsAuthenticated = createSelector(
  selectAuth,
  (state: AuthState) => state.isAuthenticated
);

export const selectAccessToken = createSelector(
  selectAuth,
  (state: AuthState) => state.accessToken
);

export const selectAuthLoading = createSelector(
  selectAuth,
  (state: AuthState) => state.loading
);

export const selectAuthLoaded = createSelector(
  selectAuth,
  (state: AuthState) => state.loaded
);

export const selectAuthError = createSelector(
  selectAuth,
  (state: AuthState) => state.error
);
