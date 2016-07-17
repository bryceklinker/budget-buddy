import { CopyBudgetComponent } from './copy-budget.component';
import { BudgetRoute } from './budget.route';
import { ConfigService, Config } from '../shared';

describe('CopyBudgetComponent', () => {
    let $stateParams: angular.ui.IStateParamsService;
    let $state: angular.ui.IStateService
    let $httpBackend: angular.IHttpBackendService;
    let $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance;
    let config: Config;
    let configService: ConfigService;
    let copyBudgetComponent: CopyBudgetComponent;

    beforeEach(angular.mock.inject((_$controller_, _$httpBackend_, _$state_, _$stateParams_, _$q_, _ConfigService_) => {
        $httpBackend = _$httpBackend_;

        $state = _$state_;
        spyOn($state, 'go').and.callFake(() => { });

        $stateParams = _$stateParams_;
        $stateParams['year'] = 2016;
        $stateParams['month'] = 8;

        configService = _ConfigService_;
        config = { budgetApiUrl: 'http://google.com/api' };
        spyOn(configService, 'getConfig').and.returnValue(_$q_.resolve(config));

        $uibModalInstance = jasmine.createSpyObj<angular.ui.bootstrap.IModalServiceInstance>('modalInstance', ['close'])
        copyBudgetComponent = _$controller_(CopyBudgetComponent, {
            $state: $state,
            $stateParams: $stateParams,
            $uibModalInstance: $uibModalInstance,
            ConfigService: configService
        });
    }));

    it('should default from date to previous month', () => {
        copyBudgetComponent.$onInit();
        expect(copyBudgetComponent.fromDate).toEqual(new Date(2016, 6, 1));
    });

    it('should have to date as state params date', () => {
        copyBudgetComponent.$onInit();
        expect(copyBudgetComponent.toDate).toEqual(new Date(2016, 7, 1));
    });

    it('should go to newly copied budget', () => {
        $httpBackend.expectPOST(`${config.budgetApiUrl}/budgets/copy`, {
            fromYear: 2016,
            fromMonth: 6,
            toYear: 2016,
            toMonth: 8
        }).respond({});

        copyBudgetComponent.$onInit();
        copyBudgetComponent.fromDate = new Date(2016, 5, 1);
        copyBudgetComponent.copy();
        $httpBackend.flush();

        expect($state.go).toHaveBeenCalledWith(BudgetRoute,
            { year: 2016, month: 8 },
            { reload: true }
        );
        expect($uibModalInstance.close).toHaveBeenCalled();
    });

    it('should go to newly created budget', () => {
        $httpBackend.expectPOST(`${config.budgetApiUrl}/budgets`, { startDate: new Date(2016, 7, 1) })
            .respond({});

        copyBudgetComponent.$onInit();
        copyBudgetComponent.toDate = new Date(2016, 7, 1);
        copyBudgetComponent.create();
        $httpBackend.flush();

        expect($state.go).toHaveBeenCalledWith(BudgetRoute,
            { year: 2016, month: 8 },
            { reload: true }
        );
        expect($uibModalInstance.close).toHaveBeenCalled();
    });

    afterEach(() => {
        $httpBackend.verifyNoOutstandingRequest();
        $httpBackend.verifyNoOutstandingExpectation();
    })
})