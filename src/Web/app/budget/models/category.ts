import { BudgetLineItem } from './';

export interface Category {
    id?: string;
    name?: string;
    lineItems?: BudgetLineItem[];
}