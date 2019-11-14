import { Component, OnInit } from '@angular/core';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'm-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  providers: [NgbModalConfig, NgbModal]
})
export class HeaderComponent implements OnInit {
  login: boolean;
  constructor(config: NgbModalConfig, private spinner: NgxSpinnerService, private authService: AuthenticationService, private modalService: NgbModal, private toaster: ToastrService, private router: Router) {
    // customize default values of modals used by this component tree
    config.backdrop = 'static';
    config.keyboard = false;
  }

  openCreateNewInvoice(content) {
    this.modalService.open(content, { windowClass: 'my-class' });
  }

  ngOnInit() {
  }


  logout() {
    this.spinner.show();
    setTimeout(() => {
      this.authService.logout();
      this.spinner.hide();
      this.toaster.success("Succusfully Logout")
    }, 2000)
  }
}
