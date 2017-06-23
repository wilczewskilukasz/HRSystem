import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { AuthService } from '../../providers/auth-service/auth-service';

@Component({
  selector: 'page-tabs',
  templateUrl: 'kalendarz.html'
})
export class KalendarzPage {

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

  resultNumber: number = 0;

  calendarList: {
      id: number,
      name: string,
      description: string,
      dateFrom: Date,
      timeFrom: TimeRanges,
      dateTo: Date,
      timeTo: TimeRanges,
      workDaysNumber: number,
      statusId: number,
      statusName: string,
      eventId: number,
      eventName: string
  }[] = [];

  getData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Calendar/Employee",
          type: "GET",
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              $.each(wynik, function (index) {
                  modelPage.calendarList.push({
                      "id": wynik[index].Id,
                      "name": wynik[index].Name,
                      "description": wynik[index].Description,
                      "dateFrom": wynik[index].DateFrom,
                      "timeFrom": wynik[index].TimeFrom,
                      "dateTo": wynik[index].DateTo,
                      "timeTo": wynik[index].TimeTo,
                      "workDaysNumber": wynik[index].WorkDaysNumber,
                      "statusId": wynik[index].StatusId,
                      "statusName": wynik[index].StatusName,
                      "eventId": wynik[index].EventId,
                      "eventName": wynik[index].EventName
                  });
              });

              if (modelPage.calendarList.length < 1) {
                  modelPage.resultNumber = 0;
              }
              else {
                  modelPage.resultNumber = modelPage.calendarList.length;
              }
          },
          error: function (error) {
              modelPage.authCtrl.showError('Wystąpił błąd podczas pobierania danych.<br/><br/>Prosimy spróbować ponownie później.');
              modelPage.resultNumber = 0;
          }
      });

      loading.dismiss();

      if (modelPage.resultNumber == 0)
          alert("Brak aktywnych wpisów w kalendarzu.");
  }
}
