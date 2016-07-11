import { 
    BudgetLineItem,
    validateBudgetLineItem
} from './budget-line-item';

describe('BudgetLineItem', () => {
    let budgetLineItem: BudgetLineItem;

    beforeEach(() => {
        budgetLineItem = {
            name: 'valid',
            estimate: 235.4
        };
    });

    it('should have invalid name', () => {
        budgetLineItem.name = undefined;

        const errors = validateBudgetLineItem(budgetLineItem);
        expect(errors[0].text).toBe('Name is required.');
    });

    it('should have invalid estimate', () => {
        budgetLineItem.estimate = undefined;

        const errors = validateBudgetLineItem(budgetLineItem);
        expect(errors[0].text).toBe('Estimate is required.');
    });

    it('should be valid', () => {
        const errors = validateBudgetLineItem(budgetLineItem);
        expect(errors.length).toBe(0);
    })
});