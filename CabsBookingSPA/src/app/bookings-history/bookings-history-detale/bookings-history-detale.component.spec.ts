import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingsHistoryDetaleComponent } from './bookings-history-detale.component';

describe('BookingsHistoryDetaleComponent', () => {
  let component: BookingsHistoryDetaleComponent;
  let fixture: ComponentFixture<BookingsHistoryDetaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookingsHistoryDetaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookingsHistoryDetaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
