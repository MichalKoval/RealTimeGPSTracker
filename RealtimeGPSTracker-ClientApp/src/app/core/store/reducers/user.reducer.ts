import * as UserActions from '../actions/user.actions';
import { Action, createReducer, on } from '@ngrx/store';
import { initialUserState, UserState } from '../states/user.state';

// Exporting user reducer function, basically it is a switch like statement that returns different states based on an action type.
const userReducer = createReducer(
    initialUserState,
    // Load user ------------------------------------------------------
    on(UserActions.loadUser, state => ({
        ...state,
        loaded: false,
        loading: true
    })),
    on(UserActions.loadUserSuccess, (state, { user: payload } ) => ({
        ...state,
        user: payload,
        loading: false,
        loaded: true
    })),
    on(UserActions.loadUserFail, (state, { errors } ) => ({
        ...state,
        user: null,
        loading: false,
        loaded: false,
        error: errors
    })),
    // Update user ----------------------------------------------------
    on(UserActions.updateUser, state => ({
        ...state,
        loaded: false,
        loading: true
    })),
    on(UserActions.updateUserSuccess, state => ({
        ...state,
        loading: false,
        loaded: true
    })),
    on(UserActions.updateUserFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    })),
    // Change user password--------------------------------------------
    on(UserActions.changeUserPassword, state => ({
        ...state,
        loaded: false,
        loading: true
    })),
    on(UserActions.changeUserPasswordSuccess, state => ({
        ...state,
        loading: false,
        loaded: true
    })),
    on(UserActions.changeUserPasswordFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    })),
    // Delete user ----------------------------------------------------
    on(UserActions.deleteUser, state => ({
        ...state,
        loaded: false,
        loading: true
    })),
    on(UserActions.deleteUserSuccess, state => ({
        ...state,
        user: null,
        loading: false,
        loaded: true
    })),
    on(UserActions.deleteUserFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    }))
);

export function reducer(state: UserState | undefined, action: Action) {
    return userReducer(state, action);
}