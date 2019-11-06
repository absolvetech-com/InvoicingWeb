import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'm-aside',
  templateUrl: './aside.component.html',
  styleUrls: ['./aside.component.css']
})
export class AsideComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }
  onClickInvoiceManagement() {
    this.router.navigateByUrl('/invoice');
  }
  onClickDashBoard() {
    this.router.navigateByUrl('/dashboard');
  }
}
