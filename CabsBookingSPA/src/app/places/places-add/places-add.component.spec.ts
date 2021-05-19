import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlacesAddComponent } from './places-add.component';

describe('PlacesAddComponent', () => {
  let component: PlacesAddComponent;
  let fixture: ComponentFixture<PlacesAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlacesAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlacesAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
