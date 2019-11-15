import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';
  registerForm = new FormGroup({
    name: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*\\d)[a-z\\d$@$#!%*?&]{8,}')]),
    phone: new FormControl('', [Validators.required, Validators.minLength(10)]),
    role: new FormControl('admin'),
    deviceToken: new FormControl('admindevictoken'),
    deviceType: new FormControl('web')
  });
  constructor(private spinner: NgxSpinnerService, private toaster: ToastrService, private router: Router, private userService: UserService) { }

  ngOnInit() {
  }

  onRegister() {
    this.submitted = true;
    if (this.registerForm.invalid) {
      return;
    }

    this.spinner.show();
    setTimeout(() => {
      this.userService.register(this.registerForm.value)
        .pipe(first())
        .subscribe(
          data => {
            if (data.success == true) {
              this.router.navigateByUrl('/otp'),
                this.toaster.success(data.message);
            }
            else {
              this.toaster.error(data.message);
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
