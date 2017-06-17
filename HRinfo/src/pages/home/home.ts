import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { DanePage } from '../dane/dane';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {



  constructor(public navCtrl: NavController,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {

  }


  daneClick() {
    $.ajax({
      url: 'http://hrinfoapi.azurewebsites.net/api/Employee/FindEmployee?employeeId=1',
      success: function (wynik) {
        this.navCtrl.push(DanePage, {
          param1: 'John', param2: 'Johnson'
        });
      },
      error: function (error) {
        alert(JSON.stringify(error));
      }
    });
  }
}
