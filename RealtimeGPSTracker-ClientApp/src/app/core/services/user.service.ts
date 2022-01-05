import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../core/services/base.service';
import { UrlQueryService } from '../../core/services/url.service';
import { QueryBaseUrl, QueryAddress, HubAddress } from '../../core/models/url.model';

@Injectable()
export class UserService extends BaseService {
    // Query adresa na server pre 'User' --> 'dashboard/user/'
    constructor(
        private httpClient: HttpClient,
        @Inject('API_BASE_URL') private baseUrl: string,
        
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('dashboard/user'),
                new HubAddress('userhub')
            )
        );
    }

    getUserSignalRHub() {
        return super.getSignalRHub('UserHub')
    }
}