import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePurseComponent } from './create-purse.component';

describe('CreatePurseComponent', () => {
  let component: CreatePurseComponent;
  let fixture: ComponentFixture<CreatePurseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreatePurseComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreatePurseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
