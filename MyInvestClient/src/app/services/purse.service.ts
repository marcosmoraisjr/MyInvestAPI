import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { development_environments } from '../environments/development-environments';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PurseService {

  constructor(
    private http: HttpClient
  ) { }

  url: string = development_environments.url + "/purse";

  create(body: any): Observable<any>
  {
    return this.http.post(this.url, body, { observe: 'response'});
  }

  delete(id: any): Observable<any>
  {
    const urlForRequest = this.url + "/" + id;
    return this.http.delete(urlForRequest, {observe: 'response'});
  }

  getById(id: any): Observable<any>
  {
    const urlForRequest = this.url + "/" + id;
    return this.http.get(urlForRequest, { observe: 'response' });
  }

  update(body: any): Observable<any>
  {
    const urlForRequest = this.url + "/" + body.user_Id;
    return this.http.put(urlForRequest, body, {observe: 'response'});
  }
}
