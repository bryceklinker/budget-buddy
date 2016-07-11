import { 
    BudgetLineItem,
    isLineItemValid
} from './budget-line-item';
import { ValidationError } from './validation-error';

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

export function validateCategory(category: Category): ValidationError[] {
    const errors: ValidationError[] = [];

    if (!category.name)
        errors.push({ text: 'Name is required.' });

    return errors;
}

export function isCategoryValid(category: Category): boolean {
    const errors = validateCategory(category);
    if (errors.length > 0)
        return false;
    
    if (!category.lineItems)
        return true;

    for (var i = 0; i < category.lineItems.length; i++) {
        if (!isLineItemValid(category.lineItems[i]))
            return false;
    }

    return true;
}

function sumLineItems(category: Category, property: string): number {
    if (!category.lineItems
        || category.lineItems.length === 0)
        return 0;

    return category.lineItems
        .map(c => c[property] ? c[property] : 0)
        .reduce((p, v) => p + v);
}