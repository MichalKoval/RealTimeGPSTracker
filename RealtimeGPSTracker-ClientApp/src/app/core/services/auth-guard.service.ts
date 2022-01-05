import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromAuth from 'src/app/core/store/selectors/auth.selectors';
import { of, Observable } from 'rxjs';
import { mergeMap, map, take, first } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import * as MomentService from 'src/app/core/services/moment.service'
import { JwtService } from './jwt.service';
import { updateAuthToken } from '../store/actions/auth.actions';

@Injectable()
export class AuthorizationGuard implements CanActivate {
    constructor(
        private router: Router,
        private store: Store<fromStore.State>,
        private jwtService: JwtService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.checkStoreAuthentication().pipe(
            mergeMap(storeAuthentication => {
                // If user is logged in than return true. If not try to check by calling backend rest API.
                return (storeAuthentication) ? of(true) : this.checkLocalStorageAuthentication();
            }),
            map(storeOrLocalStorageAuth => {
                if (!storeOrLocalStorageAuth) {
                    
                    // If user is still not logged in than it is redirected to login page.
                    this.router.navigate(
                        ['/auth/login']//,
                        //{ queryParams: { returnUrl: state.url }}
                    );
                    return false;
                }

                return true;
            })
        )
    }

    checkStoreAuthentication(): Observable<boolean> {
        return this.store.select(fromAuth.selectIsAuthenticated).pipe(first());
    }

    checkLocalStorageAuthentication(): Observable<boolean> {
        return this.store.select(fromAuth.selectAccessToken).pipe(
            mergeMap(storeStateAccessToken => {
              
                let localStorageAccessToken = localStorage.getItem('access_token');

                if (localStorageAccessToken) {
                    // Getting jwt token from previous session, and checking if it's not expired
                
                    let decodedJwtToken = this.jwtService.decodeToken(localStorageAccessToken);
                
                    let jwtTokenIsExpired = MomentService.isExpiredDateTimeInSeconds(Number(decodedJwtToken.exp)) 

                    if (!jwtTokenIsExpired) {
                        
                        // Updating value of access token in app state (store)
                        if (!storeStateAccessToken) {
                            this.store.dispatch(updateAuthToken({ accessToken: localStorageAccessToken }))
                        }

                        return of(true);
                    }
                    
                    return of(false);

                }
                
                return of(false);                
            })
        );
    }
}