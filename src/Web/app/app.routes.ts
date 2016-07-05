import { provideRouter, RouterConfig } from '@angular/router';

import { BudgetRoute } from './budgets/budget.route';

export const routes: RouterConfig = [
    BudgetRoute
]

export const APP_ROUTER_PROVIDERS = [
    provideRouter(routes)
];