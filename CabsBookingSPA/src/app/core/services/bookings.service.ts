import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Bookings } from 'src/app/shared/models/bookings';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class BookingsService {

  constructor(private apiService: ApiService) { }
  getAllBookings(): Observable<Bookings[]> {
    return this.apiService.getList('bookings');
  }
  getBookingsDetail(id: number): Observable<Bookings> {
    return this.apiService.getOne(`${`bookings/`}${id}`);
  }
  deleteBookings(id : number): Observable<Bookings> {
    return this.apiService.delete(`${`bookings/delete/`}${id}`);
  }
  updateBookings(resource: any): Observable<Bookings> {
    return this.apiService.update('bookings/edit', resource);
  }
  addBookings(resource: any): Observable<Bookings> {
    return this.apiService.create('bookings/add', resource );
  }

}
