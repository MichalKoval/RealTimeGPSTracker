import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardModule } from './dashboard/dashboard.module';
import { AuthModule } from './auth/auth.module';
import { IconsProviderModule } from './icons-provider.module';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData, DatePipe } from '@angular/common';
import en from '@angular/common/locales/en';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools'
import { ErrorPageModule } from './error-page/error-page.module';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { AuthorizationGuard } from './core/services/auth-guard.service';
import { EffectsModule } from '@ngrx/effects';
import { CoordinateService } from './core/services/coordinate.service';
import { DashboardService } from './core/services/dashboard.service';
import { DeviceService } from './core/services/device.service';
import { HistoryService } from './core/services/history.service';
import { LoginService } from './core/services/login.service';
import { RegistrationService } from './core/services/registration.service';
import { UserService } from './core/services/user.service';
import { environment } from '@environments/environment';
import { PaginationService } from './core/services/pagination.service';
import { SortService } from './core/services/sort.service';
import { RealtimeService } from './core/services/realtime.service';
import { BaseService } from './core/services/base.service';
import { SignalRService } from './core/services/signalR.service';

// NgRx store effects
import { CoordinateEffects } from './core/store/effects/coordinate.effects';
import { DeviceEffects } from './core/store/effects/device.effects';
import { HistoryEffects } from './core/store/effects/history.effects';
import { RealtimeEffects } from './core/store/effects/realtime.effects';
import { DashboardEffects } from './core/store/effects/dashboard.effects';
import { AuthEffects } from './core/store/effects/auth.effects';
import { UserEffects } from './core/store/effects/user.effects';

// NgRx SignalR store effects
import { SignalREffects } from 'ngrx-signalr-core';

// NgRx global app state
import { reducers } from './core/store/states/state';
import { JwtService } from './core/services/jwt.service';

// Ngx Logger
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';


registerLocaleData(en);

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule,
      
      // Definition of app routes including routes exported from Auth and Dashboard modules.
      AppRoutingModule,
      
      // Authorization module - includes Login, Logout and Registration module and their components.
      AuthModule,
      // Dashboard module - includes User, Realime, History and Device modules and their components.
      DashboardModule,
      // ErrorPage module - includes components for error code pages.
      ErrorPageModule,

      // Layout helper modules for custom app layout based on NG Zorro Ant library.
      // (Custom layout consists of header, footer, sidebar, breadcrumb, ...)
      IconsProviderModule,
      //NgZorroAntdModule,
      FormsModule,
      HttpClientModule,
      BrowserAnimationsModule,

      /// Definition of NgRx app state:
      // NgRx redurers used in Auth and Dashboard modules (registered globally not lazy).
      StoreModule.forRoot(reducers),
      
      // NgRx effects used in Auth and Dashboard modules (registered globally not lazy).
      EffectsModule.forRoot([
         AuthEffects,
         CoordinateEffects,
         DashboardEffects,
         DeviceEffects,
         HistoryEffects,
         RealtimeEffects,
         UserEffects,
         SignalREffects
      ]),

      // Ngx Logger to log events in the app.
      LoggerModule.forRoot({
         serverLoggingUrl: `${environment.apiUrl}/logs`,
         level: environment.logLevel,
         serverLogLevel: environment.serverLogLevel,
         disableConsoleLogging: false
      }),

      // Use Redux (NgRx) debug tool in development mode.
      !environment.production ? StoreDevtoolsModule.instrument() : []
   ],
   providers: [
      
      // Definition of authorization guard service used in app routing module.
      AuthorizationGuard,

      // Services
      BaseService,
      CoordinateService,
      DashboardService,
      DeviceService,
      HistoryService,
      RealtimeService,
      LoginService,
      RegistrationService,
      UserService,
      PaginationService,
      SortService,
      JwtService,

      // SignalR services
      //SignalRService,
      //TODO:

      // DatePipe service
      DatePipe,

      // Base url addresss
      { provide: "API_BASE_URL", useValue: environment.apiUrl },
      
      // Language pack
      { provide: NZ_I18N, useValue: en_US },
      
      // Adds JWT token to the authorization header when user is logged in so data can be retrieved from backend.
      { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
      
      //{ provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
