import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from '../../../sidebar/sidebar.component';
import { HeaderComponent } from '../../../header/header.component';
import { BudgetCategoryService } from '../../../_services/budget-category.service';
import { ActivatedRoute } from '@angular/router';
import { BudgetCategory } from '../../../_models/budgetCategory';
import { CommonModule } from '@angular/common';
import { ModalService } from '../../../_services/modal.service';
import { CreateBudgetCategoryModalComponent } from '../../../modals/create-budget-category-modal/create-budget-category-modal.component';
import { BudgetExpenseCardComponent } from '../budget-expense-card/budget-expense-card.component';

@Component({
  selector: 'app-budget-category',
  standalone: true,
  imports: [
    HeaderComponent,
    CommonModule,
    CreateBudgetCategoryModalComponent,
    BudgetExpenseCardComponent,
  ],
  templateUrl: './budget-category.component.html',
  styleUrl: './budget-category.component.scss',
})
export class BudgetCategoryComponent implements OnInit {
  id: string | null = '';
  budgetCategories: BudgetCategory[] = [];
  isBudgetModalOpen = false;
  isExpenseModalOpen = false;
  isLoading = false;

  constructor(
    private budgetCategoryService: BudgetCategoryService,
    private route: ActivatedRoute,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getBudgetCategories();
    this.modalService
      .getModalState('budgetCategoryModal')
      .subscribe((isOpen) => {
        this.isBudgetModalOpen = isOpen;
      });
    this.modalService.getModalState('expenseModal').subscribe((isOpen) => {
      this.isExpenseModalOpen = isOpen;
    });
  }

  getBudgetCategories() {
    this.isLoading = true;
    this.budgetCategoryService.getBudgetCategories(this.id!).subscribe({
      next: (response) => (this.budgetCategories = response),
      error: (err) => console.error(err),
      complete: () => (this.isLoading = false),
    });
  }

  openBudgetModal() {
    this.modalService.openModal('budgetCategoryModal');
  }
}
