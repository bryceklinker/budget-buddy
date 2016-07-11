import { 
    Budget,
    validateBudget,
    isBudgetValid
} from './budget';

describe('Budget', () => {
    let budget: Budget;

    beforeEach(() => {
        budget = { income: 34.12};
    });

    it('should have invalid income', () => {
        budget.income = undefined;

        const errors = validateBudget(budget);
        expect(errors[0].text).toBe('Income is required.');
    });

    it('should have valid income', () => {
        const errors = validateBudget(budget);
        expect(errors.length).toBe(0);
    });

    it('should not be valid without income', () => {
        budget.income = undefined;

        const isValid = isBudgetValid(budget);
        expect(isValid).toBeFalsy();
    });

    it('should not be valid with invalid categories', () => {
        budget.income = 65.34;
        budget.categories = [
            { }
        ];

        const isValid = isBudgetValid(budget);
        expect(isValid).toBeFalsy();
    });

    it('should not be valid with invalid line items', () => {
        budget.income = 43.123;
        budget.categories = [
            {
                name: 'bob',
                lineItems: [
                    {}
                ]
            }
        ];

        const isValid = isBudgetValid(budget);
        expect(isValid).toBeFalsy();
    })

    it('should be valid', () => {
        const isValid = isBudgetValid(budget);
        expect(isValid).toBeTruthy();
    })
});