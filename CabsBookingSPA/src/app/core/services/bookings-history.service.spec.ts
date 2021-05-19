import { TestBed } from '@angular/core/testing';

import { BookingsHistoryService } from './bookings-history.service';

describe('BookingsHistoryService', () => {
  let service: BookingsHistoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookingsHistoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
