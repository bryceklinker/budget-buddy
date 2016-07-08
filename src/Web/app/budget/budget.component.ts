import { ConfigService } from '../shared';
import { Budget, Category } from './';

import './styles/budget';
export class BudgetComponent implements angular.IComponentController {
    budget: Budget;

    constructor(private configService: ConfigService,
        private $http: angular.IHttpService) {

    }

    $onInit(): void {
        this.configService.getConfig()
            .then(c => this.$http.get<Budget>(`${c.budgetApiUrl}/budgets/current`))
            .then(res => this.budget = res.data);
    }

    addCategory(): void {
        if (!this.budget.categories)
            this.budget.categories = [];
        
        this.budget.categories.push({});
    }

    addLineItem(category: Category): void {
        if (!category.lineItems)
            category.lineItems = [];
        
        category.lineItems.push({});
    }

    save(): void {
        const month = this.budget.month;
        const year = this.budget.year;
        this.configService.getConfig()
            .then(c => this.$http.put(`${c.budgetApiUrl}/budgets/${month}/${year}`, this.budget));
    }
}
BudgetComponent.$inject = ['ConfigService', '$http']
angular.module('budget-buddy')
    .component('budget', {
        controller: BudgetComponent,
        template: require('./templates/budget.template')
    })