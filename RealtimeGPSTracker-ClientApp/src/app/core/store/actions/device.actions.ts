import { createAction, props } from '@ngrx/store';
import { IQueryProperty } from 'src/app/core/models/url.model';
import { IDeviceRequest, Device, IDevicesToDeleteRequest, IDevicesResponse, DeviceStatus, DeviceOrderBy, IDevicesItem, DeviceFilterCriterion } from 'src/app/core/models/device.model';
import { SortOrder } from '../../models/sort.model';
import { Update } from '@ngrx/entity';

export enum DeviceActionTypes {
    // Load devices actions
    LOAD_DEVICES = "[Device] Load Devices",
    LOAD_DEVICES_SUCCESS = "[Device] Load Devices Success",
    LOAD_DEVICES_FAIL = "[Device] Load Devices Fail",
    // Load device actions
    LOAD_DEVICE = "[Device] Load Device",
    LOAD_DEVICE_SUCCESS = "[Device] Load Device Success",
    LOAD_DEVICE_FAIL = "[Device] Load Device Fail",
    // Create device actions
    CREATE_DEVICE = "[Device] Create Device",
    CREATE_DEVICE_SUCCESS = "[Device] Create Device Success",
    CREATE_DEVICE_FAIL = "[Device] Create Device Fail",
    // Update device actions
    UPDATE_DEVICE = "[Device] Update Device",
    UPDATE_DEVICE_SUCCESS = "[Device] Update Device Success",
    UPDATE_DEVICE_FAIL = "[Device] Update Device Fail",
    // Delete device actions
    DELETE_DEVICES = "[Device] Delete Devices",
    DELETE_DEVICES_SUCCESS = "[Device] Delete Devices Success",
    DELETE_DEVICES_FAIL = "[Device] Delete Devices Fail",
    // Change current displayed list of devices based on filter, sort order and pagination
    CHANGE_DEVICES_FILTER_VIEW = "[Device] Change Devices Filter View",
    CHANGE_DEVICES_ORDER_VIEW = "[Device] Change Devices Order View",
    CHANGE_DEVICES_ORDER_BY_VIEW = "[Device] Change Devices OrderBy View",
    CHANGE_DEVICES_PAGE_INDEX_VIEW = "[Device] Change Devices List Size View",
    CHANGE_DEVICES_PAGE_SIZE_VIEW = "[Device] Change Devices List Size View",
    // Change devices item state
    CHANGE_DEVICES_ITEM = "[Device] Change Devices Item",
    // Do nothing devices action
    DO_NOTHING = "[Device] No further action needed at the momemnt"
}

// Load devices actions------------------------------
export const loadDevices = createAction(
    DeviceActionTypes.LOAD_DEVICES//,
    //props<{ payload: IQueryProperty[]}>()
);

export const loadDevicesSuccess = createAction(
    DeviceActionTypes.LOAD_DEVICES_SUCCESS,
    props<{ devicesResponse: IDevicesResponse }>()
);

export const loadDevicesFail = createAction(
    DeviceActionTypes.LOAD_DEVICES_FAIL,
    props<{ errors: string[] }>()
);

// Load device actions------------------------------
export const loadDevice = createAction(
    DeviceActionTypes.LOAD_DEVICE,
    props<{ deviceRequest: IDeviceRequest}>()
);

export const loadDeviceSuccess = createAction(
    DeviceActionTypes.LOAD_DEVICE_SUCCESS,
    props<{ deviceResponse: Device }>()
);

export const loadDeviceFail = createAction(
    DeviceActionTypes.LOAD_DEVICE_FAIL,
    props<{ errors: string[] }>()
);

// Create device actions-----------------------------
export const createDevice = createAction(
    DeviceActionTypes.CREATE_DEVICE,
    props<{ createDeviceRequest: Device }>()
);

export const createDeviceSuccess = createAction(
    DeviceActionTypes.CREATE_DEVICE_SUCCESS    
);

export const createDeviceFail = createAction(
    DeviceActionTypes.CREATE_DEVICE_FAIL,
    props<{ errors: string[] }>()
);

// Update device actions-----------------------------
export const updateDevice = createAction(
    DeviceActionTypes.UPDATE_DEVICE,
    props<{ updateDeviceRequest: Device }>()
);

export const updateDeviceSuccess = createAction(
    DeviceActionTypes.UPDATE_DEVICE_SUCCESS    
);

export const updateDeviceFail = createAction(
    DeviceActionTypes.UPDATE_DEVICE_FAIL,
    props<{ errors: string[] }>()
);

// Delete device actions-----------------------------
export const deleteDevices = createAction(
    DeviceActionTypes.DELETE_DEVICES,
    props<{ devicesToDeleteRequest: IDevicesToDeleteRequest }>()
);

export const deleteDevicesSuccess = createAction(
    DeviceActionTypes.DELETE_DEVICES_SUCCESS    
);

export const deleteDevicesFail = createAction(
    DeviceActionTypes.DELETE_DEVICES_FAIL,
    props<{ errors: string[] }>()
);

export const changeDevicesFilterView = createAction(
    DeviceActionTypes.CHANGE_DEVICES_FILTER_VIEW,
    props<{ deviceFilterCriteria: DeviceFilterCriterion[] }>()
);

export const changeDevicesOrderView = createAction(
    DeviceActionTypes.CHANGE_DEVICES_ORDER_VIEW,
    props<{ deviceOrder: SortOrder }>()
);

export const changeDevicesOrderByView = createAction(
    DeviceActionTypes.CHANGE_DEVICES_ORDER_BY_VIEW,
    props<{ deviceOrderBy: DeviceOrderBy }>()
);

export const changeDevicesPageIndexView = createAction(
    DeviceActionTypes.CHANGE_DEVICES_PAGE_INDEX_VIEW,
    props<{ devicePageIndex: number }>()
);

export const changeDevicesPageSizeView = createAction(
    DeviceActionTypes.CHANGE_DEVICES_PAGE_SIZE_VIEW,
    props<{ devicePageSize: number }>()
);

export const changeDevicesItem = createAction(
    DeviceActionTypes.CHANGE_DEVICES_ITEM,
    props<{ changeDevicesItem: Update<IDevicesItem> }>()
);

export const doNothing = createAction(
    DeviceActionTypes.DO_NOTHING
);






// Doesn't work for TypeScript 3.7.2 and higher (Import declaration conflicts with local declaration of 'Action')
// // Strongly typed actions to load devices (Easier to work with)
// export class LoadDevices implements Action {
//     readonly type = DeviceActionTypes.LOAD_DEVICES
// }

// export class LoadDevicesSuccess implements Action {
//     readonly type = DeviceActionTypes.LOAD_DEVICES_SUCCESS

//     constructor(
//         public payLoad: Device[]
//     ) {}
// }

// export class LoadDevicesFail implements Action {
//     readonly type = DeviceActionTypes.LOAD_DEVICES_FAIL

//     constructor(
//         public payLoad: string
//     ) {}
// }

// // Union all the actions to load devices
// export type Action = LoadDevices | LoadDevicesSuccess | LoadDevicesFail;
