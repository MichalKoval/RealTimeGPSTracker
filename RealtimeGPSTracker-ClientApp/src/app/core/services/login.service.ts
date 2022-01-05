import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from 'src/app/core/services/base.service';
import { UrlQueryService } from 'src/app/core/services/url.service';
import { QueryBaseUrl, QueryAddress, HubAddress } from 'src/app/core/models/url.model';
import { ILoginRequest, ILoginResponse } from '../models/login.model';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class LoginService extends BaseService {
    // Query adresa na server pre 'Login' --> 'auth/login'
    constructor(
        private httpClient: HttpClient,
        @Inject('API_BASE_URL') private baseUrl: string,
        
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('auth/login'),
                new HubAddress('')
            )
        );
    }

    login(loginRequest: ILoginRequest): Observable<ILoginResponse> {

        return super.post<ILoginResponse>(loginRequest);
    }
}