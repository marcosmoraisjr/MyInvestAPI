import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { development_environments } from '../environments/development-environments';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActiveService {

  url: string = development_environments.url;

  constructor(
    private http: HttpClient
  ) { }

  search(active: string, percentage: number): Observable<any>
  {
    const urlForRequest = this.url + `/search-active/${active}/${percentage}`;
    return this.http.get(urlForRequest, { observe: 'response' });
  }

  create(purseId: string, type: string, code: string, dYDesiredPercentage: string): Observable<any>
  {
    const urlForRequest = this.url + "/active";

    const objForRequest = {
      type: type,
      purse_Id: purseId,
      code: code,
      dyDesiredPercentage: dYDesiredPercentage
    }
    return this.http.post(urlForRequest, objForRequest, { observe: 'response' });
  }

  searchActivesByPurseId(purseId: string): Observable<any>
  {
    const urlForRequest = this.url + "/get-actives/" + purseId;
    return this.http.get(urlForRequest, { observe: 'response' });
  }

  delete(purseId: string): Observable<any>
  {
    const urlForRequest = this.url + "/active/" + purseId;
    console.log(urlForRequest)
    return this.http.delete(urlForRequest, { observe: 'response' });
  }

  update(activeId: string, dYDesiredPercentage: number): Observable<any>
  {
    const urlForRequest = this.url + "/active/" + activeId;
    console.log(urlForRequest);

    const objForRequest = {
      dyDesiredPercentage: dYDesiredPercentage
    }
    return this.http.put(urlForRequest, objForRequest, { observe: 'response' });
  }
}
