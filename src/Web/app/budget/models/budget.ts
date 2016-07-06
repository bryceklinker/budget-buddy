import { Category } from './';

export interface Budget {
    income?: number;
    month?: number;
    year?: number;
    categories?: Category[];
}