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
  eventId: number = 0;
  dateFrom: Date = null;
  dateTo: Date = null;
  workDaysNumber: number = 0;
  eventList: { id: number, name: string }[] = [];
  solutionBaseId: number;

  addData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      if (modelPage.workDaysNumber == 0) {
          alert("Zweryfikuj liczbę dni wolnych przed wysłaniem wniosku.");
          loading.dismiss();
          return;
      }

      if (modelPage.eventId == 0 || modelPage.workDaysNumber == 0 || !modelPage.dateFrom || !modelPage.dateTo) {
          alert("Przed wysłaniem wniosku należy uzupełnić wszystkie pola!");
          loading.dismiss();
          return;
      }

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Calendar/Employee",
          type: "POST",
          data: {
              "DateFrom": modelPage.dateFrom.toString(),
              "DateTo": modelPage.dateTo.toString(),
              "WorkDaysNumber": modelPage.workDaysNumber,
              "EventId": modelPage.eventId,
              "IsActive": true
            },
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function () {
              alert('Twój urlop został przesłany do akceptacji.');
              modelPage.eventId = 0;
              modelPage.workDaysNumber = 0;
              modelPage.dateFrom = null;
              modelPage.dateTo = null;
          },
          error: function (error) {
              if (error.status == 200 || error.statusText == "OK") {
                  alert('Twój urlop został przesłany do akceptacji.');
                  modelPage.eventId = 0;
                  modelPage.workDaysNumber = 0;
                  modelPage.dateFrom = null;
                  modelPage.dateTo = null;
              }
              else {
                  modelPage.authCtrl.showError('Wystąpił błąd podczas zapisywania danych.<br/><br/>Prosimy spróbować ponownie później.');
              }
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

