import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { DanePage } from '../dane/dane';
import { AuthService } from '../../providers/auth-service/auth-service';
import { LoginPage } from '../login/login';
import { App } from 'ionic-angular';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})

export class HomePage {

  id = '';
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  position: string;
  department: string;


  constructor(public navCtrl: NavController,
    private app: App,
    private authCtrl: AuthService,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
    let info = this.authCtrl.getUserInfo();
    this.id = info['id'];
  }

  daneClick() {

    let modelPage = this;
    let loading = this.loadingCtrl.create({
      content: "Trwa ładowanie..."
    });
    loading.present();

    $.ajax({
      url: "http://hrinfoapi.azurewebsites.net/api/Employee/FindEmployee?employeeId=1",
      async: false,
      success: function (wynik) {
        modelPage.firstName = wynik.FirstName;
        modelPage.lastName = wynik.LastName;
        modelPage.phone = wynik.Phone;
        modelPage.email = wynik.Email;
        modelPage.position = wynik.Position;
        modelPage.department = wynik.Department;
      },
      error: function (error) {
        alert(JSON.stringify(error));
      }

    });

    this.navCtrl.push(DanePage, {
      pushedFirstName: this.firstName,
      pushedLastName: this.lastName,
      pushedPhone: this.phone,
      pushedEmail: this.email,
      pushedPosition: this.position,
      pushedDepartment: this.department
    });
    loading.dismiss();
  }

  public logout() {
    this.authCtrl.logout().subscribe(succ => {
      this.app.getRootNav().setRoot(LoginPage);
    });
  }
}
