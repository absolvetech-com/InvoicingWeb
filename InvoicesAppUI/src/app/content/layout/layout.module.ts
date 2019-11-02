import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AsideComponent } from './aside/aside.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { SubHeaderComponent } from './sub-header/sub-header.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [AsideComponent, FooterComponent, HeaderComponent, SubHeaderComponent],
  exports: [AsideComponent,
    FooterComponent,
    HeaderComponent,
    SubHeaderComponent
  ]
})
export class LayoutModule { }
