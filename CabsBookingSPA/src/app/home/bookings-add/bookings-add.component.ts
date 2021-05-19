import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookingsService } from 'src/app/core/services/bookings.service';
import { Bookings } from 'src/app/shared/models/bookings';

@Component({
  selector: 'app-bookings-add',
  templateUrl: './bookings-add.component.html',
  styleUrls: ['./bookings-add.component.css']
})
export class BookingsAddComponent implements OnInit {

  booking : Bookings = {
    email: '',
    places:'',
    cabTypes: '',
    id: 1 ,
    bookingDate: new Date("2099-12-30") ,
    bookingTime: '',
    fromPlace: 0,
    toPlace: 1,
    pickupAddress: '',
    landMark: '',
    pickupDate: new Date("2099-12-30"),
    pickupTime: '',
    cabTypesId: 1, 
    contactNo: '',
    status: ''
  };
  id! : number;
  constructor(private bookingsService : BookingsService, private router : Router) { }

  ngOnInit(): void {
  }

  add() {
    console.log('button was clicked');
    this.bookingsService.addBookings(this.booking).subscribe();
    this.router.navigate(['/']);
    console.log(this.booking);
  }


}
