import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-otp',
  templateUrl: './otp.component.html',
  styleUrls: ['./otp.component.css']
})
export class OtpComponent implements OnInit {
  isSubmitted = false;
  verifyOtpForm = new FormGroup({
    otp: new FormControl('', Validators.required)
  });
  constructor(private toaster: ToastrService, private router: Router) { }

  ngOnInit() {
  }
  onVerify() {
    console.log(this.verifyOtpForm.value);
    this.isSubmitted = true;
    if (this.verifyOtpForm.invalid) {
      return;
    }
    this.toaster.info("Email Verified");
    this.verifyOtpForm.reset();
    this.router.navigateByUrl('/login');
  }
}
