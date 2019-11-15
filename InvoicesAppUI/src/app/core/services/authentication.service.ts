import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../models/user';
import { environment } from 'src/environments/environment';
import { ApiEndPoint } from '../enum/api-end-point.enum';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient, private router: Router, private toaster: ToastrService) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(user: User) {
    return this.http.post<any>(environment.apiUrl + '/' + ApiEndPoint.login, user)
      .pipe(map(user => {

        if (user.userstatus) {
          localStorage.setItem('currentUser', JSON.stringify(user.user_info.accessToken));
          this.currentUserSubject.next(user.userstatus);
          this.toaster.success(user.message);
        }
        else {
          this.toaster.error(user.message);
        }
        return user.userstatus;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigateByUrl('/')
  }

  forgotPassword(email: string) {
    return this.http.post<any>(environment.apiUrl + '/' + ApiEndPoint.forgotPassword, email)
      .pipe(map(user => {
        if (user.userstatus) {
          this.toaster.success(user.message);
        }
        else {
          console.log(user);
          this.toaster.error(user.message);
        }
        return user.userstatus;
      }));
  }



  resetPassword(code: number, email: string, password: string) {
    return this.http.post<any>(environment.apiUrl + '/' + ApiEndPoint.resetPassword, { code, email, password })
      .pipe(map(user => {
        console.log('from auth service', user);
        if (user.userstatus == true) {
          this.toaster.success(user.message);
        }
        else {
          this.toaster.error(user.message);
        }
        return user.userstatus;
      }));
  }
}
