import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpParams} from '@angular/common/http'
import {environment } from 'src/environments/environment'
import {catchError, map } from 'rxjs/operators'
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private  headers : HttpHeaders | undefined;
  constructor(protected http: HttpClient) { }

  getList(path: string): Observable<any[]>
  {
 return this.http.get(`${environment.apiUrl}${path}`).pipe(map(resp => resp as any[]))
  }
  getOne(path:string) : Observable<any>
  {

    return this.http.get(`${environment.apiUrl}${path}`).pipe(map(resp => resp as any))
  }

  create(path: string, resource: any, options?: any): Observable<any> {
    return this.http
      .post(`${environment.apiUrl}${path}`, resource, { headers: this.headers })
      .pipe(map((response: any) => response)
      );
  }

  update(path:string, resource: any, options?: any): Observable<any>
  {
    return this.http
      .put(
        `${environment.apiUrl}${path}` , resource, { headers: this.headers }
        )      
      .pipe(
        map(response => response)
      );
  }
 
  delete(path:string): Observable<any>
  {
      return this.http.delete(`${environment.apiUrl}${path}`
      ).pipe(
      map(response => response)
    );
  }

}
