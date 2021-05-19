import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PlacesService } from 'src/app/core/services/places.service';
import { Places } from 'src/app/shared/models/places';

@Component({
  selector: 'app-places-add',
  templateUrl: './places-add.component.html',
  styleUrls: ['./places-add.component.css']
})
export class PlacesAddComponent implements OnInit {

  place : Places  = {
    placeName: '',
    placeId: 0
  };
  id! : number;

  constructor(private placesService : PlacesService, private router : Router) { }

  ngOnInit(): void {
  }

  add() {
    console.log('button was clicked');
    this.placesService.addPlaces(this.place).subscribe();
    this.router.navigate(['/']);
    console.log(this.place);
  }

}
