import { createAction, props } from '@ngrx/store';
import { ILoginRequest, ILoginResponse } from 'src/app/core/models/login.model';
import { IRegistrationRequest } from '../../models/registration.model';

export enum AuthActionTypes {
    // Login actions
    LOGIN = '[Login Page] Login',
    LOGIN_SUCCESS = '[Auth API] Login Success',
    LOGIN_FAILURE = '[Auth API] Login Failure',
    UPDATE_TOKEN = '[Auth] Update Token',
    
    // Registration actions
    REGISTRATION = '[Registration Page] Registration',
    REGISTRATION_COMPLETE = '[Registration Page] Registration Complete',
    REGISTRATION_SUCCESS = '[Auth API] Registration Success',
    REGISTRATION_FAILURE = '[Auth API] Registration Failure',

    // Logout actions
    LOGOUT = '[Auth] Confirm Logout',
    LOGOUT_CANCELLED = '[Auth] Logout Cancelled',
    LOGOUT_CONFIRMED = '[Auth] Logout Confirmed',

    // Reset Auth action
    RESET_STATE = '[Auth] Reset State'
}

// Login actions ---------------------------------------------------

export const login = createAction(
    AuthActionTypes.LOGIN,
    props<{loginRequest: ILoginRequest}>()
);

export const loginSuccess = createAction(
    AuthActionTypes.LOGIN_SUCCESS,
    props<{loginResponse: ILoginResponse}>()
);

export const loginFailure = createAction(
    AuthActionTypes.LOGIN_FAILURE,
    props<{errors: string[]}>()
);

export const updateAuthToken = createAction(
    AuthActionTypes.UPDATE_TOKEN,
    props<{accessToken: string}>()
);

// Registration actions --------------------------------------------

export const registration = createAction(
    AuthActionTypes.REGISTRATION,
    props<{ registrationRequest: IRegistrationRequest }>()
);

// export const registrationComplete = createAction(
//     AuthActionTypes.REGISTRATION_COMPLETE    
// );

export const registrationSuccess = createAction(
    AuthActionTypes.REGISTRATION_SUCCESS,
    props<{loginRequest: ILoginRequest}>()
);

export const registrationFailure = createAction(
    AuthActionTypes.REGISTRATION_FAILURE,
    props<{errors: string[]}>()
);

// Logout actions --------------------------------------------------

export const logout = createAction(
    AuthActionTypes.LOGOUT
);

export const logoutCancelled = createAction(
    AuthActionTypes.LOGOUT_CANCELLED
);

export const logoutConfirmed = createAction(
    AuthActionTypes.LOGOUT_CONFIRMED
);

// Reset Auth action -----------------------------------------------

export const resetAuthState = createAction(
    AuthActionTypes.RESET_STATE
);