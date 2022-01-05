import { Component, OnInit, Input, EventEmitter, Output, AfterViewInit, ElementRef } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['../../app.component.css', './footer.component.css']
})
export class FooterComponent implements OnInit, AfterViewInit {
  private _componentHeight: number = 0;
  
  @Input() title: string = '';

  @Input()
  set componentHeight(componentHeight: number) {
    if (this._componentHeight !== componentHeight) {
      this._componentHeight = componentHeight;
      this.componentHeightChange.emit(this._componentHeight);
    }    
  }  

  get componentHeight(): number {
    return this._componentHeight;
  }

  @Output() componentHeightChange: EventEmitter<number> = new EventEmitter<number>();

  constructor(private _elementRef: ElementRef) {
    console.log("Footer component instantiated.");
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.componentHeight = this._elementRef.nativeElement.offsetHeight;
    //console.log('Footer height: ' + this.componentHeight);
  }
}
