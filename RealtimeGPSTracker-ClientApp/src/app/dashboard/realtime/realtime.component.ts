import { Component, OnInit, ViewChild } from '@angular/core';
import { TransferChange, NzTreeNode, NzTreeNodeOptions, NzTreeComponent } from 'ng-zorro-antd';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromDevice from 'src/app/core/store/selectors/device.selectors';
import * as fromDashboard from 'src/app/core/store/selectors/dashboard.selectors';
import { setCurrentDashboardTitle } from 'src/app/core/store/actions/dashboard.actions';
import { Observable } from 'rxjs';
import { DeviceFilterCriterion, DeviceOrderBy, IDevicesDataSettings, IDevicesItem } from 'src/app/core/models/device.model';
import { changeDevicesFilterView, changeDevicesOrderByView, changeDevicesOrderView, loadDevices } from 'src/app/core/store/actions/device.actions';
import { SortOrder } from 'src/app/core/models/sort.model';

@Component({
  selector: 'app-realtime',
  templateUrl: './realtime.component.html',
  styleUrls: ['../../app.component.css', './realtime.component.scss']
})
export class RealtimeComponent implements OnInit {
  private _title: string = 'Realtime';

  realtimeSearchContainer = {
    active: false,
    disabled: true,
    name: 'Search available devices',
    icon: 'search',
    style: {
      background: '#ffffff',
      border: '0px'
    }
  }

  isLeftSidebarCollapsed$: Observable<boolean>;
  
  devicesDataSettings$: Observable<IDevicesDataSettings>;

  devicesData$: Observable<IDevicesItem[]>;

  devicesDataLoading$: Observable<boolean>;
  devicesDataLoaded$: Observable<boolean>;
  devicesDataError$: Observable<string[]>;

  constructor(
    private store: Store<fromStore.State>
  ) {
    console.log("Realtime component instantiated.");
    this.store.dispatch(setCurrentDashboardTitle({title: this._title }));

    this.isLeftSidebarCollapsed$ = this.store.select(fromDashboard.selectDashboardLeftSidebarCollapsed);

    this.devicesDataSettings$ = this.store.select(fromDevice.selectDevicesDataSettings);

    this.devicesData$ = this.store.select(fromDevice.selectDevicesData);

    this.devicesDataLoading$ = this.store.select(fromDevice.selectDevicesDataLoading);
    this.devicesDataLoaded$ = this.store.select(fromDevice.selectDevicesDataLoaded);
    this.devicesDataError$ = this.store.select(fromDevice.selectDevicesDataError);
  }

  ngOnInit() {

    // Changing which type of devices will be viewed based on filter criteria (status)
    this.store.dispatch(changeDevicesFilterView({ deviceFilterCriteria: [
      DeviceFilterCriterion.STATUS_ONLINE,
      DeviceFilterCriterion.STATUS_AWAY
    ]}));
    
    // Changing which order (Asc, Desc) of devices will be viewed
    this.store.dispatch(changeDevicesOrderView({ deviceOrder: SortOrder.ASC}));

    // Changing which orderBy (Name, CreationTime, Interval or Color) of devices will be viewed
    this.store.dispatch(changeDevicesOrderByView({ deviceOrderBy: DeviceOrderBy.STATUS}));
    
    // Load devices specified by filter (status), order, orderBy, pageIndex and pageSize view
    this.store.dispatch(loadDevices())
  }

  onRealtimeItemSelectedChange() {
    
  }

  @ViewChild('tree', { static: true }) tree!: NzTreeComponent;
  list: NzTreeNodeOptions[] = [
    { key: '1', id: 1, parentid: 0, title: 'Online devices' },
    { key: '2', id: 2, parentid: 1, title: 'device1', isLeaf: true },
    { key: '3', id: 3, parentid: 1, title: 'device4', isLeaf: true },
    { key: '4', id: 4, parentid: 0, title: 'Away devices' },
    { key: '5', id: 5, parentid: 4, title: 'device6', isLeaf: true },
    { key: '6', id: 6, parentid: 4, title: 'device3', isLeaf: true },
  ];
  treeData = this.generateTree(this.list);
  checkedNodeList: NzTreeNode[] = [];

  private generateTree(arr: NzTreeNodeOptions[]): NzTreeNodeOptions[] {
    const tree: NzTreeNodeOptions[] = [];
    // tslint:disable-next-line:no-any
    const mappedArr: any = {};
    let arrElem: NzTreeNodeOptions;
    let mappedElem: NzTreeNodeOptions;

    for (let i = 0, len = arr.length; i < len; i++) {
      arrElem = arr[i];
      mappedArr[arrElem.id] = { ...arrElem };
      mappedArr[arrElem.id].children = [];
    }

    for (const id in mappedArr) {
      if (mappedArr.hasOwnProperty(id)) {
        mappedElem = mappedArr[id];
        if (mappedElem.parentid) {
          mappedArr[mappedElem.parentid].children.push(mappedElem);
        } else {
          tree.push(mappedElem);
        }
      }
    }
    return tree;
  }

  checkBoxChange(node: NzTreeNode, onItemSelect: (item: NzTreeNodeOptions) => void): void {
    if (node.isDisabled) {
      return;
    }
    node.isChecked = !node.isChecked;
    if (node.isChecked) {
      this.checkedNodeList.push(node);
    } else {
      const idx = this.checkedNodeList.indexOf(node);
      if (idx !== -1) {
        this.checkedNodeList.splice(idx, 1);
      }
    }
    const item = this.list.find(w => w.id === node.origin.id);
    onItemSelect(item!);
  }

  change(ret: TransferChange): void {
    const isDisabled = ret.to === 'right';
    this.checkedNodeList.forEach(node => {
      node.isDisabled = isDisabled;
      node.isChecked = isDisabled;
    });
  }

}
