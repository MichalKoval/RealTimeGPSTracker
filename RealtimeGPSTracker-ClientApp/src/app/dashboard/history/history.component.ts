import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';
import * as fromHistory from 'src/app/core/store/selectors/history.selectors';
import * as fromDashboard from 'src/app/core/store/selectors/dashboard.selectors';
import { setCurrentDashboardTitle } from 'src/app/core/store/actions/dashboard.actions';
import { Observable } from 'rxjs';
import { IHistoryData, HistoryValidationErrorMessages, IHistoryDataSettings, IHistoryDateTimeInterval } from 'src/app/core/models/history.model';
import { loadTrips, setTripsTimeInterval, setSearchInProgress } from 'src/app/core/store/actions/history.actions';
import { NzMessageService } from 'ng-zorro-antd';
import { DatePipe } from '@angular/common';
import { SortOrder } from 'src/app/core/models/sort.model';
import { TripOrderBy } from 'src/app/core/models/trip.model';
import { convertFromLocalClientToUTC, compareTwoDates } from 'src/app/core/services/moment.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['../../app.component.css', './history.component.scss']
})
export class HistoryComponent implements OnInit {
  private _title: string = 'History';
  
  endOpen = false;

  historySearchContainer = {
    active: false,
    disabled: true,
    name: 'Search trips',
    icon: 'search',
    style: {
      background: '#ffffff',
      border: '0px'
    }
  }

  isLeftSidebarCollapsed$: Observable<boolean>;

  historyData$: Observable<IHistoryData>;
  historyDataSettings$: Observable<IHistoryDataSettings>;
  historyDataIsLoading$ : Observable<boolean>;
  historyDataLoaded$ : Observable<boolean>;
  historyDataError$ : Observable<string[]>;

  constructor(
    private store: Store<fromStore.State>,
    private message: NzMessageService,
    private datePipe: DatePipe
  ) {
    console.log("History component instantiated.");
    this.store.dispatch(setCurrentDashboardTitle({title: this._title }));

    this.isLeftSidebarCollapsed$ = this.store.select(fromDashboard.selectDashboardLeftSidebarCollapsed);

    this.historyData$ = this.store.select(fromHistory.selectHistoryData);
    this.historyDataSettings$ = this.store.select(fromHistory.selectHistoryDataSettings);
    this.historyDataIsLoading$ = this.store.select(fromHistory.selectHistoryDataLoading);
    this.historyDataLoaded$ = this.store.select(fromHistory.selectHistoryDataLoaded);
    this.historyDataError$ = this.store.select(fromHistory.selectHistoryDataError);
  }

  ngOnInit() {
    this.store.dispatch(setSearchInProgress({ searchInProgress: false }))    
  }

  onSelectedOrderChange(order: SortOrder) {

  }

  onSelectedOrderByChange(orderBy: TripOrderBy) {

  }

  onHistoryItemSelectedChange() {
    
  }

  onSearchHistory() {
    this.store.dispatch(setSearchInProgress({ searchInProgress: true }));    
    this.store.dispatch(loadTrips());
  }

  onIntervalChange(dateTimeIterval: IHistoryDateTimeInterval) {
    if (dateTimeIterval.dateFrom && dateTimeIterval.dateTo) {
      
      // Converting datetime from local zone to UTC
      let dateFromStr = convertFromLocalClientToUTC(this.datePipe.transform(dateTimeIterval.dateFrom, 'MMM dd yyyy HH:mm:ss'));
      let dateToStr = convertFromLocalClientToUTC(this.datePipe.transform(dateTimeIterval.dateTo, 'MMM dd yyyy HH:mm:ss'));

      if (compareTwoDates(dateFromStr, dateToStr)) {
        this.store.dispatch(setTripsTimeInterval({ fromDate: dateFromStr, toDate: dateToStr }));
      
      } else {
        
        
        this.message.create(
          HistoryValidationErrorMessages.errors.less.type,
          HistoryValidationErrorMessages.errors.less.message
        );
      }

    } else if (!dateTimeIterval.dateFrom) {
      this.message.create(
        HistoryValidationErrorMessages.errors.from.type,
        HistoryValidationErrorMessages.errors.from.message
      );
    } else if (!dateTimeIterval.dateTo) {
      this.message.create(
        HistoryValidationErrorMessages.errors.to.type,
        HistoryValidationErrorMessages.errors.to.message
      );
    } else {
      this.message.create(
        'error',
        'Error'
      );
    }
  }
}
