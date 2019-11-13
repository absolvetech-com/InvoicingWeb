import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpClient } from '@angular/common/http';
import { SessionService } from 'src/app/core/helpers/session.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  isSubmitted = false;
  registerForm = new FormGroup({
    name: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    phone: new FormControl('', [Validators.required, Validators.minLength(10)]),
    role: new FormControl('admin'),
    deviceToken: new FormControl('admindevictoken'),
    deviceType: new FormControl('web')
  });
  constructor(private spinner: NgxSpinnerService, private http: HttpClient, private sessionService: SessionService) { }

  ngOnInit() {
  }

  onRegister() {
    debugger;
    this.http.post('Account/Register', this.registerForm.value).subscribe(
      (data) => {
        this.sessionService.registerUser(data);
      })
    this.spinner.hide();

  }

}
