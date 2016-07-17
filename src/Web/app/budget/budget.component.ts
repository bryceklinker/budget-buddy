import { ConfigService } from '../shared';
import { CopyBudgetComponent } from './copy-budget.component';
import { 
    Budget, 
    Category
} from './';

import './styles/budget';
export class BudgetComponent implements angular.IComponentController {
    budget: Budget;
    year: number;
    month: number;

    constructor(private configService: ConfigService,
        private $http: angular.IHttpService,
        private $uibModal: angular.ui.bootstrap.IModalService,
        private $stateParams: angular.ui.IStateParamsService) {

    }

    $onInit(): void {
        this.year = this.$stateParams['year'];
        this.month = this.$stateParams['month'];
        this.configService.getConfig()
            .then(c => this.$http.get<Budget>(`${c.budgetApiUrl}/budgets/${this.year}/${this.month}`))
            .then(res => this.budget = res.data, (err: angular.IHttpPromiseCallbackArg<Budget>) => {
                this.$uibModal.open({
                        backdrop: 'static',
                        keyboard: false,
                        controller: CopyBudgetComponent,
                        controllerAs: '$copy',
                        template: require('./templates/copy-budget.template')
                    });
            });
    }

    getCategoryHeading(category: Category): string {
        if (!category.name)
            return 'New Category';
        
        return category.name;
    }

    addCategory(): void {
        if (!this.budget.categories)
            this.budget.categories = [];
        
        this.budget.categories.push({});
    }

    save(): void {
        const month = this.budget.month;
        const year = this.budget.year;
        this.configService.getConfig()
            .then(c => this.$http.put(`${c.budgetApiUrl}/budgets/${this.month}/${this.year}`, this.budget));
    }
}
BudgetComponent.$inject = ['ConfigService', '$http', '$uibModal', '$stateParams']
angular.module('budget-buddy')
    .component('budget', {
        controller: BudgetComponent,
        template: require('./templates/budget.template')
    });