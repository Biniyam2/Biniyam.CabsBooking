import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CabtypesComponent } from './cabtypes.component';

describe('CabtypesComponent', () => {
  let component: CabtypesComponent;
  let fixture: ComponentFixture<CabtypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CabtypesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CabtypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
