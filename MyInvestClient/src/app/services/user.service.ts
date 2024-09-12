import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { development_environments } from '../environments/development-environments';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient
  ) { }

  url: string = development_environments.url + "/user";

  create(data: any): Observable<any>
  {
    return this.http.post(this.url, data, {observe: 'response'});
  }

  getPurses(userId: any): Observable<any>
  {
    const urlForRequest = this.url + `/${userId}/purses`;
    return this.http.get(urlForRequest, {observe: 'response'});
  }
}
