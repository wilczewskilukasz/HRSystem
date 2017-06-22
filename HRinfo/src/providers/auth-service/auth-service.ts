import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { AlertController } from 'ionic-angular';

export class User {
  employeeId: string;
  email: string;
  token: string;

  constructor(employeeId: string, email: string, token: string, ) {
    this.employeeId = employeeId;
    this.email = email;
    this.token = token;
  }
}

@Injectable()
export class AuthService {
    currentUser: User;

    constructor(public alertCtrl: AlertController) { };

  public login(credentials) {
    if (credentials.id === null || credentials.password === null) {
      return Observable.throw("Wprowadź dane logowania");
    } else {

        let token: string;
        let pageModel = this;

        return Observable.create(observer => {
          let access = false;
          $.ajax({
              url: "http://hrinfoapi.azurewebsites.net/api/Account/Login",
              type: "POST",
              dataType: "json",
              data: { "Email": credentials.id, "Password": credentials.password },
              async: false,
              success: function (wynik) {
                  access = true;
                  token = wynik.token_type + ' ' + wynik.access_token;
              },
              error: function (error) {
                  let alertKomunikat = pageModel.alertCtrl.create({
                      title: 'Nieprawidłowe dane logowania!',
                      subTitle: 'Wprowadzone login lub hasło są niepoprawne.',
                      buttons: ['OK']
                  });
                  alertKomunikat.present();
              }
          });
          this.currentUser = new User(credentials.id, credentials.id, token);
        observer.next(access);
        observer.complete();
      });
    }
  }

  public getUserInfo(): User {
    return this.currentUser;
  }

  public getUserID(): String {
    return this.currentUser.employeeId;
  }

  public getToken(): string {
      return this.currentUser.token;
  }

  public logout() {
    return Observable.create(observer => {
      this.currentUser = null;
      observer.next(true);
      observer.complete();
    });
  }

  public showError(text) {
      let alert = this.alertCtrl.create({
          title: 'Wystąpił błąd.',
          subTitle: text,
          buttons: ['OK']
      });
      alert.present(prompt);
  }
}