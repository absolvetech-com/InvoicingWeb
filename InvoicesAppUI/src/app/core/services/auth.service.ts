import { Injectable } from '@angular/core';
import { SessionService } from '../helpers/session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  currentUserValue: any;

  constructor(private sessionService: SessionService) { }



  public isLoggedIn() {
    debugger;
    this.sessionService.getToken();
    return localStorage.getItem('ACCESS_TOKEN')!! === null;
  }

  public logout() {
    localStorage.removeItem('ACCESS_TOKEN');
  }

}
