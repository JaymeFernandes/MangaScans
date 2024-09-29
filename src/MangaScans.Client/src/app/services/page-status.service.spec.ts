import { TestBed } from '@angular/core/testing';

import { PageStatusService } from './page-status.service';

describe('PageStatusService', () => {
  let service: PageStatusService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PageStatusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
