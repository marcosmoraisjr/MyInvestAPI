import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewTickerFinalComponent } from './view-ticker-final.component';

describe('ViewTickerFinalComponent', () => {
  let component: ViewTickerFinalComponent;
  let fixture: ComponentFixture<ViewTickerFinalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewTickerFinalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewTickerFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
