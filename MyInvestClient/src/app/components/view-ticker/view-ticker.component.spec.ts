import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewTickerComponent } from './view-ticker.component';

describe('ViewTickerComponent', () => {
  let component: ViewTickerComponent;
  let fixture: ComponentFixture<ViewTickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewTickerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewTickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
