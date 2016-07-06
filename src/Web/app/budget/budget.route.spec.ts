import { Route } from '../shared';

describe('BudgetRoute', () => {
    let budgetRoute: Route;

    beforeEach(angular.mock.inject((_$state_) => {
        budgetRoute = _$state_.get('Budget');
    }));

    it('should define url for budget', () => {
        expect(budgetRoute.url).toBe('/budget');
    });

    it('should be first route', () => {
        expect(budgetRoute.order).toBe(1);
    });

    it('should use component as template', () => {
        expect(budgetRoute.template).toBe('<budget></budget>')
    })
})