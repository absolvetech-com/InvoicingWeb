import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { MustMatch } from 'src/app/core/helpers/must-match.validator';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';


  ResetPasswordOtpForm = new FormGroup({

  }
  );


  constructor(private toaster: ToastrService, private router: Router, private spinner: NgxSpinnerService, private formBuilder: FormBuilder, private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.ResetPasswordOtpForm = this.formBuilder.group({
      otp: new FormControl('', Validators.required),
      newpassword: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d$@$#!%*?&]{8,}")]),
      confirmpassword: new FormControl('', Validators.required),

    }, { validator: this.checkIfMatchingPasswords('newpassword', 'confirmpassword') });

  }
  get f() { return this.ResetPasswordOtpForm.controls; }


  checkIfMatchingPasswords(passwordKey: string, passwordConfirmationKey: string) {
    return (group: FormGroup) => {
      let passwordInput = group.controls[passwordKey],
        passwordConfirmationInput = group.controls[passwordConfirmationKey];
      if (passwordInput.value !== passwordConfirmationInput.value) {
        return passwordConfirmationInput.setErrors({ notEquivalent: true })
      }
      else {
        return passwordConfirmationInput.setErrors(null);
      }
    }
  }

  onResetPassword() {
    this.submitted = true;
    console.log(this.ResetPasswordOtpForm.value);
    if (this.ResetPasswordOtpForm.invalid) {
      return;
    }

    this.spinner.show();
    setTimeout(() => {
      const email = localStorage.getItem('email')
      this.authenticationService.resetPassword(this.ResetPasswordOtpForm.controls['otp'].value, email, this.ResetPasswordOtpForm.controls['newpassword'].value)
        .pipe(first())
        .subscribe(
          data => {
            if (data == true) {
              this.router.navigateByUrl('/login');
              localStorage.removeItem('email');
            }
            this.spinner.hide();
          });
    }, 2000);
  }
}
