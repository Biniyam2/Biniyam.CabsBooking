import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CabtypesService } from 'src/app/core/services/cabtypes.service';
import { CabTypes } from 'src/app/shared/models/cabTypes';

@Component({
  selector: 'app-cabtypes-edit',
  templateUrl: './cabtypes-edit.component.html',
  styleUrls: ['./cabtypes-edit.component.css']
})
export class CabtypesEditComponent implements OnInit {

  cabtype : CabTypes = {
    cabTypeName: '',
    cabTypeId: 0
  };
  id! : number;
  constructor(private cabtypesService : CabtypesService, private route : Router,  private router : ActivatedRoute) { }

  ngOnInit(): void {

    this.router.paramMap
    .subscribe(params => 
    {
      this.id = Number( params.get('id') );
      this.cabtypesService.getCabTypesDetail(this.id).subscribe(
        m => {
           this.cabtype = m;
            }
      );
    }
    );
    
  }
  edit() {
    console.log('button was clicked');
    this.cabtypesService.updateCabTypes(this.cabtype).subscribe();
    this.route.navigate(['/']);
    console.log(this.cabtype);
  }

}
