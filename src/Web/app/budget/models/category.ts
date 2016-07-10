import { BudgetLineItem } from './';

export interface Category {
    id?: string;
    name?: string;
    lineItems?: BudgetLineItem[];
}

export function calculateCategoryEstimateTotal(category: Category): number {
    return sumLineItems(category, 'estimate');
}

export function calculateCategoryActualTotal(category: Category): number {
    return sumLineItems(category, 'actual');
}

function sumLineItems(category: Category, property: string): number {
    if (!category.lineItems
        || category.lineItems.length === 0)
        return 0;

    return category.lineItems
        .map(c => c[property] ? c[property] : 0)
        .reduce((p, v) => p + v);
}