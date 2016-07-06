import './main';

describe('main', () => {
    it('should define budget buddy module', () => {
        expect(angular.module('budget-buddy')).toBeDefined();
    })
})