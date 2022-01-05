import { createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromDevice from 'src/app/core/store/states/device.state';
import { DeviceStatus } from '../../models/device.model';

export const selectDeviceState = createFeatureSelector<fromDevice.DeviceState>(fromDevice.deviceFeatureKey);

export const selectDevicesData = createSelector(
    selectDeviceState,
    fromDevice.selectAll
);

// export const selectOnlineDevicesData = createSelector(
//     selectDevice,
//     (state: fromDevice.DeviceState) => state.devicesData.devices[DeviceStatus.ONLINE]
// );

// export const selectOfflineDevicesData = createSelector(
//     selectDevice,
//     (state: fromDevice.DeviceState) => state.devicesData.devices[DeviceStatus.OFFLINE]
// );

// export const selectAwayDevicesData = createSelector(
//     selectDevice,
//     (state: fromDevice.DeviceState) => state.devicesData.devices[DeviceStatus.AWAY]
// );

export const selectDevicesDataLoaded = createSelector(
    selectDeviceState,
    (state: fromDevice.DeviceState) => state.loaded
  );
  
  export const selectDevicesDataLoading = createSelector(
    selectDeviceState,
    (state: fromDevice.DeviceState) => state.loading
  );
  
  export const selectDevicesDataError = createSelector(
    selectDeviceState,
    (state: fromDevice.DeviceState) => state.error
  );

export const selectDevicesDataSettings = createSelector(
    selectDeviceState,
    (state: fromDevice.DeviceState) => state.settings
);

//TODO: finish all selectors