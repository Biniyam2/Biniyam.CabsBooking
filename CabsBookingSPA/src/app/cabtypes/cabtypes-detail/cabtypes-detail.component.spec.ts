import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CabtypesDetailComponent } from './cabtypes-detail.component';

describe('CabtypesDetailComponent', () => {
  let component: CabtypesDetailComponent;
  let fixture: ComponentFixture<CabtypesDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CabtypesDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CabtypesDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
