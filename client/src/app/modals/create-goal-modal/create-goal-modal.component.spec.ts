import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateGoalModalComponent } from './create-goal-modal.component';

describe('CreateGoalModalComponent', () => {
  let component: CreateGoalModalComponent;
  let fixture: ComponentFixture<CreateGoalModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateGoalModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateGoalModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
