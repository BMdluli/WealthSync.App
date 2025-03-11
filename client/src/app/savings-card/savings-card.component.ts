import { Component, Input } from '@angular/core';
import { Goal } from '../_models/goal';
import { NgCircleProgressModule } from 'ng-circle-progress';
import { CommonModule } from '@angular/common';
import { SavingsService } from '../_services/savings.service';

@Component({
  selector: 'app-savings-card',
  standalone: true,
  imports: [NgCircleProgressModule, CommonModule],
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

  radius = 40; // Radius of the circle
  circumference = 2 * Math.PI * this.radius; // Full circle length

  get strokeDashoffset(): number {
    // Offset based on calculated percentage
    return (
      this.circumference -
      (this.calculatePercentage() / 100) * this.circumference
    );
  }

  constructor(private savingsService: SavingsService) {}

  calculatePercentage() {
    const percentage = (this.goal.currentAmount / this.goal.targetAmount) * 100;
    return percentage;
  }

  // window.location.reload()
  deleteGoal() {
    this.savingsService.deleteSavingsGoal(this.goal.id).subscribe({
      next: (_) => console.log('Goal deleted successfully'),
      error: (err) => console.error(err),
    });
  }
}
