import { Component } from '@angular/core';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-budget',
  standalone: true,
  imports: [SidebarComponent, HeaderComponent],
  templateUrl: './budget.component.html',
  styleUrl: './budget.component.scss',
})
export class BudgetComponent {}
