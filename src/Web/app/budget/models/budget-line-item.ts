import { ValidationError } from './validation-error';

export interface BudgetLineItem {
    actual?: number;
    estimate?: number;
    name?: string;
    id?: string;
}

export function validateBudgetLineItem(budgetLineItem: BudgetLineItem): ValidationError[] {
    const errors: ValidationError[] = [];

    if (!budgetLineItem.name)
        errors.push({ text: 'Name is required.' });
    
    if (!budgetLineItem.estimate) 
        errors.push({ text: 'Estimate is required.' });

    return errors;
}

export function isLineItemValid(budgetLineItem: BudgetLineItem): boolean {
    return validateBudgetLineItem(budgetLineItem).length === 0;
}