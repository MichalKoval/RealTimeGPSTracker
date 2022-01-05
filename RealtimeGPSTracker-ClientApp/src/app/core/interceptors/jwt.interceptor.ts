import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromAuth from 'src/app/core/store/selectors/auth.selectors';
import { take, mergeMap, map, first, flatMap } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(
        private store: Store<fromStore.State>
    ) { }

    // Adds JWT token to the authorization header when user is logged in so data can be retrieved from backend.
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return this.store.select(fromAuth.selectAccessToken).pipe(
            first(),
            flatMap(accessToken => {
                const authorizedRequest = !!accessToken ? request.clone({
                    setHeaders: { Authorization: `Bearer ${accessToken}` }
                }) : request;
                return next.handle(authorizedRequest);
            }),
        );
    }
}