import { throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable, Inject } from "@angular/core";
import { HttpHeaders, HttpClient, HttpResponse } from "@angular/common/http";
import { PaginationService } from "./pagination.service";
import { IQueryProperty } from "../models/url.model";
import { SortService } from './sort.service';
import { UrlQueryService } from './url.service';

@Injectable()
export class BaseService {
    private _headers = new HttpHeaders();
    private _baseUrl: string;
    private _queryAddress: string;
    private _hubAddress: string;
    
    constructor(
        private _httpClient: HttpClient,
        private _urlQueryService: UrlQueryService,
        private _paginationService?: PaginationService,
        private _sortService?: SortService
    ) {
        this._baseUrl = _urlQueryService.queryUrl.value;
        this._queryAddress = _urlQueryService.queryAddress.value;
        this._hubAddress = _urlQueryService.hubAddress.value;
        this._headers.set('Content-Type', 'application/json');
        this._headers.set('Accept', 'application/json');
    }

    // 'Obecne genericke metody pre nacitanie/upravu/zmazanie dat z backendu'

    getList<T>(additionalQueryProperties?: IQueryProperty[]): Observable<HttpResponse<T>> {
        var queryUrl = `${this._baseUrl}/${this._queryAddress}/list?`;

        if ((additionalQueryProperties != null) && additionalQueryProperties.length > 0) {
            additionalQueryProperties.forEach( queryProperty => {
                queryUrl += `${queryProperty.name}=${queryProperty.value}&`
            });

            // Odoberieme posledny '&'. K URL budeme prilepovat mozno aj dalsie Query properties.
            queryUrl = queryUrl.slice(0, -1);
        }

        // Old Aproach when adding pagination and sorting query properties
        // if (this._paginationService) {
        //     if (queryUrl.charAt(queryUrl.length - 1) !== '?') {
        //         queryUrl += '&'
        //     }
            
        //     queryUrl += `PageIndex=${this._paginationService.pageIndex}` +
        //                 `&PageSize=${this._paginationService.pageSize}`;
        // }
        
        // if (this._sortService) {
        //     if (queryUrl.charAt(queryUrl.length - 1) !== '?') {
        //         queryUrl += '&'
        //     }

        //     queryUrl += `Order=${this._sortService.orderDirection}` +
        //                 `&OrderBy=${this._sortService.orderBy}`;
        // }
                    

        console.log("QueryUrl: " + queryUrl);

        return this._httpClient.get<T>(queryUrl, { observe: 'response' });
    }

    get<T>(): Observable<T> {
        const queryUrl = `${this._baseUrl}/${this._queryAddress}`;

        return this._httpClient.get<T>(queryUrl);
    }

    getSignalRHub(hubName: string) {
        return {
            hubName: hubName,
            url: `${this._baseUrl}/${this._hubAddress}`
        }
    }

    post<T>(itemToSet: any): Observable<T> {
        const queryUrl = `${this._baseUrl}/${this._queryAddress}`;

        return this._httpClient.post<T>(queryUrl, itemToSet, { headers: this._headers });
    }

    add<T>(itemToAdd: any) {
        const queryUrl = `${this._baseUrl}/${this._queryAddress}/create`;

        return this._httpClient.post<T>(queryUrl, itemToAdd, { headers: this._headers });
    }

    update<T>(itemToUpdate: any) {
        const queryUrl = `${this._baseUrl}/${this._queryAddress}/update`;

        return this._httpClient.post<T>(queryUrl, itemToUpdate, { headers: this._headers });
    }

    delete<T>(itemToDelete: any) {
        const queryUrl = `${this._baseUrl}/${this._queryAddress}/delete`;

        return this._httpClient.post<T>(queryUrl, itemToDelete);
    }
}