import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBudgetCategoryModalComponent } from './create-budget-category-modal.component';

describe('CreateBudgetCategoryModalComponent', () => {
  let component: CreateBudgetCategoryModalComponent;
  let fixture: ComponentFixture<CreateBudgetCategoryModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateBudgetCategoryModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateBudgetCategoryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
