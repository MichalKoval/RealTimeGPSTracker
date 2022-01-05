import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SortOrder } from 'src/app/core/models/sort.model';
import { TripOrderBy } from 'src/app/core/models/trip.model';
import { IHistoryDateTimeInterval } from 'src/app/core/models/history.model';

@Component({
  selector: 'app-history-search',
  templateUrl: './history-search.component.html',
  styleUrls: ['./history-search.component.scss']
})
export class HistorySearchComponent implements OnInit {
  private _order: SortOrder = SortOrder.ASC;
  private _orderBy: TripOrderBy = TripOrderBy.END;

  @Input()
  set selectedOrder(order: SortOrder) {
    if (this._order !== order) {
      this._order = order;
      this.selectedOrderChange.emit(this._order)
    }
  }

  get selectedOrder(): SortOrder {
    return this._order;
  }

  @Output() selectedOrderChange: EventEmitter<SortOrder> = new EventEmitter<SortOrder>();

  @Input()
  set selectedOrderBy(orderBy: TripOrderBy) {
    if (this._orderBy !== orderBy) {
      this._orderBy = orderBy;
      this.selectedOrderByChange.emit(this._orderBy)
    }
  }

  get selectedOrderBy(): TripOrderBy {
    return this._orderBy;
  }

  @Output() selectedOrderByChange: EventEmitter<TripOrderBy> = new EventEmitter<TripOrderBy>();

  @Output() selectedIntervalChange: EventEmitter<IHistoryDateTimeInterval> = new EventEmitter<IHistoryDateTimeInterval>();

  @Output() searchClick: EventEmitter<boolean> = new EventEmitter<boolean>();
  
  dateFrom: Date | null = null;
  dateTo: Date | null = null;

  constructor() { }

  ngOnInit() {
  }

  onOrderButtonClick() {
    this.selectedOrder = (this.selectedOrder === SortOrder.ASC) ? SortOrder.DESC : SortOrder.ASC;
  }

  onSearchHistory() {
    
    this.searchClick.emit();
  }

  onRangeDateOk(rangeDateResult: Date | Date[] | null) {
    if (rangeDateResult instanceof Date) {
      this.dateFrom = rangeDateResult;

    } else if (
      rangeDateResult instanceof Array &&
      rangeDateResult[0] instanceof Date &&
      rangeDateResult[1] instanceof Date
      ) {
        this.dateFrom = rangeDateResult[0];
        this.dateTo = rangeDateResult[1];
        this.selectedIntervalChange.emit({ dateFrom: this.dateFrom, dateTo: this.dateTo});

    } else {
      this.dateFrom = null;
      this.dateTo = null;
    }
  }
}
