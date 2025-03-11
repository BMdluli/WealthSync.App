import { Component, Input } from '@angular/core';
import { Goal } from '../_models/goal';
import { NgCircleProgressModule } from 'ng-circle-progress';

@Component({
  selector: 'app-savings-card',
  standalone: true,
  imports: [NgCircleProgressModule],
  templateUrl: './savings-card.component.html',
  styleUrl: './savings-card.component.scss',
})
export class SavingsCardComponent {
  @Input() goal: Goal = {
    id: 1,
    name: '',
    targetAmount: 0,
    currentAmount: 0,
    startDate: new Date(),
    targetDate: new Date(),
  };

  calculatePercentage() {
    const percentage = (this.goal.currentAmount / this.goal.targetAmount) * 100;
    return percentage;
  }
}
