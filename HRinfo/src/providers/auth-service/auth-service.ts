import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

export class User {
  employeeId: string;
  email: string;
  token: string;

  constructor(employeeId: string, email: string, token: string) {
    this.employeeId = employeeId;
    this.email = email;
    this.token = token;
  }
}

@Injectable()
export class AuthService {
    currentUser: User;

  public login(credentials) {
    if (credentials.id === null || credentials.password === null) {
      return Observable.throw("Wprowadź dane logowania");
    } else {

        let token: string;

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
                  alert(JSON.stringify(error));
              }
          });
        // TODO: dokończyć !!!
        this.currentUser = new User('admin', credentials.id, token);
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
}