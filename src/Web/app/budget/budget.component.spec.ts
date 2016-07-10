import { Budget, Category, BudgetComponent } from './';
import { Config, ConfigService } from '../shared';

describe('BudgetComponent', () => {
    let $httpBackend: angular.IHttpBackendService;
    let $http: angular.IHttpService;
    let budget: Budget;
    let configService: ConfigService;
    let config: Config;
    let budgetComponent: BudgetComponent;
    let componentOptions: angular.IComponentOptions;

    beforeEach(angular.mock.inject((_$controller_, _$httpBackend_, _$http_, _$q_, _budgetDirective_, _ConfigService_) => {
        $http = _$http_;
        $httpBackend = _$httpBackend_;

        configService = _ConfigService_;
        config = { budgetApiUrl: 'http://google.com/api' };
        spyOn(configService, 'getConfig').and.returnValue(_$q_.resolve(config));

        budget = { month: 3, year: 2015 };
        $httpBackend.whenGET(`${config.budgetApiUrl}/budgets/current`)
            .respond(budget);

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

        budgetComponent.save();
        $httpBackend.flush();

        expect($http.put).toHaveBeenCalledWith(`${config.budgetApiUrl}/budgets/${budget.month}/${budget.year}`, budgetComponent.budget);
    });

    it('should sum up line item estimates', () => {
        budget.categories = createCategories();

        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.estimateTotal).toBe(300.88);
    });

    it('should sum up line item actuals', () => {
        budget.categories = createCategories();

        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.actualTotal).toBe(411.74)
    });

    it('should have zero actual total', () => {
        budget.categories = undefined;

        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.actualTotal).toBe(0);
    });

    it('should have zero estimate total', () => {
        budget.categories = undefined;

        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.estimateTotal).toBe(0);
    });

    it('should get estimate balance', () => {
        budget.categories = createCategories();
        budget.income = 234.5;

        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.estimateBalance).toBe(-66.38);
    });

    it('should get actual balance', () => {
        budget.categories = createCategories();
        budget.income = 234.5;

        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.actualBalance).toBe(-177.24);
    });

    it('should have zero estimate total if no budget', () => {
        expect(budgetComponent.estimateTotal).toBe(0);
    });

    it('should have zero actual total if no budget', () => {
        expect(budgetComponent.actualTotal).toBe(0);
    });

    it('should have zero estimate balance if no budget', () => {
        expect(budgetComponent.estimateBalance).toBe(0);
    });

    it('should have zero actual balance if no budget', () => {
        expect(budgetComponent.actualBalance).toBe(0);
    });

    it('should get default name if no category name', () => {
        budgetComponent.$onInit();
        $httpBackend.flush();

        budgetComponent.addCategory();
        budgetComponent.budget.categories[0].name = undefined;
        expect(budgetComponent.getCategoryHeading(budgetComponent.budget.categories[0])).toBe('New Category');
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