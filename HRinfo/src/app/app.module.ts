﻿import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { MyApp } from './app.component';

import { FirmaPage } from '../pages/firma/firma';
import { KalendarzPage } from '../pages/kalendarz/kalendarz';
import { HomePage } from '../pages/home/home';
import { LoginPage } from '../pages/login/login';
import { SzkoleniaPage } from '../pages/szkolenia/szkolenia';
import { TabsPage } from '../pages/tabs/tabs';
import { DanePage } from '../pages/dane/dane';
import { WynagrodzeniaPage } from '../pages/wynagrodzenia/wynagrodzenia';
import { StazPage } from '../pages/staz/staz';
import { WiadomosciPage } from '../pages/wiadomosci/wiadomosci';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { AuthService } from '../providers/auth-service/auth-service';

@NgModule({
  declarations: [
    MyApp,
    LoginPage,
    SzkoleniaPage,
    KalendarzPage,
    HomePage,
    DanePage,
    TabsPage,
    WynagrodzeniaPage,
    StazPage,
    FirmaPage,
    WiadomosciPage
  ],
  imports: [
    BrowserModule,
    IonicModule.forRoot(MyApp, { tabsPlacement: 'top' })
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    LoginPage,
    SzkoleniaPage,
    KalendarzPage,
    HomePage,
    DanePage,
    TabsPage,
    WynagrodzeniaPage,
    StazPage,
    FirmaPage,
    WiadomosciPage
  ],
  providers: [
    StatusBar,
    SplashScreen,
    { provide: ErrorHandler, useClass: IonicErrorHandler },
    AuthService
  ]
})
export class AppModule { }
