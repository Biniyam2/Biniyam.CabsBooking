import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookingsHistoryService } from 'src/app/core/services/bookings-history.service';
import { BookingsHistory } from 'src/app/shared/models/bookingsHistory';

@Component({
  selector: 'app-bookings-history-add',
  templateUrl: './bookings-history-add.component.html',
  styleUrls: ['./bookings-history-add.component.css']
})
export class BookingsHistoryAddComponent implements OnInit {

  booking : BookingsHistory = {
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
    status: '',
    comp_time: '',
    charge: 0,
    feedback: ''
  };
  id! : number;
  constructor(private bookingsService : BookingsHistoryService, private router : Router) { }

  ngOnInit(): void {
  }

  add() {
    console.log('button was clicked');
    this.bookingsService.addBookingsHistory(this.booking).subscribe();
    this.router.navigate(['/bookingsHistory']);
    console.log(this.booking);
  }



}
