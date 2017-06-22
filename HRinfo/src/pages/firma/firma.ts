import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { WiadomosciPage } from '../wiadomosci/wiadomosci';
import { AuthService } from '../../providers/auth-service/auth-service';
import { SzukajPage } from '../szukaj/szukaj';
import { App } from 'ionic-angular';

@Component({
  selector: 'page-firma',
  templateUrl: 'firma.html'
})

export class FirmaPage {

id: String;

  constructor(public navCtrl: NavController,
    private app: App,
    private authCtrl: AuthService,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
      this.token = this.authCtrl.getToken();
      this.getData();
  }

  token: string;
  newsList: { news: string, dateFrom: Date, dateTo: Date }[] = [];

  getData() {
      let modelPage = this;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Company/TopNews",
          type: "POST",
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              $.each(wynik, function (index) {
                  modelPage.newsList.push({ "news": wynik[index].ResultNews, "dateFrom": wynik[index].ResultDateFrom, "dateTo": wynik[index].ResultDateTo });
              });
          },
          error: function (error) {
              modelPage.authCtrl.showError('Wystąpił błąd podczas pobierania wiadomości.<br/><br/>Prosimy spróbować ponownie później.');
          }
      });
  }

  search() {
      this.navCtrl.push(SzukajPage);
  }





  newses() {
      this.navCtrl.push(WiadomosciPage);
  }
}
