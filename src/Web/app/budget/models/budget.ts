import { Category, calculateCategoryEstimateTotal, calculateCategoryActualTotal } from './';

export interface Budget {
    income?: number;
    month?: number;
    year?: number;
    categories?: Category[];
}

export function calculateBudgetEstimateTotal(budget: Budget): number {
    if (!budget
        ||!budget.categories
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