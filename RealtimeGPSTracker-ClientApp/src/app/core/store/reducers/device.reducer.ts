import * as DeviceActions from '../actions/device.actions';
import { Action, createReducer, on } from '@ngrx/store';
import { initialDeviceState, DeviceState, deviceAdapter } from '../states/device.state';
import { DeviceStatus, Device } from '../../models/device.model';
import { mapIDevicesItem } from '../../services/device.service';

// Exporting device reducer function, basically it is a switch like statement that returns different states based on an action type.
const deviceReducer = createReducer(
    initialDeviceState,
    /// ============================================================================
    // Load devices which have all, online, offline or away status
    on(DeviceActions.loadDevices, state => ({
        ...state,
        loading: true
    })),
    on(DeviceActions.loadDevicesSuccess, (state, { devicesResponse } ) =>
        deviceAdapter.setAll(
            // Puts newly loaded devices to store
            devicesResponse.items.map((device: Device) => (mapIDevicesItem(device))),
            // Sets devices pagination and count settings
            {
                ...state,
                settings: {
                    ...state.settings,
                    pagination: {
                        ...state.settings.pagination,
                        pageIndex: devicesResponse.paginationHeader.pageIndex,
                        pageSize: devicesResponse.paginationHeader.pageSize,
                        totalItemsCount: devicesResponse.paginationHeader.totalItemsCount,
                        totalPages: devicesResponse.paginationHeader.totalPages
                    },
                    count: {
                        ...state.settings.count,
                        [DeviceStatus.OFFLINE]: Number(devicesResponse.deviceCounts.offlineCount),
                        [DeviceStatus.ONLINE]: Number(devicesResponse.deviceCounts.onlineCount),
                        [DeviceStatus.AWAY]: Number(devicesResponse.deviceCounts.awayCount),
                        [DeviceStatus.ALL]: Number(devicesResponse.deviceCounts.allCount)
                    }
                },
                loading: false,
                loaded: true
            }
        )
    ),
    on(DeviceActions.loadDevicesFail, (state, { errors } ) => ({
        ...state,
        // TODO:

        loading: false,
        loaded: false,
        error: errors
    })),
    // Load device detail ---------------------------------------------
    on(DeviceActions.loadDevice, state => ({
        ...state,
        loading: true
    })),
    on(DeviceActions.loadDeviceSuccess, (state, { deviceResponse } ) => ({
        ...state,
        // devicesData: {
        //     ...state.devicesData,
        //     devices: {
        //         ...state.devicesData.devices,
        //         [state.devicesData.settings.selectedStatus]: devicesResponse.map((device: Device) => ({
        //             device: device,
        //             isSelected: false,
        //             isEdited: false,
        //             isShownOnMap: false,
        //             toAdd: false,
        //             toUpdate: false,
        //             toDelete: false
        //         }))
        //     },
        // },
        loading: false,
        loaded: true
    })),
    on(DeviceActions.loadDeviceFail, (state, { errors } ) => ({
        ...state,
        // devicesData: {
        //     ...state.devicesData,
        //     devices: {
        //         ...state.devicesData.devices,
        //         [state.devicesData.settings.selectedStatus]: null
        //     } 
        // },
        loading: false,
        loaded: false,
        error: errors
    })),
    // Create device --------------------------------------------------
    on(DeviceActions.createDevice, state => ({
        ...state,
        loading: true
    })),
    on(DeviceActions.createDeviceSuccess, state => ({
        ...state,
        loading: false,
        loaded: true
    })),
    on(DeviceActions.createDeviceFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    })),
    // Update device details ------------------------------------------
    on(DeviceActions.updateDevice, state => ({
        ...state,
        loading: true
    })),
    on(DeviceActions.updateDeviceSuccess, state => ({
        ...state,
        loading: false,
        loaded: true
    })),
    on(DeviceActions.updateDeviceFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    })),
    // Delete specified devices ---------------------------------------
    on(DeviceActions.deleteDevices, state => ({
        ...state,
        loading: true
    })),
    on(DeviceActions.deleteDevicesSuccess, state => ({
        ...state,
        // devicesData: {
        //     ...state.devicesData,
        //     devices: {
        //         ...state.devicesData.devices,
        //         [state.devicesData.settings.selectedStatus]: devicesResponse.map((device: Device) => ({
        //             device: device,
        //             isSelected: false,
        //             isEdited: false,
        //             isShownOnMap: false,
        //             toAdd: false,
        //             toUpdate: false,
        //             toDelete: false
        //         }))
        //     },
        // },
        loading: false,
        loaded: true
    })),
    on(DeviceActions.deleteDevicesFail, (state, { errors } ) => ({
        ...state,
        loading: false,
        loaded: false,
        error: errors
    })),
    // Change devices filter view -------------------------------------
    on(DeviceActions.changeDevicesFilterView, (state, { deviceFilterCriteria } ) => ({
        ...state,
        settings: {
            ...state.settings,
            filter: {
                ...state.settings.filter,
                criteria: deviceFilterCriteria
            }
        }
    })),
    // Change devices page index view -------------------------------------
    on(DeviceActions.changeDevicesPageIndexView, (state, { devicePageIndex } ) => ({
        ...state,
        settings: {
            ...state.settings,
            pagination: {
                ...state.settings.pagination,
                pageIndex: devicePageIndex
            }
        }
        // devicesData: {
        //     ...state.devicesData,
        //     settings: {
        //         ...state.devicesData.settings,
        //         pagination: {
        //             ...state.devicesData.settings.pagination,
        //             [state.devicesData.settings.selectedStatus]: {
        //                 ...state.devicesData.settings.pagination[state.devicesData.settings.selectedStatus],
        //                 pageIndex: devicePageIndex
        //             }
        //         }
        //     }
        // }
    })),
    // Change devices page size view -------------------------------------
    on(DeviceActions.changeDevicesPageSizeView, (state, { devicePageSize } ) => ({
        ...state,
        settings: {
            ...state.settings,
            pagination: {
                ...state.settings.pagination,
                pageSize: devicePageSize
            }
        }
        // devicesData: {
        //     ...state.devicesData,
        //     settings: {
        //         ...state.devicesData.settings,
        //         pagination: {
        //             ...state.devicesData.settings.pagination,
        //             [state.devicesData.settings.selectedStatus]: {
        //                 ...state.devicesData.settings.pagination[state.devicesData.settings.selectedStatus],
        //                 pageSize: devicePageSize
        //             }
        //         }
        //     }
        // }
    })),
    // Change devices order view -------------------------------------
    on(DeviceActions.changeDevicesOrderView, (state, { deviceOrder } ) => ({
        ...state,
        settings: {
            ...state.settings,
            sort: {
                ...state.settings.sort,
                order: deviceOrder
            }
        }
    })),
    // Change devices order by view -------------------------------------
    on(DeviceActions.changeDevicesOrderByView, (state, { deviceOrderBy } ) => ({
        ...state,
        settings: {
            ...state.settings,
            sort: {
                ...state.settings.sort,
                orderBy: deviceOrderBy
            }
        }
    })),
    // Change devices item details or state------------------------------
    on(DeviceActions.changeDevicesItem, (state, { changeDevicesItem: updateDevicesItem } ) =>
        deviceAdapter.updateOne(updateDevicesItem, state)
    )
);

export function reducer(state: DeviceState | undefined, action: Action) {
    return deviceReducer(state, action);
}