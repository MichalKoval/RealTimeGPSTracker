import { createAction, props } from '@ngrx/store';
import { IUser, IUpdateUserRequest, IDeleteUserRequest } from 'src/app/core/models/user.model';

export enum UserActionTypes {
    // Load user actions
    LOAD_USER = "[User] Load User",
    LOAD_USER_SUCCESS = "[User] Load User Success",
    LOAD_USER_FAIL = "[User] Load User Fail",
    // Update user actions
    UPDATE_USER = "[User] Update User",
    UPDATE_USER_SUCCESS = "[User] Update User Success",
    UPDATE_USER_FAIL = "[User] Update User Fail",
    // Change user password actions
    CHANGE_USER_PASSWORD = "[User] Change User Password",
    CHANGE_USER_PASSWORD_SUCCESS = "[User] Change User Password Success",
    CHANGE_USER_PASSWORD_FAIL = "[User] Change User Password Fail",
    // Delete user actions
    DELETE_USER = "[User] Delete User",
    DELETE_USER_SUCCESS = "[User] Delete User Success",
    DELETE_USER_FAIL = "[User] Update User Fail"
}

// Load user actions--------------------------------
export const loadUser = createAction(
    UserActionTypes.LOAD_USER
);

export const loadUserSuccess = createAction(
    UserActionTypes.LOAD_USER_SUCCESS,
    props<{ user: IUser }>()
);

export const loadUserFail = createAction(
    UserActionTypes.LOAD_USER_FAIL,
    props<{ errors: string[]}>()
);

// Update user actions------------------------------
export const updateUser = createAction(
    UserActionTypes.UPDATE_USER,
    props<{ updateUserRequest: IUpdateUserRequest }>()
);

export const updateUserSuccess = createAction(
    UserActionTypes.UPDATE_USER_SUCCESS
);

export const updateUserFail = createAction(
    UserActionTypes.UPDATE_USER_FAIL,
    props<{ errors: string[]}>()
);

// Change user password actions------------------------------
export const changeUserPassword = createAction(
    UserActionTypes.CHANGE_USER_PASSWORD,
    props<{ updateUserRequest: IUpdateUserRequest }>()
);

export const changeUserPasswordSuccess = createAction(
    UserActionTypes.CHANGE_USER_PASSWORD_SUCCESS
);

export const changeUserPasswordFail = createAction(
    UserActionTypes.CHANGE_USER_PASSWORD_FAIL,
    props<{ errors: string[]}>()
);

// Delete user actions------------------------------
export const deleteUser = createAction(
    UserActionTypes.DELETE_USER,
    props<{ deleteUserRequest: IDeleteUserRequest}>()
);

export const deleteUserSuccess = createAction(
    UserActionTypes.DELETE_USER_SUCCESS
);

export const deleteUserFail = createAction(
    UserActionTypes.DELETE_USER_FAIL,
    props<{ errors: string[]}>()
);