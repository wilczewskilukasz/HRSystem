import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { AuthService } from '../../providers/auth-service/auth-service';

@Component({
  selector: 'page-home',
  templateUrl: 'dane.html'
})
export class DanePage {

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
    
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  position: string;
  department: string;
  salaryFee: number;
  privatePhoneNumber: string;
  privateEmail: string;
  holidayDaysTotal: number;
  usedHolidayDays: number;
  leftHolidayDays: number;
  newPrivatePhone: string;
  newPrivateEmail: string;

  saveChanges() {
    let modelPage = this;

    if (modelPage.newPrivatePhone == modelPage.privatePhoneNumber
      && modelPage.newPrivateEmail == modelPage.privateEmail)
        return;
    else {
        let loading = this.loadingCtrl.create({
            content: "Trwa zapisywanie wprowadzonych zmian..."
        });
        loading.present();

        $.ajax({
            url: "http://hrinfoapi.azurewebsites.net/api/Employee/PrivateData",
            type: "PUT",
            dataType: "json",
            data: { "PrivateEmail": modelPage.newPrivateEmail, "PrivatePhoneNumber": modelPage.newPrivatePhone },
            beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
            async: false
        });

        loading.dismiss();
    }
  }

  getData() {
      let loading = this.loadingCtrl.create({
          content: "Trwa ładowanie danych..."
      });
      loading.present();

      let modelPage = this;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Employee/PrivateData",
          type: "POST",
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              modelPage.firstName = wynik.FirstName;
              modelPage.lastName = wynik.LastName;
              modelPage.phone = wynik.PhoneNumber;
              modelPage.email = wynik.Email;
              modelPage.position = wynik.Position;
              modelPage.department = wynik.Department;

              modelPage.salaryFee = wynik.SalaryFee;
              modelPage.privatePhoneNumber = wynik.PrivatePhoneNumber;
              modelPage.privateEmail = wynik.PrivateEmail;
              modelPage.holidayDaysTotal = wynik.HolidayDaysTotal;
              modelPage.usedHolidayDays = wynik.UsedHolidayDays;
              modelPage.leftHolidayDays = wynik.HolidayDaysTotal - wynik.UsedHolidayDays;

              modelPage.newPrivatePhone = wynik.PrivatePhoneNumber;
              modelPage.newPrivateEmail = wynik.PrivateEmail;
          },
          error: function (error) {
              modelPage.authCtrl.showError('Wystąpił błąd podczas pobierania danych.<br/><br/>Prosimy spróbować ponownie później.');
          }
      });
      
      loading.dismiss();
  }
}

