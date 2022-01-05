import { Component, OnInit, Input } from '@angular/core';
import { ISidebarMenuItem } from '../core/models/layout.model';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromDashboard from 'src/app/core/store/selectors/dashboard.selectors';
import * as fromUser from 'src/app/core/store/selectors/user.selectors';
import { setLeftSidebarCollapsed } from '../core/store/actions/dashboard.actions';
import { IUser } from '../core/models/user.model';
import { loadUser } from '../core/store/actions/user.actions';
import { logout } from '../core/store/actions/auth.actions';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['../app.component.css', './dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  private _isLeftSidebarCollapsed: boolean = false;

  @Input()
  set isCollapsed(isCollapsed: boolean) {
    this._isLeftSidebarCollapsed = isCollapsed;

    // Setting state that left sidebar collapsed
    this.store.dispatch(setLeftSidebarCollapsed({ isCollapsed: isCollapsed}));
  }

  get isCollapsed(): boolean {
    return this._isLeftSidebarCollapsed;
  }

  isRealtimeNotification$: Observable<boolean>;
  isHistoryNotification$: Observable<boolean>;
  isDevicesNotification$: Observable<boolean>;

  sidebarMenuTitle: string = 'Dashboard';
  sidebarMenuItems: ISidebarMenuItem[] = [
    { routerLink: 'realtime', title: 'Realtime', iconType: 'environment' },
    { routerLink: 'history', title: 'History', iconType: 'history' },
    { routerLink: 'devices', title: 'Devices', iconType: 'control' }
  ];

  headerHeight: number = 0;
  footerHeight: number = 0;

  headerTitle$: Observable<string>;
  footerTitle: string = 'Realtime GPS Tracker. Created by Michal Koval.';
  user$: Observable<IUser>;

  
  constructor(
    private store: Store<fromStore.State>
  ) {
    console.log("Dashboard component instantiated.");
    this.headerTitle$ = this.store.select(fromDashboard.selectDashboardCurrentTitle);

    this.user$ = this.store.select(fromUser.selectUserDetail);
  }

  ngOnInit() {
    this.store.dispatch(loadUser());
  }

  onLogoutButtonPressed(event: Event) {
    this.store.dispatch(logout())
  }
}
