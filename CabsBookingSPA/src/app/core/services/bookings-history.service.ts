import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookingsHistory } from 'src/app/shared/models/bookingsHistory';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class BookingsHistoryService {

  constructor(private apiService: ApiService) { }
  getAllBookingsHistory(): Observable<BookingsHistory[]> {
    return this.apiService.getList('bookingsHistory');
  }
  getBookingsHistoryDetail(id: number): Observable<BookingsHistory> {
    return this.apiService.getOne(`${`bookingsHistory/`}${id}`);
  }
  deleteBookingsHistory(id : number): Observable<BookingsHistory> {
    return this.apiService.delete(`${`bookingsHistory/delete/`}${id}`);
  }
  updateBookingsHistory(resource: any): Observable<BookingsHistory> {
    return this.apiService.update('bookingsHistory/edit', resource);
  }
  addBookingsHistory(resource: any): Observable<BookingsHistory> {
    return this.apiService.create('bookingsHistory/add', resource );
  }
}
