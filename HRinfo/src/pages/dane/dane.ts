import { Component } from '@angular/core';
import {NavController, NavParams} from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';

@Component({
  selector: 'page-home',
  templateUrl: 'dane.html'
})
export class DanePage {

  constructor(private navController: NavController, private navParams: NavParams) {

  }

}
