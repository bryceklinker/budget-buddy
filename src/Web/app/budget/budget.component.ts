import { ConfigService } from '../shared';
import { 
    Budget, 
    Category    
} from './';

import './styles/budget';
export class BudgetComponent implements angular.IComponentController {
    budget: Budget;

    constructor(private configService: ConfigService,
        private $http: angular.IHttpService,
        private $uibModal: angular.ui.bootstrap.IModalService) {

    }

    $onInit(): void {
        this.configService.getConfig()
            .then(c => this.$http.get<Budget>(`${c.budgetApiUrl}/budgets/current`))
            .then(res => this.budget = res.data);
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

    save(form: angular.IFormController): void {
        if (!form.$valid) {
            return;
        }

        const month = this.budget.month;
        const year = this.budget.year;
        this.configService.getConfig()
            .then(c => this.$http.put(`${c.budgetApiUrl}/budgets/${month}/${year}`, this.budget));
    }
}
BudgetComponent.$inject = ['ConfigService', '$http', '$uibModal']
angular.module('budget-buddy')
    .component('budget', {
        controller: BudgetComponent,
        template: require('./templates/budget.template')
    });