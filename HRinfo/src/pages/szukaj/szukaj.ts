import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { AuthService } from '../../providers/auth-service/auth-service';
import { CallNumber } from '@ionic-native/call-number';

@Component({
  selector: 'page-firma',
  templateUrl: 'szukaj.html'
})
export class SzukajPage {

  token: string;

  constructor(public navCtrl: NavController,
    private authCtrl: AuthService,
    private callNumber: CallNumber,
    public navParams: NavParams,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
      this.token = this.authCtrl.getToken();
    }
    
  firstName: string;
  lastName: string;
  position: string;
  department: string;
  resultsNumber: number;
  afterSearch: boolean = false;

  resultList: { resultFirstName: string, resultLastName: string, resultPhoneNumber: string, resultEmail: string, resultPosition: string, resultDepartment: string } [] = [];

  newSearch() {
      this.afterSearch = false;
      //this.resultsNumber = 0;
  }

  searchEmployee() {
      let loading = this.loadingCtrl.create({
          content: "Trwa wyszukiwanie danych..."
      });
      loading.present();

      let modelPage = this;

      modelPage.resultList = [];
      modelPage.resultsNumber = 0;

      $.ajax({
          url: "http://hrinfoapi.azurewebsites.net/api/Employee/FindEmployee",
          type: "POST",
          data: { "FirstName": modelPage.firstName, "LastName": modelPage.lastName, "Position": modelPage.position, "Department": modelPage.department },
          dataType: "json",
          beforeSend: function (xhr) { xhr.setRequestHeader('Authorization', modelPage.token); },
          async: false,
          success: function (wynik) {
              $.each(wynik, function (index) {
                  modelPage.resultList.push({ "resultFirstName": wynik[index].FirstName, "resultLastName": wynik[index].LastName, "resultPhoneNumber": wynik[index].PhoneNumber, "resultEmail": wynik[index].Email, "resultPosition": wynik[index].Position, "resultDepartment": wynik[index].Department });
              });
              modelPage.resultsNumber = modelPage.resultList.length;
          },
          error: function (error) {
              modelPage.resultsNumber = 0;
          }
      });

      //$.each(modelPage.resultsNumber, function (index) {
      //    modelPage.callNumber.callNumber(modelPage.resultsNumber[index].resultPhoneNumber, true);
      //});

      modelPage.afterSearch = true;
      
      loading.dismiss();
  }
}

