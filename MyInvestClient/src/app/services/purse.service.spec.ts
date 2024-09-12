import { TestBed } from '@angular/core/testing';

import { PurseService } from './purse.service';

describe('PurseService', () => {
  let service: PurseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PurseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
