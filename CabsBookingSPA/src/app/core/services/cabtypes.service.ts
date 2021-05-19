import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CabTypes } from 'src/app/shared/models/cabTypes';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class CabtypesService {

  constructor(private apiService: ApiService) { }
  getAllCabTypes(): Observable<CabTypes[]> {
    return this.apiService.getList('cabTypes');
  }
  getCabTypesDetail(id: number): Observable<CabTypes> {
    return this.apiService.getOne(`${`cabTypes/`}${id}`);
  }
  deleteCabTypes(id : number): Observable<CabTypes> {
    return this.apiService.delete(`${`cabTypes/`}${id}`);
  }
  updateCabTypes(resource: any): Observable<CabTypes> {
    return this.apiService.update('cabTypes/edit', resource);
  }
  addCabTypes(resource: any): Observable<CabTypes> {
    return this.apiService.create('cabTypes/add', resource );
  }
}
