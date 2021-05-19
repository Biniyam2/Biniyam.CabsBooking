import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Places } from 'src/app/shared/models/places';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class PlacesService {

  constructor(private apiService: ApiService) { }
  getAllPlaces(): Observable<Places[]> {
    return this.apiService.getList('places');
  }
  getPlacesDetail(id: number): Observable<Places> {
    return this.apiService.getOne(`${`places/`}${id}`);
  }
  deletePlaces(id : number): Observable<Places> {
    return this.apiService.delete( `${`places/delete/`}${id}`);
  }
  updatePlaces(resource: any): Observable<Places> {
    return this.apiService.update('places/edit', resource);
  }
  addPlaces(resource: any): Observable<Places> {
    return this.apiService.create('places/add', resource );
  }
}

