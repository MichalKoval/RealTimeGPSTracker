import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromStore from 'src/app/core/store/states/state';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  
  constructor(
    private store: Store<fromStore.State>
  ) {
    console.log("Auth component instantiated.");
  }

  ngOnInit() {
  }
}
