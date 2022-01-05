import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ISidebarMenuItem } from 'src/app/core/models/layout.model';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['../../app.component.css', './sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  private _isCollapsed: boolean;
  @Input() userName: string = 'Name'
  @Input() userDescription: string = 'Welcome'
  @Input() userAvatarSrc: string;
  @Input() menuTitle: string = "List"
  @Input() menuItems: ISidebarMenuItem[];
  
  @Input()
  set darkMode(isDarkMode: boolean) {
    this.sidebarTheme = isDarkMode ? 'dark' : 'light'
  }

  @Output() isCollapsedChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() isLogoutButtonPressed: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input()
  set isCollapsed(collapsed: boolean) {
    // Emit sidebar collapsed state only when it is really changed
    if (this._isCollapsed !== collapsed) {
      this._isCollapsed = collapsed;
      this.isCollapsedChange.emit(this._isCollapsed);
      //console.log('Sidebar collapsed: ' + String(this._isCollapsed))
    }    
  }  

  get isCollapsed(): boolean {
    return this._isCollapsed;
  }

  sidebarTheme = 'light'
  sidebarWidth = '200px';
  sidebarCollapsedWidth = 0;
  sidebarBreakpoint = 'md'
  userAvatarSize = 55;

  constructor() {
    console.log("Sidebar component instantiated.");
  }

  ngOnInit() {
  }

  onLogoutButtonPressed() {
    this.isLogoutButtonPressed.emit(true);
  }

}
