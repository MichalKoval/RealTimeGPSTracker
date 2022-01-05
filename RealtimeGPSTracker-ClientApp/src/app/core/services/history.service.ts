import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../core/services/base.service';
import { PaginationService } from '../../core/services/pagination.service';
import { SortService } from '../../core/services/sort.service';
import { UrlQueryService } from '../../core/services/url.service';
import { QueryBaseUrl, QueryAddress, IQueryProperty, HubAddress } from '../../core/models/url.model';
import { Observable, Subject } from 'rxjs';
import { IHistoryResponse } from '../models/history.model';

@Injectable()
export class HistoryService extends BaseService {
    // Query adresa na server pre 'History' alias Trips --> 'Dashboard/History/'
    constructor(
        private httpClient: HttpClient,
        private paginationService: PaginationService,
        private sortService: SortService,
        @Inject('API_BASE_URL')  baseUrl: string
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('dashboard/history'),
                new HubAddress('historyhub')
            ),
            paginationService,
            sortService
        );
    }

    getTrips(queryProperties: IQueryProperty[]) : Observable<IHistoryResponse> {
        // var queryProperties: QueryProperty[] = [
        //     { name: 'Start', value: dateFrom },
        //     { name: 'End', value: dateTo }
        // ];

        var tripsSubject = new Subject<IHistoryResponse>();

        // Async dotaz, pockame kym nevrati data z databazy
        super.getList<IHistoryResponse>(queryProperties).subscribe(result => {
                console.log(result.body);

                tripsSubject.next(result.body);          
            }
        );

        return tripsSubject.asObservable();
    }

    getHistorySignalRHub() {
        return super.getSignalRHub('HistoryHub')
    }
}
