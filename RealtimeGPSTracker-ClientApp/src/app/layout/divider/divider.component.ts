import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-divider',
  templateUrl: './divider.component.html',
  styleUrls: ['./divider.component.css']
})
export class DividerComponent implements OnInit {
  @Input() width: string = '100px'
  @Input() color: string = '#000000'
  @Input() thickness: string = '1px'
  @Input() align: 'left' | 'right' | 'center';

  constructor() {
    console.log("Divider component instantiated.")
  }

  ngOnInit() {
  }

}
