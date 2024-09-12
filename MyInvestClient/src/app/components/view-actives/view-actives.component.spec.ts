import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewActivesComponent } from './view-actives.component';

describe('ViewActivesComponent', () => {
  let component: ViewActivesComponent;
  let fixture: ComponentFixture<ViewActivesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewActivesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewActivesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
