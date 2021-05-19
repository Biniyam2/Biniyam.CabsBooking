import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PlacesService } from 'src/app/core/services/places.service';
import { Places } from 'src/app/shared/models/places';

@Component({
  selector: 'app-places-edit',
  templateUrl: './places-edit.component.html',
  styleUrls: ['./places-edit.component.css']
})
export class PlacesEditComponent implements OnInit {

 
  place : Places  = {
    placeName: '',
    placeId: 1
  };
   id!: number ;

  constructor(private placesService : PlacesService, private route : Router, private router : ActivatedRoute) { }

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
  edit() {
    this.place.placeId = this.id;
    console.log('button was clicked');
    this.placesService.updatePlaces(this.place).subscribe();
    this.route.navigate(['/places']);
    console.log(this.place);
  }

}
