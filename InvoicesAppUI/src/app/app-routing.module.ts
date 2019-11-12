import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './content/pages/components/dashboard/dashboard.component';
import { LoginComponent } from './content/pages/auth/login/login.component';
import { ForgotPasswordComponent } from './content/pages/auth/forgot-password/forgot-password.component';
import { VendorManagementComponent } from './content/pages/components/vendor-management/vendor-management.component';
import { ExpensesComponent } from './content/pages/components/expenses/expenses.component';
import { RegisterComponent } from './content/pages/auth/register/register.component';
import { OtpComponent } from './content/pages/auth/otp/otp.component';
import { AuthGuard } from './core/helpers/auth.guard';
import { SettingComponent } from './content/pages/components/setting/setting.component';
const routes: Routes = [

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]

  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'vendor-management',
    component: VendorManagementComponent
  },
  {
    path: 'expenses',
    component: ExpensesComponent
  },
  {
    path: 'setting',
    component: SettingComponent
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'otp',
    component: OtpComponent
  },
  {
    path: '**',
    redirectTo: '404',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { useHash: true })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
