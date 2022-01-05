import { createFeatureSelector, createSelector } from '@ngrx/store';
import { UserState, userFeatureKey } from '../states/user.state';
import { State } from '../states/state';

export const selectUser = createFeatureSelector<UserState>(userFeatureKey);

export const selectUserDetail = createSelector(
    selectUser,
    (state: UserState) => state.user
);

export const selectUserDetailLoaded = createSelector(
    selectUser,
    (state: UserState) => state.loaded
);

export const selectUserDetailLoading = createSelector(
    selectUser,
    (state: UserState) => state.loading
);

export const selectUserDetailError = createSelector(
    selectUser,
    (state: UserState) => state.error
);