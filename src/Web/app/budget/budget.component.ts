import { ConfigService } from '../shared';
import { 
    Budget, 
    Category,
    calculateBudgetActualTotal, 
    calculateBudgetEstimateTotal, 
    calculateBudgetEstimateBalance, 
    calculateBudgetActualBalance 
} from './';

import './styles/budget';
export class BudgetComponent implements angular.IComponentController {
    budget: Budget;
    
    get estimateTotal(): number {
        return calculateBudgetEstimateTotal(this.budget);
    }

    get actualTotal(): number {
        return calculateBudgetActualTotal(this.budget);
    }

    get estimateBalance(): number {
        return calculateBudgetEstimateBalance(this.budget);
    }

    get actualBalance(): number {
        return calculateBudgetActualBalance(this.budget);
    }

    constructor(private configService: ConfigService,
        private $http: angular.IHttpService) {

    }

    $onInit(): void {
        this.configService.getConfig()
            .then(c => this.$http.get<Budget>(`${c.budgetApiUrl}/budgets/current`))
            .then(res => this.budget = res.data);
    }

    getCategoryHeading(category: Category): string {
        if (!category.name)
            return 'New Category';
        
        return category.name;
    }

    addCategory(): void {
        if (!this.budget.categories)
            this.budget.categories = [];
        
        this.budget.categories.push({});
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
    });