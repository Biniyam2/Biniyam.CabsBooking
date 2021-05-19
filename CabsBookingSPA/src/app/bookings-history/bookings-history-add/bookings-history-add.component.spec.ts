import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingsHistoryAddComponent } from './bookings-history-add.component';

describe('BookingsHistoryAddComponent', () => {
  let component: BookingsHistoryAddComponent;
  let fixture: ComponentFixture<BookingsHistoryAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookingsHistoryAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookingsHistoryAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
