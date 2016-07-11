import { ValidationError } from './validation-error';
import {
    Category,
    calculateCategoryEstimateTotal,
    calculateCategoryActualTotal,
    validateCategory,
    isCategoryValid
} from './category';

export interface Budget {
    income?: number;
    month?: number;
    year?: number;
    categories?: Category[];
}

export function calculateBudgetEstimateTotal(budget: Budget): number {
    if (!budget
        || !budget.categories
        || budget.categories.length === 0)
        return 0;

    return budget.categories
        .map(c => calculateCategoryEstimateTotal(c))
        .reduce((p, c) => p + c);
}

export function calculateBudgetActualTotal(budget: Budget): number {
    if (!budget
        || !budget.categories
        || budget.categories.length === 0)
        return 0;

    return budget.categories
        .map(c => calculateCategoryActualTotal(c))
        .reduce((p, c) => p + c);
}

export function calculateBudgetEstimateBalance(budget: Budget): number {
    if (!budget)
        return 0;

    return budget.income - calculateBudgetEstimateTotal(budget);
}

export function calculateBudgetActualBalance(budget: Budget): number {
    if (!budget)
        return 0;

    return budget.income - calculateBudgetActualTotal(budget);
}

export function validateBudget(budget: Budget): ValidationError[] {
    const errors: ValidationError[] = [];
    
    if (!budget.income) 
        errors.push({ text: 'Income is required.' });
    
    return errors;
}

export function isBudgetValid(budget: Budget): boolean {
    const budgetErrors = validateBudget(budget);
    if (budgetErrors.length > 0) 
        return false;
    
    if (!budget.categories)
        return true;

    for (let i = 0; i < budget.categories.length; i++) {
        if (!isCategoryValid(budget.categories[i]))
            return false;
    }

    return budgetErrors.length === 0;
}