import { Budget, BudgetComponent } from './';
import { Config, ConfigService } from '../shared';

describe('BudgetComponent', () => {
    let $httpBackend: angular.IHttpBackendService;
    let $compile: angular.ICompileService;
    let $scope: angular.IScope;
    let budget: Budget;
    let configService: ConfigService;
    let config: Config;
    let budgetComponent: BudgetComponent;
    let componentOptions: angular.IComponentOptions;

    beforeEach(angular.mock.inject((_$controller_, _$httpBackend_, _$q_, _$compile_, _$rootScope_, _budgetDirective_, _ConfigService_) => {
        $httpBackend = _$httpBackend_;
        $compile = _$compile_;
        $scope = _$rootScope_.$new();

        configService = _ConfigService_;
        config = { budgetApiUrl: 'http://google.com/api' };
        spyOn(configService, 'getConfig').and.returnValue(_$q_.resolve(config));

        budget = { month: 3, year: 2015 };
        $httpBackend.whenGET(`${config.budgetApiUrl}/budgets/current`)
            .respond(budget);

        componentOptions = _budgetDirective_[0];
        budgetComponent = _$controller_(BudgetComponent);
    }));

    it('should get current budget', () => {
        budgetComponent.$onInit();
        $httpBackend.flush();

        expect(budgetComponent.budget).toEqual(budget);
    });

    it('should use budget component as controller', () => {
        expect(componentOptions.controller).toBe(BudgetComponent);
    });

    it('should use budget template as template', () => {
        expect(componentOptions.template).toBe(require('./templates/budget.template'));
    });
})