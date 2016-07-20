import { 
    Budget,
    calculateBudgetActualTotal, 
    calculateBudgetEstimateTotal, 
    calculateBudgetEstimateBalance, 
    calculateBudgetActualBalance,
    calculatePercentOfIncome
} from './models';

export class BudgetTotalsComponent implements angular.IComponentController {
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

    get percentOfIncome(): number {
        return calculatePercentOfIncome(this.actualTotal, this.budget);
    }

    $onInit(): void {
        
    }
}
angular.module('budget-buddy')
    .component('budgetTotals', {
        bindings: {
            budget: '='
        },
        controller: BudgetTotalsComponent,
        template: require('./templates/budget-totals.template')
    })