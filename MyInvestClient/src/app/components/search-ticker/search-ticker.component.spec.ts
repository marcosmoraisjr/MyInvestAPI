import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchTickerComponent } from './search-ticker.component';

describe('SearchTickerComponent', () => {
  let component: SearchTickerComponent;
  let fixture: ComponentFixture<SearchTickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchTickerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SearchTickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
