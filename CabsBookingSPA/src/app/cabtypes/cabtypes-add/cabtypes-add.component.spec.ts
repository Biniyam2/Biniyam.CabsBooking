import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CabtypesAddComponent } from './cabtypes-add.component';

describe('CabtypesAddComponent', () => {
  let component: CabtypesAddComponent;
  let fixture: ComponentFixture<CabtypesAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CabtypesAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CabtypesAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
