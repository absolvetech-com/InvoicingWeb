import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesComponent } from './pages.component';
import { LayoutModule } from '../layout/layout.module';
import { PagesRoutingModule } from './pages-routing.module';
import { LoginComponent } from './auth/login/login.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';

@NgModule({
  imports: [
    CommonModule,
    LayoutModule,
    PagesRoutingModule,
  ],
  declarations: [
    PagesComponent,
    LoginComponent,
    SignUpComponent,
    ForgotPasswordComponent,
  ],
  exports: [
    PagesComponent,
    LoginComponent,
    SignUpComponent,
    ForgotPasswordComponent,
  ]
})
export class PagesModule { }

