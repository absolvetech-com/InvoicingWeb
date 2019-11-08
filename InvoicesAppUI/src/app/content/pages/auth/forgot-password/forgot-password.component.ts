import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'forgot-password-page',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  public showLoginLink = false;
  constructor(private toastr: ToastrService) { }

  ngOnInit() {
  }

  onClickSendResetLink() {
    this.toastr.success("Password Reset Link Send To Your Email Account", 'Sent')
    this.showLoginLink = true;
  }
}
