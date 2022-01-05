import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { IHistoryItem } from 'src/app/core/models/history.model';
import { convertFromUTCDatabaseToLocalClient } from 'src/app/core/services/moment.service';

@Component({
  selector: 'app-history-item',
  templateUrl: './history-item.component.html',
  styleUrls: ['./history-item.component.scss']
})
export class HistoryItemComponent implements OnInit {
  private _historyItem: IHistoryItem = null;
  private _isSelected: boolean = false;

  // History item bind property (one-way) -------------------------
  @Input()
  set historyItem(historyItem: IHistoryItem) {
    this._historyItem = historyItem  
  }  

  get historyItem(): IHistoryItem {
    return this._historyItem;
  }

  // History item is selected bind property (two-way) -------------
  @Input()
  set isSelected(isSelected: boolean) {
    if (this._isSelected !== isSelected) {
      this._isSelected = isSelected;
      this.isSelectedChange.emit(this._isSelected);
    }    
  }  

  get isSelected(): boolean {
    return this._isSelected;
  }

  fromUTCDatabaseToLocalClient = convertFromUTCDatabaseToLocalClient;

  @Output() isSelectedChange: EventEmitter<boolean> = new EventEmitter<boolean>();


  constructor() { }

  ngOnInit() {
  }

}
