import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './content/pages/components/dashboard/dashboard.component';
import { FooterComponent } from './content/layout/footer/footer.component';
import { HeaderComponent } from './content/layout/header/header.component';
import { AsideComponent } from './content/layout/aside/aside.component';
import { LoginComponent } from './content/pages/auth/login/login.component';
import { ForgotPasswordComponent } from './content/pages/auth/forgot-password/forgot-password.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VendorManagementComponent } from './content/pages/components/vendor-management/vendor-management.component';
import { ExpensesComponent } from './content/pages/components/expenses/expenses.component';
import { RegisterComponent } from './content/pages/auth/register/register.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { OtpComponent } from './content/pages/auth/otp/otp.component';
import { RequestInterceptorService } from './core/helpers/http-interceptor';
import { SessionService } from './core/helpers/session.service';
import { NgxSpinnerModule } from "ngx-spinner";
import { SettingComponent } from './content/pages/components/setting/setting.component';
import { OnlyNumber } from './core/directives/onlyNumber.directive';
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
    SettingComponent,
    OnlyNumber
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
    SessionService,
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },

    // provider used to create fake backend
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
