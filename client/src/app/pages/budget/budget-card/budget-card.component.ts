import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';

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
}
