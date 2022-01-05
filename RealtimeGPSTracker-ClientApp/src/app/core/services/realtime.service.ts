import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../core/services/base.service';
import { QueryBaseUrl, QueryAddress, HubAddress } from '../../core/models/url.model';
import { Observable, Subject } from 'rxjs';
import { UrlQueryService } from '../../core/services/url.service';
import { CoordinateService } from './coordinate.service';
import { DeviceService } from './device.service';

@Injectable()
export class RealtimeService extends BaseService {
    // Query adresa na server pre 'Realtime' --> 'Dashboard/Realtime/'
        
    constructor(
        private httpClient: HttpClient,
        private _coordinateService: CoordinateService,
        private _deviceService: DeviceService,
        @Inject('API_BASE_URL') private baseUrl: string
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('coordinate'),
                new HubAddress('')
            )
        );
    }

    // getRealtimeDevices(queryProperties: QueryProperty[]): Observable<RealtimeData> {
    //     var realtimeDataSubject = new Subject<RealtimeData>();

    //     // Async dotaz, pockame kym nevrati data z databazy
    //     super.getList<CoordinatesData>(queryProperties).subscribe(result => {
    //         console.log(result.body);

    //         var coordinatesCount = result.body.coordinates.length;
            
    //         if (coordinatesCount > 0) {
    //             realtimeDataSubject.next(result.body);
    //         } else {
    //             realtimeDataSubject.next(null);
    //         }       
            
    //     });

    //     return realtimeDataSubject.asObservable();
    // }
}