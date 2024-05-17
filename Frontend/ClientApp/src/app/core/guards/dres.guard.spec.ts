import { TestBed } from '@angular/core/testing';

import { DresGuard } from './dres.guard';

describe('DresGuard', () => {
  let guard: DresGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(DresGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
