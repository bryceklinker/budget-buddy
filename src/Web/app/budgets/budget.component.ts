import { Component, OnInit } from '@angular/core';

import { Budget } from './models/budget';
import { BudgetService } from './services/budget.service';

@Component({
    selector: 'budget',
    template: require('./templates/budget.template'),
    providers: [BudgetService]
})
export class BudgetComponent implements OnInit {
    private _budget: Budget;

    get budget(): Budget {
        return this._budget;
    }

    constructor(private budgetService: BudgetService) {
        this._budget = {};
    }

    ngOnInit(){
        this.budgetService.getBudget()
            .subscribe(b => this._budget = b);
    }
}