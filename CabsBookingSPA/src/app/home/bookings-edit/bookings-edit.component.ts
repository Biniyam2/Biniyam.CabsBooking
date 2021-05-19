import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingsService } from 'src/app/core/services/bookings.service';
import { Bookings } from 'src/app/shared/models/bookings';

@Component({
  selector: 'app-bookings-edit',
  templateUrl: './bookings-edit.component.html',
  styleUrls: ['./bookings-edit.component.css']
})
export class BookingsEditComponent implements OnInit {
 // booking : Bookings | undefined;
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
 
  constructor(private bookingsService : BookingsService, private router : Router, private route : ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap
    .subscribe(params => 
    {
      this.id = Number( params.get('id') );
      this.bookingsService.getBookingsDetail(this.id).subscribe(
        m => {
           this.booking = m;
            }
      );
    }
    );
    
  }


  edit() {
    console.log('button was clicked');
    this.bookingsService.updateBookings(this.booking).subscribe();
    
    this.router.navigate(['/']);
    console.log(this.booking);
  }


}
