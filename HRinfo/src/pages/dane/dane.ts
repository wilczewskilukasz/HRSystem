import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { App } from 'ionic-angular';

@Component({
  selector: 'page-home',
  templateUrl: 'dane.html'
})
export class DanePage {

  constructor(public navCtrl: NavController,
    public navParams: NavParams,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
      this.getData();
    }
    
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  position: string;
  department: string;

  getData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Employee/FindEmployee?employeeId=1",
          async: false,
          success: function (wynik) {
              modelPage.firstName = wynik.FirstName;
              modelPage.lastName = wynik.LastName;
              modelPage.phone = wynik.PhoneNumber;
              modelPage.email = wynik.Email;
              modelPage.position = wynik.Position;
              modelPage.department = wynik.Department;
          },
          error: function (error) {
              alert(JSON.stringify(error));
          }
      });
      
      loading.dismiss();
  }
}

