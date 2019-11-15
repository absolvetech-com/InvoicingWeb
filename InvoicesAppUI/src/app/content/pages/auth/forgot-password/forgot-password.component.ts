import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, Validators, FormControl, FormBuilder } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
@Component({
  selector: 'forgot-password-page',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';
  public showLoginLink = false;
  constructor(private toastr: ToastrService, private formBuilder: FormBuilder, private spinner: NgxSpinnerService, private authenticationService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.forgotPasswordForm = this.formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email]),
    });
  }

  onClickSendResetLink() {
    this.submitted = true;
    if (this.forgotPasswordForm.invalid) {
      return;
    }

    this.spinner.show();
    setTimeout(() => {
      this.authenticationService.forgotPassword(this.forgotPasswordForm.value)
        .pipe(first())
        .subscribe(
          data => {
            if (data == true) {
              localStorage.setItem('email', this.forgotPasswordForm.controls['email'].value)
              this.router.navigateByUrl('/reset-password');
            }
            this.spinner.hide();
          },
          error => {
            this.error = error;
            this.loading = false;
            this.spinner.hide();
          });
    }, 2000);
  }
}
