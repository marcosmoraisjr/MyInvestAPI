import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  configureLocalStorage(body: any): void 
  {
    if (body) 
    {
      localStorage.setItem('userId', body.user_Id);
    }
  }

  verifyIfUserIdLogged() : boolean
  {
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      var userId = localStorage.getItem('userId');
      if (userId != null) {
        return true;
      };
    }
    return false;
  }

  getId() {
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      const userId = localStorage.getItem('userId');
      return userId !== null ? userId : '';
    }
    return ''; 
  }
}
