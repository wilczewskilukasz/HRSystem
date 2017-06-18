import { Component } from '@angular/core';

import { KalendarzPage } from '../kalendarz/kalendarz';
import { SzkoleniaPage } from '../szkolenia/szkolenia';
import { HomePage } from '../home/home';

@Component({
  templateUrl: 'tabs.html'
})
export class TabsPage {

  tab1Root = HomePage;
  tab2Root = KalendarzPage;
  tab3Root = SzkoleniaPage;

  constructor() {

  }
}
