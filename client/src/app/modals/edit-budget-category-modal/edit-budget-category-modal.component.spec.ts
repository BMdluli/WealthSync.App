import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditBudgetCategoryModalComponent } from './edit-budget-category-modal.component';

describe('EditBudgetCategoryModalComponent', () => {
  let component: EditBudgetCategoryModalComponent;
  let fixture: ComponentFixture<EditBudgetCategoryModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditBudgetCategoryModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditBudgetCategoryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
