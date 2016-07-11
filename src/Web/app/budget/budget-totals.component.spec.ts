import { Budget, Category } from './models';
import { BudgetTotalsComponent  } from './budget-totals.component';

describe('BudgetTotalsComponent', () => {
    let budget: Budget;
    let componentOptions: angular.IDirective;
    let budgetTotalsComponent: BudgetTotalsComponent;

    beforeEach(angular.mock.inject((_$controller_, _budgetTotalsDirective_) => {
        budget = {};
        componentOptions = _budgetTotalsDirective_[0];
        budgetTotalsComponent = _$controller_(BudgetTotalsComponent);
        budgetTotalsComponent.budget = budget;
    }));

    it('should use budget totals component as controller', () => {
        expect(componentOptions.controller).toBe(BudgetTotalsComponent);
    });

    it('should use budget totals template', () => {
        expect(componentOptions.template).toBe(require('./templates/budget-totals.template'));
    });

    it('should specify budget as binding', () => {
        expect(componentOptions.bindToController['budget']).toBe('=');
    })

    it('should sum up line item estimates', () => {
        budget.categories = createCategories();

        budgetTotalsComponent.$onInit();

        expect(budgetTotalsComponent.estimateTotal).toBe(300.88);
    });

    it('should sum up line item actuals', () => {
        budget.categories = createCategories();

        budgetTotalsComponent.$onInit();

        expect(budgetTotalsComponent.actualTotal).toBe(411.74)
    });

    it('should have zero actual total', () => {
        budget.categories = undefined;

        budgetTotalsComponent.$onInit();

        expect(budgetTotalsComponent.actualTotal).toBe(0);
    });

    it('should have zero estimate total', () => {
        budget.categories = undefined;

        budgetTotalsComponent.$onInit();

        expect(budgetTotalsComponent.estimateTotal).toBe(0);
    });

    it('should get estimate balance', () => {
        budget.categories = createCategories();
        budget.income = 234.5;

        budgetTotalsComponent.$onInit();

        expect(budgetTotalsComponent.estimateBalance).toBe(-66.38);
    });

    it('should get actual balance', () => {
        budget.categories = createCategories();
        budget.income = 234.5;

        budgetTotalsComponent.$onInit();

        expect(budgetTotalsComponent.actualBalance).toBe(-177.24);
    });

    it('should have zero estimate total if no budget', () => {
        expect(budgetTotalsComponent.estimateTotal).toBe(0);
    });

    it('should have zero actual total if no budget', () => {
        expect(budgetTotalsComponent.actualTotal).toBe(0);
    });

    it('should have zero estimate balance if no budget', () => {
        budgetTotalsComponent.budget = undefined;

        expect(budgetTotalsComponent.estimateBalance).toBe(0);
    });

    it('should have zero actual balance if no budget', () => {
        budgetTotalsComponent.budget = undefined;
        
        expect(budgetTotalsComponent.actualBalance).toBe(0);
    });

    function createCategories(): Category[] {
        return [
            {
                lineItems: [
                    { estimate: 123.34, actual: 321.4 },
                    { estimate: 123.34, actual: 3 },
                ]
            },
            {
                lineItems: [
                    { estimate: 54.2, actual: 87.34 }
                ]
            }
        ];
    }
});