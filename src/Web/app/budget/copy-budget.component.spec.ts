import { CopyBudgetComponent } from './copy-budget.component';
import { BudgetRoute } from './budget.route';
import { ConfigService, Config } from '../shared';

describe('CopyBudgetComponent', () => {
    let $stateParams: angular.ui.IStateParamsService;
    let $state: angular.ui.IStateService
    let $httpBackend: angular.IHttpBackendService;
    let config: Config;
    let configService: ConfigService;
    let copyBudgetComponent: CopyBudgetComponent;

    beforeEach(angular.mock.inject((_$controller_, _$httpBackend_, _$state_, _$stateParams_, _$q_, _ConfigService_) => {
        $state = _$state_;
        $httpBackend = _$httpBackend_;

        $stateParams = _$stateParams_;
        $stateParams['year'] = 2016;
        $stateParams['month'] = 8;

        configService = _ConfigService_;
        config = { budgetApiUrl: 'http://google.com/api' };
        spyOn(configService, 'getConfig').and.returnValue(_$q_.resolve(config));

        copyBudgetComponent = _$controller_(CopyBudgetComponent, {
            $state: $state,
            $stateParams: $stateParams,
            ConfigService: configService
        });
    }));

    it('should default from date to previous month', () => {
        copyBudgetComponent.$onInit();
        expect(copyBudgetComponent.fromDate).toEqual(new Date(2016, 6, 1));
    });

    it('should copy old budget to specified year and month', () => {
        $httpBackend.expectPOST(`${config.budgetApiUrl}/budgets/copy`, {
            fromYear: 2016,
            fromMonth: 6,
            toYear: 2016,
            toMonth: 8
        }).respond({});

        copyBudgetComponent.fromDate = new Date(2016, 5, 1);
        copyBudgetComponent.copy();
        $httpBackend.flush();
    });

    it('should go to newly copied budget', () => {
        spyOn($state, 'go').and.callFake(() => {});

        $httpBackend.expectPOST(`${config.budgetApiUrl}/budgets/copy`, {
            fromYear: 2016,
            fromMonth: 6,
            toYear: 2016,
            toMonth: 8
        }).respond({});

        copyBudgetComponent.fromDate = new Date(2016, 5, 1);
        copyBudgetComponent.copy();
        $httpBackend.flush();
        
        expect($state.go).toHaveBeenCalledWith(BudgetRoute,
            { year: 2016, month: 8 },
            { reload: true }
        );
    });

    afterEach(() => {
        $httpBackend.verifyNoOutstandingRequest();
        $httpBackend.verifyNoOutstandingExpectation();
    })
})