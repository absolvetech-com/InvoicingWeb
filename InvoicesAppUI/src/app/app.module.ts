import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './content/pages/components/dashboard/dashboard.component';
import { InvoiceManagementComponent } from './content/pages/components/invoice-management/invoice-management.component';
import { FooterComponent } from './content/layout/footer/footer.component';
import { HeaderComponent } from './content/layout/header/header.component';
import { AsideComponent } from './content/layout/aside/aside.component';
import { LoginComponent } from './content/pages/auth/login/login.component';
import { ForgotPasswordComponent } from './content/pages/auth/forgot-password/forgot-password.component';
import { SignUpComponent } from './content/pages/auth/sign-up/sign-up.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    LoginComponent,
    InvoiceManagementComponent,
    FooterComponent,
    HeaderComponent,
    AsideComponent,
    ForgotPasswordComponent,
    SignUpComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
