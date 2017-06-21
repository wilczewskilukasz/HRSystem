import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { AuthService } from '../../providers/auth-service/auth-service';

@Component({
  selector: 'page-home',
  templateUrl: 'staz.html'
})
export class StazPage {

  token: string;

  constructor(public navCtrl: NavController,
    private authCtrl: AuthService,
    public navParams: NavParams,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
      this.token = this.authCtrl.getToken();
      this.getData();
  }

  positionsList: { id: number, position: string, dateFrom: Date, dateTo: Date, salary: number }[] = [];

  getData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Employee/PositionsHistory",
          type: "POST",
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              $.each(wynik, function (index) {
                  modelPage.positionsList.push({
                      "id": wynik[index].Id,
                      "position": wynik[index].Position,
                      "dateFrom": wynik[index].DateFrom,
                      "dateTo": wynik[index].DateTo,
                      "salary": wynik[index].Salary
                  });
              });
          },
          error: function (error) {
              alert(JSON.stringify(error));
          }
      });
      
      loading.dismiss();
  }
}

