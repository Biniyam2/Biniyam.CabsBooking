import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CabtypesService } from 'src/app/core/services/cabtypes.service';
import { CabTypes } from 'src/app/shared/models/cabTypes';

@Component({
  selector: 'app-cabtypes-detail',
  templateUrl: './cabtypes-detail.component.html',
  styleUrls: ['./cabtypes-detail.component.css']
})
export class CabtypesDetailComponent implements OnInit {

  cabType : CabTypes | undefined;
  id! : number;
   
   constructor(private cabtypesService : CabtypesService, private router : ActivatedRoute,  private route : Router) { }
 
   ngOnInit(): void {
    
 
     this.router.paramMap
     .subscribe(params => 
     {
       this.id = Number( params.get('id') );
       this.cabtypesService.getCabTypesDetail(this.id).subscribe(
         m => {
            this.cabType = m;
             }
       );
     }
     );
 
   }

   delete() {
    console.log('button was clicked');

    this.cabtypesService.deleteCabTypes(this.id).subscribe( );
    this.route.navigate(['/'])

  }

}
