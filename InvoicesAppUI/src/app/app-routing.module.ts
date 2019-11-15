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
import { PageNotFoundComponent } from './content/pages/auth/page-not-found/page-not-found.component';
import { ResetPasswordComponent } from './content/pages/auth/reset-password/reset-password.component';
import { LinkedUserComponent, CurrencyComponent, EditProfileComponent, ChangePasswordComponent, LogsComponent, CustomerInvoiceComponent, TaxComponent, ItemsComponent, StatementComponent, SignatureComponent, MembershipComponent, InvitePeopleComponent, ContactUsComponent } from './content/pages/components';
const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  {
    path: 'vendor-management',
    component: VendorManagementComponent, canActivate: [AuthGuard]
  },
  {
    path: 'expenses',
    component: ExpensesComponent, canActivate: [AuthGuard]
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'otp',
    component: OtpComponent,
  },
  {
    path: 'linked-user',
    component: LinkedUserComponent,
  },
  {
    path: 'currency',
    component: CurrencyComponent,
  },
  {
    path: 'edit-profile',
    component: EditProfileComponent,
  },
  {
    path: 'change-password',
    component: ChangePasswordComponent,
  },
  {
    path: 'logs',
    component: LogsComponent,
  },
  {
    path: 'customer-invoice',
    component: CustomerInvoiceComponent,
  },
  {
    path: 'tax',
    component: TaxComponent,
  },
  {
    path: 'items',
    component: ItemsComponent,
  },
  {
    path: 'statement',
    component: StatementComponent,
  },
  {
    path: 'signature',
    component: SignatureComponent,
  },
  {
    path: 'membership',
    component: MembershipComponent,
  },
  {
    path: 'invite-people',
    component: InvitePeopleComponent,
  },
  {
    path: 'contact-us',
    component: ContactUsComponent,
  },

  //Page Not Found Routing
  {
    path: '**',
    component: PageNotFoundComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { useHash: true })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
