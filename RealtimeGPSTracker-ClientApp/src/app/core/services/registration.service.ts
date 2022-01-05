import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { UrlQueryService } from './url.service';
import { QueryBaseUrl, QueryAddress, HubAddress } from '../models/url.model';
import { IRegistrationRequest, IRegistrationResponse } from '../models/registration.model';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class RegistrationService extends BaseService {
    // Query adresa na server pre 'Registration' --> 'auth/register'
    constructor(
        private httpClient: HttpClient,
        @Inject('API_BASE_URL') private baseUrl: string,
        
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('auth/register'),
                new HubAddress('')
            )
        );

        console.log("Registration service instantiated.");
    }

    register(registrationRequest: IRegistrationRequest): Observable<IRegistrationResponse> {

        return super.post<IRegistrationResponse>(registrationRequest);
    }
}