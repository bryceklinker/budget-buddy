import { Budget, Category, BudgetComponent } from './';
import { CopyBudgetComponent } from './copy-budget.component';
import { Config, ConfigService } from '../shared';

describe('BudgetComponent', () => {
    let $httpBackend: angular.IHttpBackendService;
    let $http: angular.IHttpService;
    let $scope: angular.IScope;
    let $stateParams: angular.ui.IStateParamsService;
    let $uibModal: angular.ui.bootstrap.IModalService;
    let toastr: angular.toastr.IToastrService;
    let budget: Budget;
    let configService: ConfigService;
    let config: Config;
    let budgetComponent: BudgetComponent;
    let componentOptions: angular.IComponentOptions;

    beforeEach(angular.mock.inject((_$controller_, _$rootScope_, _$httpBackend_, _$http_, _$q_, _$stateParams_, _$uibModal_, _toastr_, _budgetDirective_, _ConfigService_) => {
        $http = _$http_;
        $scope = _$rootScope_.$new();
        $httpBackend = _$httpBackend_;
        $stateParams = _$stateParams_;
        $uibModal = _$uibModal_;
        toastr = _toastr_;

        configService = _ConfigService_;
        config = { budgetApiUrl: 'http://google.com/api' };
        spyOn(configService, 'getConfig').and.returnValue(_$q_.resolve(config));

        $stateParams['month'] = 3;
        $stateParams['year'] = 2015;
        budget = { month: 3, year: 2015 };
        $httpBackend.whenGET(`${config.budgetApiUrl}/budgets/${$stateParams['year']}/${$stateParams['month']}`)
            .respond(budget);

        componentOptions = _budgetDirective_[0];
        budgetComponent = _$controller_(BudgetComponent, {
            $stateParams: $stateParams, 
            $uibModal: $uibModal,
            toastr: toastr
        });
    }));

    it('should use budget component as controller', () => {
        expect(componentOptions.controller).toBe(BudgetComponent);
    });

    it('should use budget template as template', () => {
        expect(componentOptions.template).toBe(require('./templates/budget.template'));
    });

    it('should get current budget', () => {
        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.budget).toEqual(budget);
    });

    it('should add category to budget', () => {
        budgetComponent.$onInit();
        $httpBackend.flush();

        budgetComponent.addCategory();
        expect(budgetComponent.budget.categories.length).toBe(1);
        expect(budgetComponent.budget.categories[0]).toEqual({});
    });

    it('should put budget in api', () => {
        spyOn($http, 'put').and.callThrough();

        budgetComponent.$onInit();
        $httpBackend.flush();

        budgetComponent.budget.income = 23;
        $httpBackend.expectPUT(`${config.budgetApiUrl}/budgets/${budget.year}/${budget.month}`, budgetComponent.budget)
            .respond({});
        
        budgetComponent.save();
        $httpBackend.flush();

        expect($http.put).toHaveBeenCalledWith(`${config.budgetApiUrl}/budgets/${budget.year}/${budget.month}`, budgetComponent.budget);
    });

    it('should get default name if no category name', () => {
        budgetComponent.$onInit();
        $httpBackend.flush();

        budgetComponent.addCategory();
        budgetComponent.budget.categories[0].name = undefined;
        expect(budgetComponent.getCategoryHeading(budgetComponent.budget.categories[0])).toBe('New Category');
    });

    it('should ask to copy or create older budget', () => {
        let modalInstance = $uibModal.open({
            template: require('./templates/copy-budget.template')
        });
        spyOn($uibModal, 'open').and.returnValue(modalInstance);

        $stateParams['year'] = 2016;
        $stateParams['month'] = 6;
        $httpBackend.expectGET(`${config.budgetApiUrl}/budgets/2016/6`)
            .respond(404);

        budgetComponent.$onInit();
        $httpBackend.flush();

        expect($uibModal.open).toHaveBeenCalledWith({
            backdrop: 'static',
            keyboard: false,
            controller: CopyBudgetComponent,
            controllerAs: '$copy',
            template: require('./templates/copy-budget.template')
        })
    });

    it('should notify that budget was saved', () => {
        spyOn(toastr, 'success').and.callThrough();
        budgetComponent.$onInit();
        $httpBackend.flush();

        $httpBackend.whenPUT(`${config.budgetApiUrl}/budgets/${budget.year}/${budget.month}`, budgetComponent.budget)
            .respond({});

        budgetComponent.save();
        $httpBackend.flush();

        expect(toastr.success).toHaveBeenCalledWith('Budget saved successfully', 'Budget');
    })

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
})