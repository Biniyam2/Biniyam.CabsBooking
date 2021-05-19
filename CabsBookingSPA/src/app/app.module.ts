;
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { PlacesComponent } from './places/places.component';
import { CabtypesComponent } from './cabtypes/cabtypes.component';
import { BookingsHistoryComponent } from './bookings-history/bookings-history.component';
import { BookingsDetailComponent } from './home/bookings-detail/bookings-detail.component';
import { BookingsAddComponent } from './home/bookings-add/bookings-add.component';
import { BookingsEditComponent } from './home/bookings-edit/bookings-edit.component';
import { BookingsHistoryEditComponent } from './bookings-history/bookings-history-edit/bookings-history-edit.component';
import { BookingsHistoryAddComponent } from './bookings-history/bookings-history-add/bookings-history-add.component';
import { BookingsHistoryDetaleComponent } from './bookings-history/bookings-history-detale/bookings-history-detale.component';
import { CabtypesDetailComponent } from './cabtypes/cabtypes-detail/cabtypes-detail.component';
import { CabtypesAddComponent } from './cabtypes/cabtypes-add/cabtypes-add.component';
import { CabtypesEditComponent } from './cabtypes/cabtypes-edit/cabtypes-edit.component';
import { PlacesEditComponent } from './places/places-edit/places-edit.component';
import { PlacesAddComponent } from './places/places-add/places-add.component';
import { PlacesDetailComponent } from './places/places-detail/places-detail.component';
import { HeaderComponent } from './core/guards/layout/header/header.component';
import { FooterComponent } from './core/guards/layout/footer/footer.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PlacesComponent,
    CabtypesComponent,
    BookingsHistoryComponent,
    BookingsDetailComponent,
    BookingsAddComponent,
    BookingsEditComponent,
    BookingsHistoryEditComponent,
    BookingsHistoryAddComponent,
    BookingsHistoryDetaleComponent,
    CabtypesDetailComponent,
    CabtypesAddComponent,
    CabtypesEditComponent,
    PlacesEditComponent,
    PlacesAddComponent,
    PlacesDetailComponent,
    HeaderComponent,
    FooterComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    JwtModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
