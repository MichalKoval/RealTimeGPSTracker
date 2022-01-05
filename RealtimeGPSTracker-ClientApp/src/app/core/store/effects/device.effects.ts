import { Injectable } from '@angular/core';

import { Actions, Effect, ofType, createEffect } from '@ngrx/effects'
import { Action, createAction, Store } from '@ngrx/store';

import { Observable, of, merge } from 'rxjs';
import { map, mergeMap, catchError, withLatestFrom, switchMap, tap } from 'rxjs/operators';

import { SIGNALR_HUB_UNSTARTED, startSignalRHub } from 'ngrx-signalr-core';
import { ofHub, mergeMapHubToAction } from 'ngrx-signalr-core';

import * as DeviceActions from '../actions/device.actions';
import { DeviceService, mapDevice } from 'src/app/core/services/device.service';
import { Device, IDeviceRequest, IDevicesToDeleteRequest, IUpdateDeviceListMessage, IDevicesDataSettings, IDevicesResponse, DeviceFilterCriterion, IDevicesItem } from 'src/app/core/models/device.model';

import * as fromStore from 'src/app/core/store/states/state';
import * as fromDevice from 'src/app/core/store/selectors/device.selectors';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class DeviceEffects {

    constructor(
        private actions$: Actions,
        private deviceService: DeviceService,
        private store: Store<fromStore.State>,
    ) {}

    // Load list of devices based on filter (status of device) and order effects

    loadDevices$ = createEffect(() => this.actions$.pipe(
            // Type of actions to listen to.
            ofType(DeviceActions.loadDevices),
            withLatestFrom(this.store.select(fromDevice.selectDevicesDataSettings)),
            switchMap(([action, devicesDataSettings]: [Action, IDevicesDataSettings]) =>
                this.deviceService.getDevices([
                    { name: 'PageIndex', value: devicesDataSettings.pagination.pageIndex.toString() },
                    { name: 'PageSize', value: devicesDataSettings.pagination.pageSize.toString() },
                    { name: 'Order', value: devicesDataSettings.sort.order },
                    { name: 'OrderBy', value: devicesDataSettings.sort.orderBy }
                ].concat(
                    devicesDataSettings.filter.criteria.map((deviceFilterCriterion: DeviceFilterCriterion) => ({
                        name: deviceFilterCriterion.fieldName,
                        value: deviceFilterCriterion.fieldValue
                    }))
                )).pipe(
                    map((devicesResponse: IDevicesResponse) => 
                            DeviceActions.loadDevicesSuccess({ devicesResponse: devicesResponse })
                        ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(DeviceActions.loadDevicesFail({ errors: errorResponse.error.errors }))
                    )
                )      
            )
        )
    );

    loadDevicesSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.loadDevicesSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );

    loadDevicesFail$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.loadDevicesFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Load device detail effects ---------------------------------------------

    loadDevice$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.loadDevice),
            map(action => action.deviceRequest),
            mergeMap((deviceRequest: IDeviceRequest) => 
                this.deviceService.getDevice(deviceRequest).pipe(
                    map((deviceResponse: Device) =>
                        DeviceActions.loadDeviceSuccess({ deviceResponse: deviceResponse})
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(DeviceActions.loadDeviceFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    loadDeviceSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.loadDeviceSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );

    loadDeviceFail$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.loadDeviceFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Create device effects ---------------------------------------------

    createDevice$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.createDevice),
            map(action => action.createDeviceRequest),
            mergeMap((createDeviceRequest: Device) => 
                this.deviceService.add<Device>(createDeviceRequest).pipe(
                    map(() =>
                        DeviceActions.createDeviceSuccess()
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(DeviceActions.createDeviceFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    createDeviceSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.createDeviceSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );

    createDeviceFail$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.createDeviceFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Update device effects ---------------------------------------------

    updateDevice$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.updateDevice),
            map(action => action.updateDeviceRequest),
            mergeMap((updateDeviceRequest: Device) => 
                this.deviceService.update<Device>(updateDeviceRequest).pipe(
                    map(() =>
                        DeviceActions.updateDeviceSuccess()
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(DeviceActions.updateDeviceFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    updateDeviceSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.updateDeviceSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );

    updateDeviceFail$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.updateDeviceFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    // Delete devices effects ---------------------------------------------

    deleteDevices$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.deleteDevices),
            map(action => action.devicesToDeleteRequest),
            mergeMap((devicesToDeleteRequest: IDevicesToDeleteRequest) =>
                this.deviceService.delete<IDevicesToDeleteRequest>(devicesToDeleteRequest).pipe(
                    map(() =>
                        DeviceActions.deleteDevicesSuccess()
                    ),
                    catchError((errorResponse: HttpErrorResponse) =>
                        of(DeviceActions.deleteDevicesFail({ errors: errorResponse.error.errors }))
                    )
                )
            )
        )
    );

    deleteDevicesSuccess$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.deleteDevicesSuccess),
            tap(result => {
                console.log(result);
            })    
        ),
        { dispatch: false }
    );

    deleteDevicesFail$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.deleteDevicesFail),
            tap((result) => {
                console.log(result.errors);
            })
        ),
        { dispatch: false }
    );

    changeDevicesItem$ = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.changeDevicesItem),
            mergeMap(action => {
                if (!action.changeDevicesItem.changes.isEdited) {
                    if (action.changeDevicesItem.changes.toAdd) {
                        return of(DeviceActions.createDevice({
                            createDeviceRequest: mapDevice(action.changeDevicesItem.changes)
                        }));
                    }
                    else if (action.changeDevicesItem.changes.toUpdate) {
                        return of(DeviceActions.updateDevice({
                            updateDeviceRequest: mapDevice(action.changeDevicesItem.changes)
                        }));
                    }
                    else if (action.changeDevicesItem.changes.toDelete) {
                        return of(DeviceActions.deleteDevices({
                            devicesToDeleteRequest: { deviceIds: [action.changeDevicesItem.changes.id]}
                        }));
                    }
                    else {
                        return of(DeviceActions.doNothing());
                    }
                }
                else {
                    return of(DeviceActions.doNothing());
                }
            })           
        )
    );

    initializeDeviceSignalRHub$ = createEffect(() => this.actions$.pipe(
            ofType(SIGNALR_HUB_UNSTARTED),
            ofHub(this.deviceService.getDeviceSignalRHub()),
            mergeMapHubToAction(({action, hub}) => {
                const whenEvent$ = hub.on('UpdateDeviceList').pipe(
                    map((message: IUpdateDeviceListMessage) => DeviceActions.loadDevices())
                );
     
                return merge(
                    whenEvent$,
                    of(startSignalRHub(hub))
                );
            })
        )
    );

    doNothing = createEffect(() => this.actions$.pipe(
            ofType(DeviceActions.doNothing),
            tap()
        ),
        { dispatch: false }
    );
}