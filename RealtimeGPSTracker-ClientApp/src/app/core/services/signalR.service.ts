import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Observable, Subject } from 'rxjs';
import { UrlQueryService } from './url.service';

@Injectable()
export class SignalRService {

    connectionOnEvent_ServerMessage;
    connectionStartedEvent = new EventEmitter<Boolean>();

    private _baseUrl: string;
    private _hubPath: string;
    private _connectionId: string = null;
    
    protected _hubconnection: HubConnection;
    
    constructor(
        private _urlQueryService: UrlQueryService
    ) {
        this._baseUrl = _urlQueryService.queryUrl.value;
        this._hubPath = _urlQueryService.queryAddress.value;
    }

    protected createConnection(): void {
        this._hubconnection = new HubConnectionBuilder()
            .withUrl(this._baseUrl + this._hubPath)
            .build();
    }

    protected startConnection(): void {
        this._hubconnection
            .start()
            .then(() => {
                console.log('Hub connection succeeded.');
                this.connectionStartedEvent.emit(true);
                
                var connectionIdSubject = new Subject<string>();
                this._hubconnection.invoke('GetConnectionId').then((connectionId: string) => {
                    this._connectionId = connectionId;    
                });
            })
            .catch(error => {
                console.log('Error while connecting to the hub');
            });
    }

    protected onConnectionInit(): void {
        this._hubconnection.on('GetConnectionId', (message: string) => {
            console.log("ConnectionId: " + message);
        });
    }

    public getConnectionId(): string {
        return this._connectionId;
    }
}