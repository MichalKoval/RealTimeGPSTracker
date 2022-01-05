import * as AuthActions from '../actions/auth.actions';
import { Action, createReducer, on } from '@ngrx/store';
import { initialAuthState, AuthState } from '../states/auth.state';

// Exporting auth reducer function, basically it is a switch like statement that returns different states based on an action type.
const authReducer = createReducer(
    initialAuthState,
    on(AuthActions.login, state => ({
        ...state,
        loading: true,        
        loaded: false,
        error: null 
    })),
    on(AuthActions.loginSuccess, (state, { loginResponse } ) => ({
        ...state,
        accessToken: loginResponse.accessToken,
        isAuthenticated: true,
        loading: false,
        loaded: true
    })),
    on(AuthActions.loginFailure, (state, { errors } ) => ({
        ...state,
        accessToken: null,
        isAuthenticated: false,
        loading: false,
        loaded: false,
        error: errors
    })),
    on(AuthActions.updateAuthToken, (state, { accessToken } ) => ({
        ...state,
        accessToken: accessToken,
        isAuthenticated: true,
        loading: false,
        loaded: true
    })),
    on(AuthActions.registration, state => ({
        ...state,
        isRegistrationSuccessful: false,
        loading: true,        
        loaded: false,
        error: null        
    })),
    on(AuthActions.registrationSuccess, (state ) => ({
        ...state,
        isRegistrationSuccessful: true,
        loading: false,
        loaded: true
    })),
    on(AuthActions.registrationFailure, (state, { errors } ) => ({
        ...state,
        isRegistrationSuccessful: false,
        loading: false,
        loaded: false,
        error: errors
    })),
    on(AuthActions.logout, state => ({
        ...state,
        isAuthenticated: false,
        isRegistrationSuccessful: false,
        accessToken: null,
        loading: false,
        loaded: true,
        error: null
        //loading: true
    })),
    on(AuthActions.logoutConfirmed, state => ({
        ...state,
        isAuthenticated: false,
        isRegistrationSuccessful: false,
        accessToken: null,
        loading: false,
        loaded: true
    })),
    on(AuthActions.logoutCancelled, state => ({
        ...state,
        loading: false,
        loaded: false
    })),
    on(AuthActions.resetAuthState, state => ({
        ...state,
        isAuthenticated: false,
        isRegistrationSuccessful: false,
        accessToken: null,
        loading: false,
        loaded: false,
        error: null
        //loading: true
    }))
);

export function reducer(state: AuthState | undefined, action: Action) {
    return authReducer(state, action);
}