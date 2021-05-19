import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookingsService } from '../core/services/bookings.service';
import { Bookings } from '../shared/models/bookings';
import {NavigationExtras } from '@angular/router'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

 bookings : Bookings[] | undefined;
 booking : Bookings | undefined;
 id! : number;

  constructor(private bookingsService : BookingsService, private router : Router) { }

  ngOnInit(): void {

    this.bookingsService.getAllBookings().subscribe(
      b => {
        this.bookings = b;
        console.table(this.bookings);
      }
    );

  }
  ///******************* */




}
