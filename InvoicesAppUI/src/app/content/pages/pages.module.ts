import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { PagesComponent } from './pages.component';
import { LayoutModule } from '../layout/layout.module';
import { PagesRoutingModule } from './pages-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  imports: [
    CommonModule,
    LayoutModule,
    PagesRoutingModule
  ],
  declarations: [
    HeaderComponent,
    PagesComponent,
    DashboardComponent
  ],
  exports: [
    PagesComponent
  ]
})
export class PagesModule { }
