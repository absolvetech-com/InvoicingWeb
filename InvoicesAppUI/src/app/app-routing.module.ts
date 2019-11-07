import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoiceManagementComponent } from './content/pages/components/invoice-management/invoice-management.component';
import { DashboardComponent } from './content/pages/components/dashboard/dashboard.component';
import { LoginComponent } from './content/pages/auth/login/login.component';
import { ForgotPasswordComponent } from './content/pages/auth/forgot-password/forgot-password.component';
import { SignUpComponent } from './content/pages/auth/sign-up/sign-up.component';
const routes: Routes = [

  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'invoice',
    component: InvoiceManagementComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent
  },
  {
    path: 'sign-up',
    component: SignUpComponent
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
