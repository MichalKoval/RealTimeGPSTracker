import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromDashboard from 'src/app/core/store/selectors/dashboard.selectors';
import { changeNotificationRealtime, changeNotificationHistory, changeNotificationDevices, setCurrentDashboardTitle } from 'src/app/core/store/actions/dashboard.actions';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['../../app.component.css', './settings.component.scss']
})
export class SettingsComponent implements OnInit {
  private _title: string = 'Settings';

  isRealtimeNotification$: Observable<boolean>;
  isHistoryNotification$: Observable<boolean>;
  isDevicesNotification$: Observable<boolean>;

  constructor(
    private store: Store<fromStore.State>
  ) {
    console.log("Settings component instantiated.");
    this.store.dispatch(setCurrentDashboardTitle({title: this._title }));
    this.isRealtimeNotification$ = this.store.select(fromDashboard.selectDashboardNotificationRealtime);
    this.isHistoryNotification$ = this.store.select(fromDashboard.selectDashboardNotificationHistory);
    this.isDevicesNotification$ = this.store.select(fromDashboard.selectDashboardNotificationDevices);
  }

  ngOnInit() {
  }

  onRealtimeNotificationChange() {
    this.store.dispatch(changeNotificationRealtime())
  }

  onHistoryNotificationChange() {
    this.store.dispatch(changeNotificationHistory())
  }

  onDevicesNotificationChange() {
    this.store.dispatch(changeNotificationDevices())
  }
}
