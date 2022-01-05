import { Component, OnInit, EventEmitter } from '@angular/core';
import * as moment from 'moment';
import { v4 as uuid } from 'uuid';

import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromDevice from 'src/app/core/store/selectors/device.selectors';
import { setCurrentDashboardTitle } from 'src/app/core/store/actions/dashboard.actions';
import { IDevicesItem, IDevicesDataSettings, DeviceStatus, DeviceOrderBy, DeviceFilterCriterion } from 'src/app/core/models/device.model';
import { Observable } from 'rxjs';
import { loadDevices, changeDevicesOrderView, changeDevicesOrderByView, changeDevicesPageSizeView, changeDevicesPageIndexView, changeDevicesFilterView, changeDevicesItem } from 'src/app/core/store/actions/device.actions';
import { SortOrder } from 'src/app/core/models/sort.model';
import { mapDevice, mapIDevicesItem } from 'src/app/core/services/device.service';
import { randomHexColor } from 'src/app/core/services/color.service';

interface ItemData {
  id: string;
  name: string;
  createTime: string;
  color: string;
  interval: number;
  status: number;
  checked: boolean;
  expand: boolean;
  disabled?: boolean;
}

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['../../app.component.css', './devices.component.css', './devices.component.scss']
})
export class DevicesComponent implements OnInit {
  private _title: string = 'Devices';

  emptyDevicesItem: IDevicesItem = null;

  deviceOrderDirectionOptions: [
    'Asc',
    'Desc'
  ]

  deviceOrderByOptions: [
    'Name',
    'CreateTime',
    'Color',
    'Interval'
  ]

  devicesDataSettings$: Observable<IDevicesDataSettings>;

  devicesData$: Observable<IDevicesItem[]>;

  devicesDataLoading$: Observable<boolean>;
  devicesDataLoaded$: Observable<boolean>;
  devicesDataError$: Observable<string[]>;

  showAddDeviceContainer: boolean = false;

  selectedDevicesCount: number = 0;

  mapDevice = mapDevice;
  mapIDevicesItem = mapIDevicesItem;

  headerHeight: number = 104;
  footerHeight: number = 55;
  deviceFilterHeight: number = 95;

  deviceListContainerHeight: number = this.headerHeight + this.footerHeight + this.deviceFilterHeight;

  constructor(
    private store: Store<fromStore.State>
  ) {
    console.log("Devices component instantiated.");
    this.store.dispatch(setCurrentDashboardTitle({title: this._title }));

    this.devicesDataSettings$ = this.store.select(fromDevice.selectDevicesDataSettings);

    this.devicesData$ = this.store.select(fromDevice.selectDevicesData);

    this.devicesDataLoading$ = this.store.select(fromDevice.selectDevicesDataLoading);
    this.devicesDataLoaded$ = this.store.select(fromDevice.selectDevicesDataLoaded);
    this.devicesDataError$ = this.store.select(fromDevice.selectDevicesDataError);
  }

  //TODO: refresh content
  refreshData(): void {

  }

  ngOnInit(): void {
    // Changing which type of devices will be viewed based on filter criteria (status)
    this.store.dispatch(changeDevicesFilterView({ deviceFilterCriteria: []}));
    
    // Changing which order (Asc, Desc) of devices will be viewed
    this.store.dispatch(changeDevicesOrderView({ deviceOrder: SortOrder.DESC}));

    // Changing which orderBy (Name, CreationTime, Interval or Color) of devices will be viewed
    this.store.dispatch(changeDevicesOrderByView({ deviceOrderBy: DeviceOrderBy.CREATETIME}));

    this.store.dispatch(loadDevices());
  }

  onSelectedFilterCriteriaChange(deviceFilterCriteria: DeviceFilterCriterion[]) {
    // Changing which type of devices will be viewed based on filter criteria (status)
    this.store.dispatch(changeDevicesFilterView({ deviceFilterCriteria: deviceFilterCriteria }));

    // Load devices specified by filter (status), order, orderBy, pageIndex and pageSize view
    this.store.dispatch(loadDevices())
  }

  onSelectedOrderChange(selectedOrder: SortOrder) {
    // Changing which order (Asc, Desc) of devices will be viewed
    this.store.dispatch(changeDevicesOrderView({ deviceOrder: selectedOrder}));

    // Load devices specified by filter (status), order, orderBy, pageIndex and pageSize view
    this.store.dispatch(loadDevices())
  }

  onSelectedOrderByChange(selectedOrderBy: DeviceOrderBy) {
    // Changing which orderBy (Name, CreationTime, Interval or Color) of devices will be viewed
    this.store.dispatch(changeDevicesOrderByView({ deviceOrderBy: selectedOrderBy}));

    // Load devices specified by filter (status), order, orderBy, pageIndex and pageSize view
    this.store.dispatch(loadDevices())
  }

  onPageIndexChange(changedIndex: number) {
    // Changing pageIndex of devices list that will be viewed
    this.store.dispatch(changeDevicesPageIndexView({ devicePageIndex: changedIndex }));
  }

  onPageSizeChange(changedSize: number) {
    // Changing pageSize of devices list that will be viewed
    this.store.dispatch(changeDevicesPageSizeView({ devicePageSize: changedSize }));
  }

  onLoadMoreDevices() {
    // Load devices specified by filter (status), order, orderBy, pageIndex and pageSize view
    this.store.dispatch(loadDevices())
  }

  onAddDeviceButtonClick() {
    // Before adding values from 'add new device container' will be reset
    this.emptyDevicesItem = {
      id: '',
      name: 'New_Device',
      createTime: '',
      interval: 500,
      color: randomHexColor(),
      status: DeviceStatus.OFFLINE,
      isSelected: false,
      isEdited: true,
      isShownOnMap: false,
      // Add flag is set --> New device will be added in store and on the server
      toAdd: true,
      toUpdate: false,
      toDelete: false
    }

    this.showAddDeviceContainer = !this.showAddDeviceContainer;
  }

  onAddDeviceCancelClick() {
    this.showAddDeviceContainer = !this.showAddDeviceContainer;    
  }

  onDevicesItemChange(changedDevicesItem: IDevicesItem) {
    // If adding of a new device was canceled
    if (changedDevicesItem.toAdd && !changedDevicesItem.isEdited) {
      this.showAddDeviceContainer = !this.showAddDeviceContainer;
    }
    
    // Changing devices item details or state
    this.store.dispatch(changeDevicesItem({
      changeDevicesItem: {
        id: changedDevicesItem.id,
        changes: changedDevicesItem
      }
    }));
  }
}
