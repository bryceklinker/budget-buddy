import { ConfigService } from '../shared';
import { Budget } from './';

export class BudgetComponent implements angular.IComponentController {
    budget: Budget;

    constructor(private configService: ConfigService,
        private $http: angular.IHttpService) {

    }

    $onInit() {
        this.configService.getConfig()
            .then(c => this.$http.get<Budget>(`${c.budgetApiUrl}/budgets/current`))
            .then(res => this.budget = res.data);
    }
}
BudgetComponent.$inject = ['ConfigService', '$http']
angular.module('budget-buddy')
    .component('budget', {
        controller: BudgetComponent,
        template: require('./templates/budget.template')
    })