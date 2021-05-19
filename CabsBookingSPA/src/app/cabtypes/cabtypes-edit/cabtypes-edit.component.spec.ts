import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CabtypesEditComponent } from './cabtypes-edit.component';

describe('CabtypesEditComponent', () => {
  let component: CabtypesEditComponent;
  let fixture: ComponentFixture<CabtypesEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CabtypesEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CabtypesEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
