import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { AuthService } from '../../providers/auth-service/auth-service';

@Component({
  selector: 'page-dane',
  templateUrl: 'urlopy.html'
})
export class UrlopyPage {
    
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

  token: string;
  allowAdd: boolean = true;
  eventName: string;
  dateFrom: Date;
  dateTo: Date;
  workDaysNumber: number;
  eventList: { id: number, name: string }[] = [];
  solutionBaseId: number;

  fakeVal: string = "Urlop wypoczynkowy";

  addData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Calendar/Employee",
          type: "POST",
          data: {
              "DateFrom": modelPage.dateFrom,
              "DateTo": modelPage.dateTo,
              "WorkDaysNumber": modelPage.workDaysNumber,
              "StatusName": "Nowy",
              "EventName": modelPage.eventName,
              "IsActive": true
            },
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              alert('Twój urlop został przesłany do akceptacji.');
          },
          error: function (error) {
              //modelPage.authCtrl.showError('Wystąpił błąd podczas zapisywania danych.<br/><br/>Prosimy spróbować ponownie później.');
              alert('Twój urlop został przesłany do akceptacji.');
          }
      });

      loading.dismiss();
  }

  getData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Dictionary/SolutionBase?name=urlop",
          type: "GET",
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              modelPage.solutionBaseId = wynik[0].Id;
          },
          error: function (error) {
              modelPage.solutionBaseId = 0;
          }
      });

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Dictionary/Event/SolutionBaseId?solutionBaseId=" + this.solutionBaseId,
          type: "GET",
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              $.each(wynik, function (index) {
                  modelPage.eventList.push({ "id": wynik[index].Id, "name": wynik[index].Name });
              });
              modelPage.allowAdd = true;
          },
          error: function (error) {
              modelPage.authCtrl.showError('Wystąpił błąd podczas pobierania danych.<br/><br/>Prosimy spróbować ponownie później.');
              modelPage.allowAdd = false;
          }
      });

      loading.dismiss();
  }
}

