import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PlacesService } from '../core/services/places.service';
import { Places } from '../shared/models/places';

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.css']
})
export class PlacesComponent implements OnInit {

  places : Places[] | undefined;
  place : Places | undefined;
  deleted : Places  = {
    placeName: '',
    placeId: 1
  };
 id! : number;

  constructor(private placesService : PlacesService, private router : Router) { }

  ngOnInit(): void {
    
    this.placesService.getAllPlaces().subscribe(
      p => {
        this.places = p;
        console.table(this.places);
      }
    )

  }
  ///******************* */
  
  

}
