import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'forgot-password-page',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }


  onClickSendResetLink() {
    alert("Reset Link Send To Your Email Account");
  }
}
