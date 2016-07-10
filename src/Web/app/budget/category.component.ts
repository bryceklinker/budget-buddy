import { Category, calculateCategoryActualTotal, calculateCategoryEstimateTotal } from './';

import './styles/category';
export class CategoryComponent implements angular.IComponentController {
    category: Category;
    isCollapsed: boolean;

    get collapseText(): string {
        return this.isCollapsed
            ? 'Expand'
            : 'Collapse';
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
        this.isCollapsed = false;
    }

    toggleCollapse(): void {
        this.isCollapsed = !this.isCollapsed;
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