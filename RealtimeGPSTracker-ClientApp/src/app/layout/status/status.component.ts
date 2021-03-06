import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.css']
})
export class StatusComponent implements OnInit {
  @Input() type: 'active' | 'away' | 'offline';

  constructor() { }

  ngOnInit() {
  }

}
