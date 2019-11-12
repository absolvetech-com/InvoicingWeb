import { Component, OnInit } from '@angular/core';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/core/services/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { SessionService } from 'src/app/core/helpers/session.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'm-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  providers: [NgbModalConfig, NgbModal]
})
export class HeaderComponent implements OnInit {
  login: boolean;
  constructor(config: NgbModalConfig, private spinner: NgxSpinnerService, private modalService: NgbModal, private sessionService: SessionService, private toaster: ToastrService, private authService: AuthService, private router: Router) {
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
      this.spinner.hide();
      this.toaster.success("Succusfully Logout");
    }, 1000);
    this.sessionService.logout();
    this.router.navigateByUrl('/login');

  }
}
