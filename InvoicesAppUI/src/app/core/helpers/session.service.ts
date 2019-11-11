import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  currentToken: string;

  constructor() { }



  login(data: any) {
    debugger;
    const user = JSON.stringify(data);
    const token = data.user_info.accessToken;
    localStorage.setItem('_currentToken', token);
    console.log("ewfhbewhbfewhbfbwewefbjwe", token);
  }

  getToken(): string {
    debugger;
    const token = localStorage.getItem('_currentToken');
    if (token) {
      this.currentToken = token;
    }

    return this.currentToken;
  }

  logout() {
    localStorage.removeItem('_currentToken');
  }
}
