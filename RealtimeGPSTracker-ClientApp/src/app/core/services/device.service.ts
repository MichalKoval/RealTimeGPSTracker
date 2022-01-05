import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../core/services/base.service';
import { PaginationService } from '../../core/services/pagination.service';
import { SortService } from '../../core/services/sort.service';
import { UrlQueryService } from '../../core/services/url.service';
import { QueryBaseUrl, QueryAddress, IQueryProperty, HubAddress } from '../../core/models/url.model';
import { Device, IDeviceRequest, IDevicesResponse, IDevicesItem } from '../models/device.model';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class DeviceService extends BaseService {
    // Query adresa na server pre 'Device' --> 'dashboard/device/'
    constructor(
        private httpClient: HttpClient,
        private paginationService: PaginationService,
        private sortService: SortService,
        @Inject('API_BASE_URL') private baseUrl: string,
        
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('dashboard/device'),
                new HubAddress('devicehub')
            ),
            paginationService,
            sortService
        );
    }

    getDevice(deviceRequest: IDeviceRequest): Observable<Device> {
        return super.post<Device>(deviceRequest);
    }

    getDevices(queryProperties: IQueryProperty[]) : Observable<IDevicesResponse> {
        // var queryProperties: QueryProperty[] = [
        //     { name: 'Status', value: '0' }
        // ];

        var devicesSubject = new Subject<IDevicesResponse>();

        // Async dotaz, pockame kym nevrati data z databazy
        super.getList<IDevicesResponse>(queryProperties).subscribe(result => {
                console.log(result.body);

                devicesSubject.next(result.body);          
            }
        );

        return devicesSubject.asObservable();
    }

    getOnlineDevices(): Observable<IDevicesResponse> {
        var queryProperties: IQueryProperty[] = [
            { name: 'Status', value: '1' }
        ];

        return this.getDevices(queryProperties);
    }

    getAwayDevices(): Observable<IDevicesResponse> {
        var queryProperties: IQueryProperty[] = [
            { name: 'Status', value: '2' }
        ];

        return this.getDevices(queryProperties);
    }

    getOfflineDevices(): Observable<IDevicesResponse> {
        var queryProperties: IQueryProperty[] = [
            { name: 'Status', value: '0' }
        ];

        return this.getDevices(queryProperties);
    }

    getDeviceSignalRHub() {
        return super.getSignalRHub('DeviceHub')
    }
}

export function mapIDevicesItem(device: Device): IDevicesItem {
    return {
        // Device details
        id: device.id,
        name: device.name,
        createTime: device.createTime,
        color: device.color,
        interval: device.interval,
        status: device.status,

        // Device state flags
        isSelected: false,
        isEdited: false,
        isShownOnMap: false,

        toAdd: false,
        toUpdate: false,
        toDelete: false        
    }
}

// export function mapDevice(devicesItem: IDevicesItem): Device {
//     return {
//         // Device details
//         id: devicesItem.id,
//         name: devicesItem.name,
//         createTime: devicesItem.createTime,
//         color: devicesItem.color,
//         interval: devicesItem.interval,
//         status: devicesItem.status,
//     }
// }

// mapDevice is overloaded function
export function mapDevice(devicesItem: IDevicesItem): Device
export function mapDevice(partialDevicesItem: Partial<IDevicesItem>): Device
export function mapDevice(anyDevicesItem: any): Device {
    return {
        // Device details
        id: anyDevicesItem.id,
        name: anyDevicesItem.name,
        createTime: anyDevicesItem.createTime,
        color: anyDevicesItem.color,
        interval: anyDevicesItem.interval,
        status: anyDevicesItem.status,
    }
}