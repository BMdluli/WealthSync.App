import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { SavingsComponent } from './pages/savings/savings.component';
import { BudgetComponent } from './pages/budget/budget.component';
import { StocksComponent } from './pages/stocks/stocks.component';
import { HomeComponent } from './pages/home/home.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import { BudgetCategoryComponent } from './pages/budget/budget-category/budget-category.component';
import { authGuard } from './auth-guard.guard';
import { ServerErrorComponent } from './pages/server-error/server-error.component';

export const routes: Routes = [
  { path: '', component: HomeComponent }, // Public
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard],
  },
  { path: 'savings', component: SavingsComponent, canActivate: [authGuard] },
  { path: 'budget', component: BudgetComponent, canActivate: [authGuard] },
  {
    path: 'budget/:id',
    component: BudgetCategoryComponent,
    canActivate: [authGuard],
  },
  { path: 'stocks', component: StocksComponent, canActivate: [authGuard] },
  { path: 'register', component: RegisterComponent }, // Public
  { path: 'login', component: LoginComponent }, // Public
  { path: 'server-error', component: ServerErrorComponent }, // Public
  { path: '**', component: NotFoundComponent, canActivate: [authGuard] }, // Protected
];
