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
}
