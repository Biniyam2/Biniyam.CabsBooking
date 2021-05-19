import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CabtypesService } from '../core/services/cabtypes.service';
import { CabTypes} from '../shared/models/cabTypes'

@Component({
  selector: 'app-cabtypes',
  templateUrl: './cabtypes.component.html',
  styleUrls: ['./cabtypes.component.css']
})
export class CabtypesComponent implements OnInit {

  cabtypes : CabTypes[] | undefined;
  cabtype : CabTypes | undefined;
 id! : number;

  constructor(private cabtypesService : CabtypesService, private router : Router) { }

  ngOnInit(): void {

    this.cabtypesService.getAllCabTypes().subscribe(
      b => {
        this.cabtypes = b;
        console.table(this.cabtypes);
      }
    )

  }
  ///******************* */
  

}
