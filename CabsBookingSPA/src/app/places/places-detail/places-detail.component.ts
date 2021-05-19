import { Route } from '@angular/compiler/src/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PlacesService } from 'src/app/core/services/places.service';
import { Places } from 'src/app/shared/models/places';

@Component({
  selector: 'app-places-detail',
  templateUrl: './places-detail.component.html',
  styleUrls: ['./places-detail.component.css']
})
export class PlacesDetailComponent implements OnInit {

  place : Places | undefined;
  id! : number;
 
   constructor(private placesService : PlacesService, private router : ActivatedRoute, private route : Router) { }

  ngOnInit(): void {
    
 
    this.router.paramMap
    .subscribe(params => 
    {
      this.id = Number( params.get('id') );
      this.placesService.getPlacesDetail(this.id).subscribe(
        m => {
           this.place = m;
            }
      );
    }
    );

  }

  delete() {
    console.log('delete button was clicked');
    
  console.log(this.place);
    this.placesService.deletePlaces(this.id).subscribe( );
    this.route.navigate(['/'])
    
  }

}
