import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NavigationComponent } from './navigation/navigation.component';
import { BudgetComponent } from './budgets/budget.component';
import { BudgetRoute } from './budgets/budget.route';

@Component({
    selector: 'app',
    template: require('./app.template'),
    directives: [RouterOutlet, NavigationComponent]
})
export class AppComponent {

}