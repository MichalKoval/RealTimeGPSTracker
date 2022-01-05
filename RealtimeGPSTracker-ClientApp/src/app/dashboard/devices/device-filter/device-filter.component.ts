import { Component, OnInit, Input, Output, EventEmitter, ElementRef, AfterViewInit, AfterViewChecked, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { DeviceStatus, DeviceOrderBy, DeviceFilterCriterion } from 'src/app/core/models/device.model';
import { SortOrder } from 'src/app/core/models/sort.model';

/**
 * Component renders device filter by which devices in the list are sorted (viewed).
 */
@Component({
  selector: 'app-device-filter',
  templateUrl: './device-filter.component.html',
  styleUrls: ['./device-filter.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeviceFilterComponent implements OnInit {
  private _componentHeight: number = 0;
  private _filterCriteria: DeviceFilterCriterion[];
  private _order: SortOrder = SortOrder.ASC;
  private _orderBy: DeviceOrderBy = DeviceOrderBy.CREATETIME;
  private _deviceCounts: { [deviceStatus: number]: number; } = {
    [DeviceStatus.OFFLINE]: 0,
    [DeviceStatus.ONLINE]: 0,
    [DeviceStatus.AWAY]: 0,
    [DeviceStatus.ALL]: 0,
  }


  // Device counts ----------------------------------------------------
  /**
   * Device counts setter (one-way input bind property).
   */
  @Input()
  set deviceCounts(deviceCounts: { [deviceStatus: number]: number; }) {
    //console.log('[DeviceFilterComponent] Devices Counts Change');
    
    this._deviceCounts = deviceCounts;
  }

  /**
   * Device counts getter.
   */
  get deviceCounts(): { [deviceStatus: number]: number; } {
    return this._deviceCounts;
  }


  // Selected filter criteria --------------------------------------------------
  /**
   * Selected device 'filter criteria view' setter (one-way input bind property).
   */
  @Input()
  set selectedFilterCriteria(filterCriteria: DeviceFilterCriterion[]) {
    //console.log('[DeviceFilterComponent] Selected Filter Change');
    
    this._filterCriteria = filterCriteria;
  }

  /**
   * Selected devices 'filter criteria view' getter.
   */
  get selectedFilterCriteria(): DeviceFilterCriterion[] {
    return this._filterCriteria;
  }

  /**
   * Selected devices 'filter criteria view' change emitter (one-way output bind property).
   */
  @Output() selectedFilterCriteriaChange: EventEmitter<DeviceFilterCriterion[]> = new EventEmitter<DeviceFilterCriterion[]>();


  // Selected order ---------------------------------------------------
  /**
   * Selected devices 'order view' setter (one-way input bind property).
   */
  @Input()
  set selectedOrder(order: SortOrder) {
    //console.log('[DeviceFilterComponent] Selected Order Change');
    
    this._order = order;
  }

  /**
   * Selected devices 'order view' getter.
   */
  get selectedOrder(): SortOrder {
    return this._order;
  }

  /**
   * Selected devices 'order view' change emitter (one-way output bind property).
   */
  @Output() selectedOrderChange: EventEmitter<SortOrder> = new EventEmitter<SortOrder>();


  // Selected order by -----------------------------------------------
  /**
   * Selected devices 'order by view' setter (one-way input bind property).
   */
  @Input()
  set selectedOrderBy(orderBy: DeviceOrderBy) {
    //console.log('[DeviceFilterComponent] Selected Order By Change');
    
    this._orderBy = orderBy;
  }

  /**
   * Selected devices 'order by view' getter.
   */
  get selectedOrderBy(): DeviceOrderBy {
    return this._orderBy;
  }

  /**
   * Selected devices 'order by view' change emitter (one-way output bind property).
   */
  @Output() selectedOrderByChange: EventEmitter<DeviceOrderBy> = new EventEmitter<DeviceOrderBy>();
  

  // Component height ------------------------------------------------
  /**
   * Component height setter (one-way input bind property).
   */
  @Input()
  set componentHeight(componentHeight: number) {
    //console.log('[DeviceFilterComponent] Component Height Change');
    
    if (this._componentHeight !== componentHeight) {
      this._componentHeight = componentHeight;
      this.componentHeightChange.emit(this._componentHeight);
    }    
  }  

  /**
   * Component height getter.
   */
  get componentHeight(): number {
    return this._componentHeight;
  }

  /**
   * Component height change emitter (one-way output bind property).
   */
  @Output() componentHeightChange: EventEmitter<number> = new EventEmitter<number>();

  /**
   * Settings for device 'filter criteria view' filter.
   */
  // deviceStatusButtons: IDeviceStatusButton[] = [
  //   {
  //     deviceStatusType: DeviceStatus.OFFLINE,
  //     active: true,
  //     showZero: true,
  //     name: 'Offline',
  //     count: 0,
  //     style: {
  //       backgroundColor: '#fff',
  //       color: '#ec0000',
  //       boxShadow: '0 0 0 1px #ec0000 inset'
  //     }
  //   },
  //   {
  //     deviceStatusType: DeviceStatus.ONLINE,
  //     active: false,
  //     showZero: true,
  //     name: 'Online',
  //     count: 0,
  //     style: {
  //       backgroundColor: '#fff',
  //       color: '#00a00d',
  //       boxShadow: '0 0 0 1px #00a00d inset'
  //     }
  //   },
  //   {
  //     deviceStatusType: DeviceStatus.AWAY,
  //     active: true,
  //     showZero: true,
  //     name: 'Away',
  //     count: 0,
  //     style: {
  //       backgroundColor: '#fff',
  //       color: '#faad14',
  //       boxShadow: '0 0 0 1px #faad14 inset'
  //     }
  //   },
  //   {
  //     deviceStatusType: DeviceStatus.ALL,
  //     active: true,
  //     showZero: true,
  //     name: 'All',
  //     count: 0,
  //     style: {
  //       backgroundColor: '#fff',
  //       color: '#999',
  //       boxShadow: '0 0 0 1px #d9d9d9 inset'
  //     }
  //   },
  // ];

  /**
   * Settings for device 'filter criteria view' filter.
   */
  filterTagOptions: DeviceFilterCriterion[] = [
    DeviceFilterCriterion.STATUS_OFFLINE,
    DeviceFilterCriterion.STATUS_ONLINE,
    DeviceFilterCriterion.STATUS_AWAY
  ]

  constructor() { }
  
  ngOnInit() {
  }

  /**
   * Changes device 'filter criteria view' to newly selected one.
   * @param deviceFilter New device 'filter criteria' to change to.
   */
  onSelectedFilterCriteria(selectedFilterCriteria: DeviceFilterCriterion[]) {
    this.selectedFilterCriteria = selectedFilterCriteria;
    this.selectedFilterCriteriaChange.emit(this.selectedFilterCriteria);
  }

  /**
   * Changes device 'order view' to newly selected one (From Asc to Desc and vice versa).
   */
  onSelectedOrderChange() {
    this.selectedOrder = (this.selectedOrder === SortOrder.ASC) ? SortOrder.DESC : SortOrder.ASC;
    this.selectedOrderChange.emit(this.selectedOrder);
  }

  /**
   * Changes device 'order by view' to newly selected one.
   * @param selectedOrderBy New device 'order by' to change to.
   */
  onSelectedOrderByChange(selectedOrderBy: DeviceOrderBy) {
    this.selectedOrderBy = selectedOrderBy;
    this.selectedOrderByChange.emit(this.selectedOrderBy);
  }
}
