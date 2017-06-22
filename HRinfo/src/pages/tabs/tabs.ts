import { Component } from '@angular/core';

import { KalendarzPage } from '../kalendarz/kalendarz';
import { FirmaPage } from '../firma/firma';
import { HomePage } from '../home/home';

@Component({
  templateUrl: 'tabs.html'
})
export class TabsPage {

  tab1Root = FirmaPage;
  tab2Root = HomePage;
  tab3Root = KalendarzPage;

  constructor() {

  }
}
