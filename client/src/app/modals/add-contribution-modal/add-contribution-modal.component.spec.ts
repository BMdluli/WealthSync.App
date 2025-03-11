import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddContributionModalComponent } from './add-contribution-modal.component';

describe('AddContributionModalComponent', () => {
  let component: AddContributionModalComponent;
  let fixture: ComponentFixture<AddContributionModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddContributionModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddContributionModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
