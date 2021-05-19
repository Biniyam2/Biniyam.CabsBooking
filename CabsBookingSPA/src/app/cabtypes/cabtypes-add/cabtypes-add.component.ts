import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CabtypesService } from 'src/app/core/services/cabtypes.service';
import { CabTypes } from 'src/app/shared/models/cabTypes';

@Component({
  selector: 'app-cabtypes-add',
  templateUrl: './cabtypes-add.component.html',
  styleUrls: ['./cabtypes-add.component.css']
})
export class CabtypesAddComponent implements OnInit {

  cabtype : CabTypes = {
    cabTypeName: '',
    cabTypeId: 0
  };
  id! : number;
  constructor(private cabtypesService : CabtypesService, private router : Router) { }

  ngOnInit(): void {
  }

  add() {
    console.log('button was clicked');
    this.cabtypesService.addCabTypes(this.cabtype).subscribe();
    this.router.navigate(['/cabType']);
    console.log(this.cabtype);
  }

}
