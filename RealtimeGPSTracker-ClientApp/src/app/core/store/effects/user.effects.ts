import { Injectable } from '@angular/core';

import { Actions, Effect, ofType, createEffect } from '@ngrx/effects'
import { Action, createAction } from '@ngrx/store';

import { Observable, of, merge } from 'rxjs';
import { map, mergeMap, catchError, tap } from 'rxjs/operators';

import { SIGNALR_HUB_UNSTARTED, startSignalRHub } from 'ngrx-signalr-core';
import { ofHub, mergeMapHubToAction } from 'ngrx-signalr-core';

import * as UserActions from '../actions/user.actions'
import { UserService } from 'src/app/core/services/user.service';
import { IUser, IUpdateUserDetailMessage, IUpdateUserRequest, IDeleteUserRequest, IDeleteUserResponse, IUpdateUserResponse, IChangeUserPasswordResponse } from 'src/app/core/models/user.model';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class UserEffects {

    constructor(
        private actions$: Actions,
        private userService: UserService
    ) {}

    // Load user detail effects -----------------------------------------------

    loadUser$ = createEffect(() => this.actions$.pipe(
            // Type of actions to listen to.
            ofType(UserActions.loadUser),
            mergeMap(action =>
                this.userService.get<IUser>().pipe(
                    map((user: IUser) =>
                        UserActions.loadUserSuccess({user: user})
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(UserActions.loadUserFail({ errors: errorResponse.error.errors }))
                    )
                )        
            )
        )
    );

    loadUserSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.loadUserSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );


    loadUserFail$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.loadUserFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Update user detail effects -----------------------------------------------

    updateUser$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.updateUser),
            map(action => action.updateUserRequest),
            mergeMap((updateUserRequest: IUpdateUserRequest) => 
                this.userService.update<IUpdateUserResponse>(updateUserRequest).pipe(
                    map(() =>
                        UserActions.updateUserSuccess()
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(UserActions.updateUserFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    updateUserSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.updateUserSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );


    updateUserFail$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.updateUserFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Change user password effects -----------------------------------------------

    changeUserPassword$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.changeUserPassword),
            map(action => action.updateUserRequest),
            mergeMap(updateUserRequest =>
                this.userService.update<IChangeUserPasswordResponse>(updateUserRequest).pipe(
                    map(() =>
                        UserActions.changeUserPasswordSuccess()
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(UserActions.changeUserPasswordFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    changeUserPasswordSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.changeUserPasswordSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );


    changeUserPasswordFail$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.changeUserPasswordFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Delete user account effects ------------------------------------------------

    deleteUser$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.deleteUser),
            map(action => action.deleteUserRequest),
            mergeMap(deleteUserRequest =>
                this.userService.delete<IDeleteUserResponse>(deleteUserRequest).pipe(
                    map(() =>
                        UserActions.deleteUserSuccess()
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(UserActions.deleteUserFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    deleteUserSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.deleteUserSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );


    deleteUserFail$ = createEffect(() => this.actions$.pipe(
            ofType(UserActions.deleteUserFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Initialize user signalr hub effects -----------------------------------------------

    initializeUserSignalRHub$ = createEffect(() =>
        this.actions$.pipe(
            ofType(SIGNALR_HUB_UNSTARTED),
            ofHub(this.userService.getUserSignalRHub()),
            mergeMapHubToAction(({action, hub}) => {
                const whenEvent$ = hub.on('UpdateUserDetail').pipe(
                    map((message: IUpdateUserDetailMessage) => UserActions.loadUser())
                );
     
                return merge(
                    whenEvent$,
                    of(startSignalRHub(hub))
                );
            })
        )
    );
}