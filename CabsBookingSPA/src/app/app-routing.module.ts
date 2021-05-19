import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookingsHistoryAddComponent } from './bookings-history/bookings-history-add/bookings-history-add.component';
import { BookingsHistoryDetaleComponent } from './bookings-history/bookings-history-detale/bookings-history-detale.component';
import { BookingsHistoryEditComponent } from './bookings-history/bookings-history-edit/bookings-history-edit.component';
import { BookingsHistoryComponent } from './bookings-history/bookings-history.component';
import { CabtypesAddComponent } from './cabtypes/cabtypes-add/cabtypes-add.component';
import { CabtypesDetailComponent } from './cabtypes/cabtypes-detail/cabtypes-detail.component';
import { CabtypesEditComponent } from './cabtypes/cabtypes-edit/cabtypes-edit.component';
import { CabtypesComponent } from './cabtypes/cabtypes.component';
import { BookingsAddComponent } from './home/bookings-add/bookings-add.component';
import { BookingsDetailComponent } from './home/bookings-detail/bookings-detail.component';
import { BookingsEditComponent } from './home/bookings-edit/bookings-edit.component';
import { HomeComponent } from './home/home.component';
import { PlacesAddComponent } from './places/places-add/places-add.component';
import { PlacesDetailComponent } from './places/places-detail/places-detail.component';
import { PlacesEditComponent } from './places/places-edit/places-edit.component';
import { PlacesComponent } from './places/places.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';

const routes: Routes = [

  {path: "", component: HomeComponent },  
  {path: "bookings/detail/:id", component: BookingsDetailComponent },
  {path: "bookings/edit/:id", component: BookingsEditComponent },
  {path: "bookings/add", component: BookingsAddComponent },  
  {path: "places", component: PlacesComponent },
  {path: "places/detail/:id", component: PlacesDetailComponent },
  {path: "places/edit/:id", component: PlacesEditComponent },
  {path: "places/add", component: PlacesAddComponent },
  {path: "cabType", component: CabtypesComponent },
  {path: "cabType/detail/:id", component: CabtypesDetailComponent },
  {path: "cabType/edit/:id", component: CabtypesEditComponent },
  {path: "cabType/add", component: CabtypesAddComponent },
  {path: "bookingsHistory", component: BookingsHistoryComponent },
  {path: "bookingsHistory/detail/:id", component: BookingsHistoryDetaleComponent },
  {path: "bookingsHistory/edit/:id", component: BookingsHistoryEditComponent },
  {path: "bookingsHistory/add", component: BookingsHistoryAddComponent },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
