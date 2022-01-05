import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-error-500',
  templateUrl: './error-500.component.html',
  styleUrls: ['./error-500.component.css']
})
export class Error500Component implements OnInit {

  constructor() {
    console.log("Error500 component instantiated.");
  }

  ngOnInit() {
  }

}
