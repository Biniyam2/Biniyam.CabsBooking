import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingsHistoryService } from 'src/app/core/services/bookings-history.service';
import { BookingsHistory } from 'src/app/shared/models/bookingsHistory';

@Component({
  selector: 'app-bookings-history-detale',
  templateUrl: './bookings-history-detale.component.html',
  styleUrls: ['./bookings-history-detale.component.css']
})


export class BookingsHistoryDetaleComponent implements OnInit {

    booking : BookingsHistory | undefined;
    id! : number;
     
     constructor(private route: ActivatedRoute, private bookingsService : BookingsHistoryService,private router: Router,) { }
   
     ngOnInit(): void {
      
   
       this.route.paramMap
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

     delete() {
      console.log('button was clicked');
  
      this.bookingsService.deleteBookingsHistory(this.id).subscribe( );
      this.router.navigate(['/'])
  
    }

}
