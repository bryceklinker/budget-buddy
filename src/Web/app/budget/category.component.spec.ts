import { CategoryComponent } from './category.component';
import { Category } from './';

describe('CategoryComponent', () => {
    let categoryComponent: CategoryComponent;
    let componentOptions: angular.IDirective;

    beforeEach(angular.mock.inject((_$controller_, _categoryDirective_) => {
        componentOptions = _categoryDirective_[0];
        categoryComponent = _$controller_(CategoryComponent);
        categoryComponent.category = {};
    }));

    it('should bind category and budget', () => {
        expect(componentOptions.bindToController['budget']).toBe('=');
        expect(componentOptions.bindToController['category']).toBe('=');
    })

    it('should sum up line item estimates', () => {
        categoryComponent.category.lineItems = [
            { estimate: 32.12 },
            { estimate: 82.62 },
            { estimate: 2.7 },
        ];

        expect(categoryComponent.getEstimateTotal()).toBeCloseTo(117.44);
    });

    it('should sum up line item actuals', () => {
        categoryComponent.category.lineItems = [
            { actual: 42.12 },
            { actual: 52.62 },
            { actual: 2.7 },
        ];

        expect(categoryComponent.getActualTotal()).toBeCloseTo(97.44);
    });

    it('should have estimate total of zero', () => {
        categoryComponent.category.lineItems = undefined;

        expect(categoryComponent.getEstimateTotal()).toBe(0);
    });

    it('should have actual total of zero', () => {
        categoryComponent.category.lineItems = undefined;

        expect(categoryComponent.getActualTotal()).toBe(0);
    });

    it('should use zero if estimate is missing from line item', () => {
        categoryComponent.category.lineItems = [
            { estimate: 5 },
            { estimate: undefined }
        ];

        expect(categoryComponent.getEstimateTotal()).toBe(5);
    });

    it('should use zero if actual is missing from line item', () => {
        categoryComponent.category.lineItems = [
            { actual: 5 },
            { actual: undefined }
        ];

        expect(categoryComponent.getActualTotal()).toBe(5);
    });

    it('should add new line item', () =>{
        categoryComponent.addLineItem();
        expect(categoryComponent.category.lineItems.length).toBe(1);
        expect(categoryComponent.category.lineItems[0]).toEqual({});
    });

    it('should delete line item from category', () => {
        categoryComponent.addLineItem();
        categoryComponent.addLineItem();
        categoryComponent.addLineItem();

        const deletedItem = categoryComponent.category.lineItems[1];
        categoryComponent.deleteLineItem(deletedItem);
        expect(categoryComponent.category.lineItems.indexOf(deletedItem)).toBe(-1);
    });

    it('should calculate percent of income for line item', () => {
        categoryComponent.budget = { income: 100};

        const lineItem = { actual: 45.3, estimate: 50 }
        const percentOfIncome = categoryComponent.calculatePercentOfIncome(lineItem);
        expect(percentOfIncome).toBeCloseTo(45.3);
    });

    it('should calculate percent of income for category', () => {
        categoryComponent.budget = { income: 200 };
        categoryComponent.category.lineItems = [
            { actual: 45.2, estimate: 50 },
            { actual: 72.5, estimate: 50 }
        ];

        const percentOfIncome = categoryComponent.calculatePercentOfIncome();
        expect(percentOfIncome).toBeCloseTo(58.85);
    });

    it('should have 0 percent of income if no budget', () => {
        categoryComponent.budget = undefined;
        expect(categoryComponent.calculatePercentOfIncome()).toBe(0);
    })
})