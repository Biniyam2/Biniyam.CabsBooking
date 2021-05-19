import { TestBed } from '@angular/core/testing';

import { CabtypesService } from './cabtypes.service';

describe('CabtypesService', () => {
  let service: CabtypesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CabtypesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
