import { Component, Input } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  hiddenHeaderAside = true;
  constructor(
    private router: Router) { }

  ngOnInit() {
    this.router.events.subscribe((defaultpage) => {
      if (defaultpage instanceof NavigationStart) {
        if (defaultpage.url === "/" || defaultpage.url === "/login" || defaultpage.url === "/forgot-password" || defaultpage.url === "/sign-up") {
          this.hiddenHeaderAside = false;
        } else {
          this.hiddenHeaderAside = true;
        }
      }
    })
  }
}
