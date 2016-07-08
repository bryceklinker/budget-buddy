import { Category } from './';

import './styles/category';
export class CategoryComponent implements angular.IComponentController {
    category: Category;

    getEstimateTotal(): number {
        return this.sumLineItems('estimate');
    }

    getActualTotal(): number {
        return this.sumLineItems('actual');
    }

    addLineItem(): void {
        if (!this.category.lineItems)
            this.category.lineItems = [];
        
        this.category.lineItems.push({});
    }

    private sumLineItems(property: string): number {
        if (!this.category.lineItems)
            return 0;
        
        return this.category.lineItems
            .map(c => c[property] ? c[property] : 0)
            .reduce((p, v) => p + v);
    }
}

angular.module('budget-buddy')
    .component('category', {
        controller: CategoryComponent,
        bindings: {
            'category': '='
        },
        template: require('./templates/category.template')
    });