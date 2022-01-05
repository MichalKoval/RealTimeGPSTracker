import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType, Effect } from '@ngrx/effects';
import { catchError, map, concatMap, mergeMap, exhaustMap, tap } from 'rxjs/operators';
import { of } from 'rxjs';

import * as AuthActions from '../actions/auth.actions';
import * as UserActions from '../actions/user.actions';
import { ILoginRequest, ILoginResponse } from 'src/app/core/models/login.model';
import { LoginService } from 'src/app/core/services/login.service';
import { RegistrationService } from '../../services/registration.service';
import { Router } from '@angular/router';
import { IRegistrationRequest } from '../../models/registration.model';
import { HttpErrorResponse } from '@angular/common/http';


@Injectable()
export class AuthEffects {
    
    constructor(
        private actions$: Actions,
        private loginService: LoginService,
        private registrationService: RegistrationService,
        private router: Router
    ) {}

    // Login effects --------------------------------------------

    login$ = createEffect(() => this.actions$.pipe(
            ofType(AuthActions.login),
            map(action => action.loginRequest),
            exhaustMap((loginRequest: ILoginRequest) => 
                this.loginService.login(loginRequest).pipe(
                    map((loginResponse: ILoginResponse) =>
                        AuthActions.loginSuccess({ loginResponse: loginResponse })
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(AuthActions.loginFailure({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    loginSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(AuthActions.loginSuccess),
            tap(result => {
                localStorage.setItem('access_token', result.loginResponse.accessToken);
                this.router.navigateByUrl('/');
            })    
        ),
        { dispatch: false }
    );

    
    loginFailure$ = createEffect(() => this.actions$.pipe(
            ofType(AuthActions.loginFailure),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Registration effects --------------------------------------------

    register$ = createEffect(() => this.actions$.pipe(
            ofType(AuthActions.registration),
            map(action => action.registrationRequest),
            exhaustMap((registrationRequest: IRegistrationRequest) => 
                this.registrationService.register(registrationRequest).pipe(
                    map(() =>
                        AuthActions.registrationSuccess({
                                loginRequest: { 
                                    userName: registrationRequest.userName,
                                    password: registrationRequest.password
                                }
                            }
                        )
                    ),
                    catchError((errorResponse) =>
                        of(AuthActions.registrationFailure({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    registrationSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(AuthActions.registrationSuccess),
            map(action => action.loginRequest),
            exhaustMap((loginRequest: ILoginRequest) => 
                this.loginService.login(loginRequest).pipe(
                    map((loginResponse: ILoginResponse) =>
                        AuthActions.loginSuccess({ loginResponse: loginResponse })
                    ),
                    catchError((errorResponse) =>
                        of(AuthActions.loginFailure({errors: errorResponse.error.errors}))
                    )
                )
            )
        )
    );

    registrationFailure$ = createEffect(() => this.actions$.pipe(
            ofType(AuthActions.registrationFailure),
            tap((result) => {
                console.log(result.errors)
            })
        ),
        { dispatch: false }
    );

    // Logout effects ---------------------------------------------------
    
    logout$ = createEffect(() => this.actions$.pipe(
            ofType(AuthActions.logout),
            tap(() => {
                localStorage.removeItem('access_token');
                this.router.navigateByUrl('/auth/login');
            })    
        ),
        { dispatch: false }
    );
}
