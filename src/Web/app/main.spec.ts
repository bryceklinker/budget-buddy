import './main';

describe('main', () => {
    let toastrConfig: angular.toastr.IToastrConfig;

    beforeEach(angular.mock.inject((_toastrConfig_) => {
        toastrConfig = _toastrConfig_;
    }))

    it('should define budget buddy module', () => {
        expect(angular.module('budget-buddy')).toBeDefined();
    });

    it('should configure notifications to be in bottom right', () => {
        expect(toastrConfig.positionClass).toBe('toast-bottom-right');
    })
})