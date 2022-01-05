import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit, ElementRef, ChangeDetectionStrategy } from '@angular/core';

/**
 * Component renders dashboard header with dynamically set header title and button to collapse dashboard left sidebar.
 */
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['../../app.component.css', './header.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent implements OnInit, AfterViewInit {
  private _componentHeight: number = 0;
  private _isCollapsed: boolean = false;
  
  @Input() title: string = 'Title';
  
  // Is collapsed ----------------------------------------------------
  /**
   * Is collapsed button setter (one-way input bind property).
   */  
  @Input() 
  set isCollapsed(isCollapsed: boolean) {
    this._isCollapsed = isCollapsed;
  }

  /**
   * Is collapsed buttton getter.
   */
  get isCollapsed(): boolean {
    return this._isCollapsed;
  }

  /**
   * Is collapsed buttton change emitter (one-way output bind property).
   */
  @Output() isCollapsedChange = new EventEmitter<boolean>();
  
  onCollapsedChange() {
    this.isCollapsed = !this.isCollapsed;
    console.log('Trigger was clicked. IsCollpased: ' + String(this.isCollapsed));
    
    //Emit that trigger button was clicked
    this.isCollapsedChange.emit(this.isCollapsed);
  }
  
  // Component height ------------------------------------------------
  /**
   * Component height setter (one-way input bind property).
   */
  @Input()
  set componentHeight(componentHeight: number) {
    this._componentHeight = componentHeight;   
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

  constructor(private _elementRef: ElementRef) {
    console.log("Header component instantiated.");
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.componentHeight = this._elementRef.nativeElement.offsetHeight;
    this.componentHeightChange.emit(this.componentHeight);
    //console.log('Header height: ' + this.componentHeight);
  }
}
