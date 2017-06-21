import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';

@Component({
  selector: 'page-home',
  templateUrl: 'wynagrodzenia.html'
})
export class WynagrodzeniaPage {

  constructor(public navCtrl: NavController,
    public navParams: NavParams,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
      this.getData();
  }

  salariesList: { amount: number, date: Date } [] = [];

  lengthList: number[] = [];

  getData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      for (var i = 0; i < 10; i++) {
          modelPage.lengthList.push(i);
      }

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Employee/Salaries",
          type: "POST",
          async: false,
          success: function (wynik) {
              $.each(wynik, function (index) {
                  modelPage.salariesList.push({ "amount": wynik[index].Amount, "date": wynik[index].Date });
              });
          },
          error: function (error) {
              alert(JSON.stringify(error));
          }
      });
      
      loading.dismiss();
  }
}

