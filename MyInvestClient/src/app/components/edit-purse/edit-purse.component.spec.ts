import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPurseComponent } from './edit-purse.component';

describe('EditPurseComponent', () => {
  let component: EditPurseComponent;
  let fixture: ComponentFixture<EditPurseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditPurseComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditPurseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
