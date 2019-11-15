import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { config, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiEndPoint } from '../enum/api-end-point.enum';
import { Router } from '@angular/router';
import { LinkedUsers } from '../models/linkeduser';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private http: HttpClient, private router: Router) { }

  // getAll() {
  //   return this.http.get<User[]>(`${environment.apiUrl + '/' + ApiEndPoint.register}`);
  // }

  register(user: User) {
    localStorage.setItem('email', user.email)
    return this.http.post<any>(environment.apiUrl + '/' + ApiEndPoint.register, user)
  }

  onVerify(code: string) {
    const email = localStorage.getItem('email');
    return this.http.post<any>(environment.apiUrl + '/' + ApiEndPoint.confirmEmail, { code, email });
  }

  getAllLinkedUsers(): Observable<LinkedUsers[]> {
    debugger;
    return this.http.get<any>(environment.apiUrl + '/' + ApiEndPoint.linkedUsers);
  }
}
