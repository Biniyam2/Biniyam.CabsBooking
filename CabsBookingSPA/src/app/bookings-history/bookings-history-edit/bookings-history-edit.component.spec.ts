import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingsHistoryEditComponent } from './bookings-history-edit.component';

describe('BookingsHistoryEditComponent', () => {
  let component: BookingsHistoryEditComponent;
  let fixture: ComponentFixture<BookingsHistoryEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookingsHistoryEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookingsHistoryEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
