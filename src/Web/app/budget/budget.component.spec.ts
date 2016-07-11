import { Budget, Category, BudgetComponent } from './';
import { Config, ConfigService } from '../shared';

describe('BudgetComponent', () => {
    let $httpBackend: angular.IHttpBackendService;
    let $http: angular.IHttpService;
    let $scope: angular.IScope;
    let form: angular.IFormController;
    let budget: Budget;
    let configService: ConfigService;
    let config: Config;
    let budgetComponent: BudgetComponent;
    let componentOptions: angular.IComponentOptions;

    beforeEach(angular.mock.inject((_$controller_, _$rootScope_, _$httpBackend_, _$http_, _$q_, _budgetDirective_, _ConfigService_) => {
        $http = _$http_;
        $scope = _$rootScope_.$new();
        $httpBackend = _$httpBackend_;

        configService = _ConfigService_;
        config = { budgetApiUrl: 'http://google.com/api' };
        spyOn(configService, 'getConfig').and.returnValue(_$q_.resolve(config));

        budget = { month: 3, year: 2015 };
        $httpBackend.whenGET(`${config.budgetApiUrl}/budgets/current`)
            .respond(budget);

        form = jasmine.createSpyObj<angular.IFormController>('form', ['$valid']);
        componentOptions = _budgetDirective_[0];
        budgetComponent = _$controller_(BudgetComponent);
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

        $httpBackend.expectPUT(`${config.budgetApiUrl}/budgets/${budget.month}/${budget.year}`, budget)
            .respond({});

        form.$valid = true;
        budgetComponent.save(form);;
        $httpBackend.flush();

        expect($http.put).toHaveBeenCalledWith(`${config.budgetApiUrl}/budgets/${budget.month}/${budget.year}`, budgetComponent.budget);
    });

    it('should get default name if no category name', () => {
        budgetComponent.$onInit();
        $httpBackend.flush();

        budgetComponent.addCategory();
        budgetComponent.budget.categories[0].name = undefined;
        expect(budgetComponent.getCategoryHeading(budgetComponent.budget.categories[0])).toBe('New Category');
    });

    it('should not save invalid budget', () => {
        budgetComponent.$onInit();
        $httpBackend.flush();

        form.$valid = false;
        budgetComponent.save(form);
        $scope.$digest();

        $httpBackend.verifyNoOutstandingRequest();
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
})