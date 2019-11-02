import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { PagesComponent } from './pages.component';
import { LayoutModule } from '../layout/layout.module';

@NgModule({
  imports: [
    CommonModule,
    LayoutModule,
  ],
  declarations: [HeaderComponent, PagesComponent]
})
export class PagesModule { }
