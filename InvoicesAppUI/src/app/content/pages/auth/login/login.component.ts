import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { AuthService } from 'src/app/core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { HttpClient } from '@angular/common/http';
import { SessionService } from 'src/app/core/helpers/session.service';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'login-page',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  isSubmitted = false;
  loginForm = new FormGroup({
    email: new FormControl('admin@gmail.com', [Validators.required, Validators.email]),
    password: new FormControl('qwerty1234', [Validators.required, Validators.minLength(6)]),
    deviceType: new FormControl('WEB'),
    deviceToken: new FormControl('q1w2e3r4t5y6u7i8o9p0a1s2de'),

  });
  constructor(private router: Router, private spinner: NgxSpinnerService, private http: HttpClient, private authService: AuthService, private toaster: ToastrService, private sessionService: SessionService) { }
  get f() { return this.loginForm.controls; }

  ngOnInit() {

  }

  onLogin() {
    debugger;
    this.spinner.show();
    setTimeout(() => {
      this.http.post('Account/Login', this.loginForm.value).subscribe(
        (data) => {
          this.sessionService.login(data);
        })
      this.spinner.hide();
      this.router.navigateByUrl('/dashboard')
    }, 2000);
  }
}
