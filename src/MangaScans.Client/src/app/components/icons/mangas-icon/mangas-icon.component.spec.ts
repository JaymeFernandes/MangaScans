import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MangasIconComponent } from './mangas-icon.component';

describe('MangasIconComponent', () => {
  let component: MangasIconComponent;
  let fixture: ComponentFixture<MangasIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MangasIconComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MangasIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
