import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CreateExpenseModalComponent } from '../../../modals/create-expense-modal/create-expense-modal.component';
import { BudgetService } from '../../../_services/budget.service';

@Component({
  selector: 'app-budget-card',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './budget-card.component.html',
  styleUrl: './budget-card.component.scss',
})
export class BudgetCardComponent {
  @Input() title: string = '';
  @Input() income: number = 0;
  @Input() subTitle: string = '';
  @Input() id: number = 0;

  constructor(private budgetService: BudgetService) {}

  deleteBudget() {
    this.budgetService.deleteBudget(this.id).subscribe({
      next: (_) => console.log('Budget Deleted successfully'),
      error: (err) => console.error(err),
    });
  }
}
