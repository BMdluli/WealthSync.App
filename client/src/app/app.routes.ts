import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { SavingsComponent } from './pages/savings/savings.component';
import { BudgetComponent } from './pages/budget/budget.component';
import { StocksComponent } from './pages/stocks/stocks.component';
import { HomeComponent } from './pages/home/home.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'savings', component: SavingsComponent },
  { path: 'budget', component: BudgetComponent },
  { path: 'stocks', component: StocksComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', component: NotFoundComponent },
];
