import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { DanePage } from '../dane/dane';
import { AuthService } from '../../providers/auth-service/auth-service';
import { LoginPage } from '../login/login';

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
    private authCtrl: AuthService,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public viewCtrl: ViewController,
    public loadingCtrl: LoadingController) {
    let info = this.authCtrl.getUserInfo();
    this.id = info['id'];
  }

  daneClick() {

    let modelStrona = this; // this(strona -> obiekt HomePage) przypisana do zmiennej w celu nieutracenia kontekstu.
    let loading = this.loadingCtrl.create({
      content: "Trwa ładowanie..."
    });
    loading.present();

    $.ajax({
      url: "http://hrinfoapi.azurewebsites.net/api/Employee/FindEmployee?employeeId=1",
      async: false,
      success: function (wynik) {
        modelStrona.firstName = wynik.FirstName;
        modelStrona.lastName = wynik.LastName;
        modelStrona.phone = wynik.Phone;
        modelStrona.email = wynik.Email;
        modelStrona.position = wynik.Position;
        modelStrona.department = wynik.Department;
        loading.dismiss();
      },
      error: function (error) {
        alert(JSON.stringify(error));
      }

    });

    this.navCtrl.push(DanePage, { imie: this.firstName, nazwisko: this.lastName });
  }

  public logout() {
    this.authCtrl.logout().subscribe(succ => {
      this.navCtrl.setRoot(LoginPage);
      window.location.reload();
    });
  }
}
