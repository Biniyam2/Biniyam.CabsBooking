import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookingsHistoryService } from '../core/services/bookings-history.service';
import { BookingsHistory } from '../shared/models/bookingsHistory';

@Component({
  selector: 'app-bookings-history',
  templateUrl: './bookings-history.component.html',
  styleUrls: ['./bookings-history.component.css']
})
export class BookingsHistoryComponent implements OnInit {

  bookings : BookingsHistory[] | undefined;
 booking : BookingsHistory | undefined;
 id! : number;

  constructor(private bookingsService : BookingsHistoryService, private router : Router) { }

  ngOnInit(): void {

    this.bookingsService.getAllBookingsHistory().subscribe(
      b => {
        this.bookings = b;
        console.table(this.bookings);
      }
    )

  }


 


}
