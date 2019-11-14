import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './content/layout/footer/footer.component';
import { HeaderComponent } from './content/layout/header/header.component';
import { AsideComponent } from './content/layout/aside/aside.component';
import { LoginComponent } from './content/pages/auth/login/login.component';
import { ForgotPasswordComponent } from './content/pages/auth/forgot-password/forgot-password.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './content/pages/auth/register/register.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { OtpComponent } from './content/pages/auth/otp/otp.component';
import { NgxSpinnerModule } from "ngx-spinner";
import { OnlyNumber } from './core/directives/onlyNumber.directive';
import { JwtInterceptor, ErrorInterceptor } from './core/helpers';
import { DashboardComponent, VendorManagementComponent, ExpensesComponent } from './content/pages/components';
import { PageNotFoundComponent } from './content/pages/auth/page-not-found/page-not-found.component';
import { ResetPasswordComponent } from './content/pages/auth/reset-password/reset-password.component';
@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    LoginComponent,
    FooterComponent,
    HeaderComponent,
    AsideComponent,
    ForgotPasswordComponent,
    VendorManagementComponent,
    ExpensesComponent,
    RegisterComponent,
    OtpComponent,
    OnlyNumber,
    PageNotFoundComponent,
    ResetPasswordComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    NgxSpinnerModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-bottom-right',

    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    // provider used to create fake backend
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
