import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingsService } from 'src/app/core/services/bookings.service';
import { Bookings } from 'src/app/shared/models/bookings';

@Component({
  selector: 'app-bookings-detail',
  templateUrl: './bookings-detail.component.html',
  styleUrls: ['./bookings-detail.component.css']
})
export class BookingsDetailComponent implements OnInit {

 booking : Bookings | undefined;
  id! : number;
  
  constructor(private route: ActivatedRoute, private bookingsService : BookingsService, private router: Router) { }

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


  delete() {
      console.log('button was clicked ');
      console.log(this.id);
      this.bookingsService.deleteBookings(this.id).subscribe( );
      this.router.navigate(['/'])
  
    }

}
