import { CategoryComponent } from './category.component';
import { Category } from './';

describe('CategoryComponent', () => {
    let categoryComponent: CategoryComponent;
    let componentOptions: angular.IComponentOptions;

    beforeEach(angular.mock.inject((_$controller_, _categoryDirective_) => {
        componentOptions = _categoryDirective_[0];
        categoryComponent = _$controller_(CategoryComponent);
        categoryComponent.category = {};
    }));

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
    })
})