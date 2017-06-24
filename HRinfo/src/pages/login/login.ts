import { Component } from '@angular/core';
import { NavController, AlertController, LoadingController, Loading, IonicPage } from 'ionic-angular';
import { AuthService } from '../../providers/auth-service/auth-service';

import { TabsPage } from '../tabs/tabs';

@IonicPage()
@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
})
export class LoginPage {
    loading: Loading;
    // TODO: na koniec usunąć dane logowania
    //registerCredentials = { id: 'admin@admin.pl', password: 'Admin123.' };
    //registerCredentials = { id: 'tester@wp.pl', password: 'Qwerty123.' };

    constructor(private nav: NavController, private auth: AuthService, private alertCtrl: AlertController, private loadingCtrl: LoadingController) {
    }

  public login() {
    this.showLoading()
    this.auth.login(this.registerCredentials).subscribe(allowed => {
      if (allowed) {
        this.nav.setRoot(TabsPage);
      } else {
        this.loading.dismiss();
      }
    },
      error => {
        this.loading.dismiss();
        //this.showError(error);
      });
  }

  showLoading() {
    this.loading = this.loadingCtrl.create({
      content: 'Ładowanie...',
      dismissOnPageChange: true
    });
    this.loading.present();
  }

  showError(text) {
      this.loading.dismiss();

    let alert = this.alertCtrl.create({
      title: 'Wystąpił błąd.',
      subTitle: text,
      buttons: ['OK']
    });
    alert.present(prompt);
  }
}