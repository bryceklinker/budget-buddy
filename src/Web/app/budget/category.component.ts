import {
    Budget,
    BudgetLineItem, 
    Category, 
    calculateCategoryActualTotal, 
    calculateCategoryEstimateTotal,
    calculatePercentOfIncome
} from './models';

import './styles/category';
export class CategoryComponent implements angular.IComponentController {
    category: Category;
    budget: Budget;

    calculatePercentOfIncome(lineItem?: BudgetLineItem): number {
        if (lineItem) {
            return calculatePercentOfIncome(lineItem.actual, this.budget);    
        }
        
        return calculatePercentOfIncome(this.getActualTotal(), this.budget);
    }

    getEstimateTotal(): number {
        return calculateCategoryEstimateTotal(this.category);
    }

    getActualTotal(): number {
        return calculateCategoryActualTotal(this.category);
    }

    addLineItem(): void {
        if (!this.category.lineItems)
            this.category.lineItems = [];
        
        this.category.lineItems.push({});
    }

    deleteLineItem(lineItem: BudgetLineItem) {
        const index = this.category.lineItems.indexOf(lineItem);
        if (index > -1) {
            this.category.lineItems.splice(index, 1);    
        }
    }
}

angular.module('budget-buddy')
    .component('category', {
        controller: CategoryComponent,
        bindings: {
            'category': '=',
            'budget': '='
        },
        template: require('./templates/category.template')
    });