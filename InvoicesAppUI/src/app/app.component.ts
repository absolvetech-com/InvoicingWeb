import { Component, Input } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  hiddenHeaderAsideFromDefualtPage = true;
  constructor(
    private router: Router) { }

  ngOnInit() {
    this.router.events.subscribe((defaultpage) => {
      if (defaultpage instanceof NavigationStart) {
        debugger
        if (defaultpage.url === "/login" || defaultpage.url === "/forgot-password" || defaultpage.url === "/sign-up") {
          this.hiddenHeaderAsideFromDefualtPage = false;
        } else {
          this.hiddenHeaderAsideFromDefualtPage = true;
        }
      }
    })
  }
}
