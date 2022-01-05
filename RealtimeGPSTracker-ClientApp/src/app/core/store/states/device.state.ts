import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { DeviceStatus, DeviceOrderBy, IDevicesItem, IDevicesDataSettings } from 'src/app/core/models/device.model';
import { SortOrder } from '../../models/sort.model';
import * as DeviceTestData from '../data/device.test.data'

export const useTestData: boolean = false;

// Device feature key
export const deviceFeatureKey: string = 'device';

// Devices state extending Ngrx EntityState
export interface DeviceState extends EntityState<IDevicesItem> {
    settings: IDevicesDataSettings | null;
    selectedDevices: string[];
    loading: boolean;
    loaded: boolean;
    error: string[] | null;
}

export const deviceAdapter: EntityAdapter<IDevicesItem> = createEntityAdapter<IDevicesItem>();

// Initial device state with no devices in it
export const initialDeviceState: DeviceState = deviceAdapter.getInitialState({
    selectedDevices: [],
    settings: {
        // Devices pagination settings
        pagination: {
            pageIndex: 1,
            pageSize: 5,
            totalItemsCount: 0,
            totalPages: 1
        },
        // Devices filter settings
        filter: {
            criteria: []
        },
        // Devices sort settings
        sort: {
            order: SortOrder.DESC,
            orderBy: DeviceOrderBy.CREATETIME
        },
        count: { 
            // Offline devices count setting
            //0: 0,
            [DeviceStatus.OFFLINE]: (useTestData) ? DeviceTestData.deviceCounts[DeviceStatus.OFFLINE] : 0,
            // Online devices count setting
            //1: 0,
            [DeviceStatus.ONLINE]: (useTestData) ? DeviceTestData.deviceCounts[DeviceStatus.ONLINE] : 0,
            // Away devices count setting
            //2: 0,
            [DeviceStatus.AWAY]: (useTestData) ? DeviceTestData.deviceCounts[DeviceStatus.AWAY] : 0,
            // All devices count setting
            //4: 0
            [DeviceStatus.ALL]: (useTestData) ? DeviceTestData.deviceCounts[DeviceStatus.ALL] : 0
        },
        // Default will be online, away and offline devices all together
        selectedStatus: DeviceStatus.ONLINE
    },
    loading: false,
    loaded: (useTestData) ? true : false,
    error: null
});

export const {
    selectAll,
    selectEntities,
    selectIds,
    selectTotal
} = deviceAdapter.getSelectors();