import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent implements OnInit {
  /** Index of the current page (view) of items. */
  private _pageIndex: number = 1;

  /** How many items will be display per one page (view). */
  private _pageSize: number = 5;

  /** Total number of currently available items to diplay. */
  private _totalItemsCount: number = 0;
  
  /**
   * Corrects pagination values (Index, Size, Item Counts) if wrong values were provided.
   */
  private correctPaginationValues() {
    // If inappropriate pagination values were used they are corrected
    if (this._pageIndex < 1) {
      this._pageIndex = 1;
    }

    if (this._pageSize < 1) {
      this._pageSize = 1;
    }

    if (this._totalItemsCount < 0) {
      this._totalItemsCount = 0;
    }
  }

  /**
   * Sets status if there are more items to load.
   */
  private setMoreItemsAvailableFlag() {
    // Checking if last available items are loaded
    this.moreItems = (this._pageIndex * this._pageSize < this._totalItemsCount) ? true : false;
  }

  // Page index -----------------------------------------------------
  /**
   * Page index setter (one-way input bind property).
   */
  @Input()
  set pageIndex(index: number) {
    console.log('[LoaderComponent] Page Index Change');

    this._pageIndex = index;
    this.correctPaginationValues();
    this.setMoreItemsAvailableFlag();  
  }

  /**
   * Page index getter.
   */
  get pageIndex(): number {
    return this._pageIndex;
  }

  /**
   * Page index change emitter (one-way output bind property).
   */
  @Output() pageIndexChange: EventEmitter<number> = new EventEmitter<number>();


  // Page size ------------------------------------------------------
  /**
   * Page size setter (one-way input bind property).
   */
  @Input()
  set pageSize(size: number) {
    console.log('[LoaderComponent] Page Size Change');

    this._pageSize = size;
    this.correctPaginationValues();
    this.setMoreItemsAvailableFlag();
  }

  /**
   * Page size getter.
   */
  get pageSize(): number {
    return this._pageSize;
  }

  /**
   * Page size change emitter (one-way output bind property).
   */
  @Output() pageSizeChange: EventEmitter<number> = new EventEmitter<number>();


  // Total items count ----------------------------------------------
  /**
   * Total items count setter (one-way input bind property).
   */
  @Input()
  set totalItemsCount(count: number) {
    console.log('[LoaderComponent] Total Items Count Change');

    this._totalItemsCount = count;
    this.correctPaginationValues();
    this.setMoreItemsAvailableFlag();
  }

  /**
   * Total items count getter.
   */
  get totalItemsCount(): number {
    return this._totalItemsCount;
  }

  @Input() isLoading: boolean = false;
  @Input() noMoreItemsTitle: string = "No more items";
  @Input() loadMoreItemsText: string = "Load more"

  moreItems: boolean;

  constructor() {}

  ngOnInit() {
  }

  onLoadMore() {
    this.pageIndex++;
    this.pageIndexChange.emit(this.pageIndex);
  }
}
