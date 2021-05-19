import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingsHistoryService } from 'src/app/core/services/bookings-history.service';
import { BookingsHistory } from 'src/app/shared/models/bookingsHistory';

@Component({
  selector: 'app-bookings-history-edit',
  templateUrl: './bookings-history-edit.component.html',
  styleUrls: ['./bookings-history-edit.component.css']
})
export class BookingsHistoryEditComponent implements OnInit {

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
  constructor(private bookingsService : BookingsHistoryService, private route : Router , private router : ActivatedRoute) { }

  ngOnInit(): void {
    this.router.paramMap
    .subscribe(params => 
    {
      this.id = Number( params.get('id') );
      this.bookingsService.getBookingsHistoryDetail(this.id).subscribe(
        m => {
           this.booking = m;
            }
      );
    }
    );
    
  }


  edit() {
    console.log('button was clicked');
    this.bookingsService.updateBookingsHistory(this.booking).subscribe();
    this.route.navigate(['/']);
    console.log(this.booking);
  }

}
