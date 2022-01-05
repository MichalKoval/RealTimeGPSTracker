import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { UrlQueryService } from './url.service';
import { QueryBaseUrl, QueryAddress, HubAddress } from '../models/url.model';

@Injectable()
export class DashboardService extends BaseService {
    // Query adresa na server pre 'Dashboard' --> '/dashboard'
    constructor(
        private httpClient: HttpClient,
        @Inject('API_BASE_URL') private baseUrl: string,
        
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('dashboard'),
                new HubAddress('')
            )
        );
    }
}