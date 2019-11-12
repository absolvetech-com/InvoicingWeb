import {
  HttpInterceptor,
  HttpHandler,
  HttpResponse,
  HttpRequest,
  HttpEvent,
  HttpHeaders
} from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { tap } from 'rxjs/operators';
import { SessionService } from './session.service';
import { AuthService } from '../services/auth.service';


@Injectable()
export class RequestInterceptorService implements HttpInterceptor {

  constructor(
    private authenticationService: AuthService,
    private router: Router,
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    let currentUser = this.authenticationService.currentUserValue;
    if (currentUser && currentUser.token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`
        }
      });
    }

    return next.handle(request);
  }
}
