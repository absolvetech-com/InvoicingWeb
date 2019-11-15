import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { first } from 'rxjs/operators';


@Component({
  selector: 'login-page',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private spinner: NgxSpinnerService,
    private authenticationService: AuthenticationService,
    private toaster: ToastrService
  ) {
    // redirect to home if already logged in
    if (this.authenticationService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: new FormControl('admin@gmail.com', [Validators.required, Validators.email]),
      password: new FormControl('qwerty1234', [Validators.required, Validators.minLength(6)]),
      deviceType: new FormControl('WEB'),
      deviceToken: new FormControl('q1w2e3r4t5y6u7i8o9p0a1s2de'),
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  // convenience getter for easy access to form fields

  onLogin() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.spinner.show();
    setTimeout(() => {
      this.authenticationService.login(this.loginForm.value)
        .pipe(first())
        .subscribe(
          data => {
            if (data == true) {
              this.router.navigateByUrl('/dashboard');
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
