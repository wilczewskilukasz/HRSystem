import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

export class User {
  employeeId: string;
  email: string;

  constructor(employeeId: string, email: string) {
    this.employeeId = name;
    this.email = email;
  }
}

@Injectable()
export class AuthService {
  currentUser: User;

  public login(credentials) {
    if (credentials.id === null || credentials.password === null) {
      return Observable.throw("Wprowadź dane logowania");
    } else {
      return Observable.create(observer => {
        // TODO: połączyć się z bazą
        let access = (credentials.password === "admin" && credentials.id === "admin");
        this.currentUser = new User('admin', 'admin@admin.pl');
        observer.next(access);
        observer.complete();
      });
    }
  }

  public getUserInfo(): User {
    return this.currentUser;
  }

  public logout() {
    return Observable.create(observer => {
      this.currentUser = null;
      observer.next(true);
      observer.complete();
    });
  }
}