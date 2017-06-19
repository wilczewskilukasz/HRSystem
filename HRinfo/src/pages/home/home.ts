import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { DanePage } from '../dane/dane';
import { AuthService } from '../../providers/auth-service/auth-service';
import { LoginPage } from '../login/login';
import { App } from 'ionic-angular';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})

export class HomePage {

  id = '';

  constructor(public navCtrl: NavController,
    private app: App,
    private authCtrl: AuthService,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
    let info = this.authCtrl.getUserInfo();
    this.id = info['id'];
  }

  daneClick() {
    this.navCtrl.push(DanePage);
  }

  public logout() {
    this.authCtrl.logout().subscribe(succ => {
      this.app.getRootNav().setRoot(LoginPage);
    });
  }
}
