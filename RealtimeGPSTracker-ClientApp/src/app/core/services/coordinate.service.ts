import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { BaseService } from './base.service';
import { UrlQueryService } from './url.service';
import { QueryBaseUrl, QueryAddress, IQueryProperty, HubAddress } from '../models/url.model';
import { ICoordinate, ICoordinatesDataResponse } from '../models/coordinate.model';

@Injectable()
export class CoordinateService extends BaseService {
    // Query adresa na server pre 'Coordinate' --> '/coordinate'
        
    constructor(
        private httpClient: HttpClient,
        @Inject('API_BASE_URL') private baseUrl: string
    ) {
        super(
            httpClient,
            new UrlQueryService(
                new QueryBaseUrl(baseUrl),
                new QueryAddress('coordinate'),
                new HubAddress('coordinatehub')
            )
        );
    }

    getCoordinates(queryProperties: IQueryProperty[]) : Observable<ICoordinatesDataResponse> {
        // let startDT = this._momentService.convertFromUTCToShortUTC(queryProperties['Start']);
        // let endDT = this._momentService.convertFromUTCToShortUTC(queryProperties['End']);

        // var queryProperties: QueryProperty[] = [
        //     { name: 'DeviceId', value: tripId },
        //     { name: 'TripId', value: tripId },
        //     { name: 'Start', value: startDT },
        //     { name: 'End', value: endDT }
        // ];

        var coordinatesSubject = new Subject<ICoordinatesDataResponse>();

        // Async dotaz, pockame kym nevrati data z databazy
        super.getList<ICoordinatesDataResponse>(queryProperties).subscribe(result => {
                //console.log(result.body);

                var coordinatesCount = result.body.coordinates.length;
                
                if (coordinatesCount > 0) {
                    coordinatesSubject.next(result.body);
                } else {
                    coordinatesSubject.next(null);
                }
                
                
            }
        );

        return coordinatesSubject.asObservable();
    }

    getCoordinateSignalRHub() {
        return super.getSignalRHub('CoordinateHub')
    }

    // Funkcia pre vypocet radianu cisla
    private rad(x: number) {
        return x * Math.PI / 180;
    }

    // Pouzijeme Haversinovu formulu pre vzdialenost dvoch suradnic sfericky
    // Zdroj: https://www.movable-type.co.uk/scripts/latlong.html
    private getDistanceBetweenTwoCoordinates(c1: ICoordinate, c2: ICoordinate): number {
        //Radius zeme, moze sa lisit!!, vzdialenost je len priblizna
        var earthRadius = 6378137;
        //Vypocitame vzdialenost v radianoch medzi lat a lnf dvoch suradnic a preratame taktiez lat suradnice oboch bodov na radiany
        var rLat1 = this.rad(c1.lat);
        var rLat2 = this.rad(c2.lat)
        var distLats = this.rad(c2.lat - c1.lat);
        var distLngs = this.rad(c2.lng - c1.lng);
        var squareOfHalfChordLength = Math.sin(distLats / 2) * Math.sin(distLats / 2) +
                                      Math.cos(rLat1) * Math.cos(rLat2) *
                                      Math.sin(distLngs / 2) * Math.sin(distLngs / 2);
        var angularDistanceInRad = 2 * Math.atan2(Math.sqrt(squareOfHalfChordLength), Math.sqrt(1 - squareOfHalfChordLength));
        var distance = earthRadius * angularDistanceInRad;
        
        // Vzdialenost je v metroch
        return distance;
    }

    computeCoordinatePathDistance(coords: ICoordinate[]): number {
        var pathDistance = 0;
        var coordsCount = coords.length;

        for(var i = 1; i < coordsCount; i++) {
            pathDistance += this.getDistanceBetweenTwoCoordinates(coords[i - 1], coords[i]);
        }

        return pathDistance;
    }
}