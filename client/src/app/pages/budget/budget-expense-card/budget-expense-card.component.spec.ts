import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetExpenseCardComponent } from './budget-expense-card.component';

describe('BudgetExpenseCardComponent', () => {
  let component: BudgetExpenseCardComponent;
  let fixture: ComponentFixture<BudgetExpenseCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BudgetExpenseCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BudgetExpenseCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
