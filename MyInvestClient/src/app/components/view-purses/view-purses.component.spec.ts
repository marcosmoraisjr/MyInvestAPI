import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewPursesComponent } from './view-purses.component';

describe('ViewPursesComponent', () => {
  let component: ViewPursesComponent;
  let fixture: ComponentFixture<ViewPursesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewPursesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewPursesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
