import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ApiEndPoint } from '../enum/api-end-point.enum';
import { Observable } from 'rxjs';
import { LinkedUsers } from '../models/linkeduser';

@Injectable({
  providedIn: 'root'
})
export class SettingService {

  constructor(private http: HttpClient) { }


  // getAllLinkedUsers(): Observable<LinkedUsers[]> {
  //   debugger;
  //   return this.http.get<LinkedUsers[]>(environment.apiUrl + '/' + ApiEndPoint.linkedUsers);
  // }

  getAllLinkedUsers(): Observable<LinkedUsers[]> {
    debugger;
    return this.http.get<any>(environment.apiUrl + '/' + ApiEndPoint.linkedUsers);
  }
}
