import * as moment from 'moment';
import { ConfigService } from '../shared';
import { CopyBudgetComponent } from './copy-budget.component';
import { 
    Budget, 
    Category,
    BudgetRoute
} from './';

import './styles/budget';
export class BudgetComponent implements angular.IComponentController {
    static $inject = ['ConfigService', '$http', '$uibModal', 'toastr', '$stateParams', '$state']
    private _startDate: Date;
    activeCategoryIndex: number;
    budget: Budget;
    year: number;
    month: number;
    
    get startDate(): Date {
        return this._startDate;
    }
    set startDate(date: Date) {
        this.$state.go(BudgetRoute, {year: date.getFullYear(), month: date.getMonth() + 1}, { reload: true });
    }

    constructor(private configService: ConfigService,
        private $http: angular.IHttpService,
        private $uibModal: angular.ui.bootstrap.IModalService,
        private toastr: angular.toastr.IToastrService,
        private $stateParams: angular.ui.IStateParamsService,
        private $state: angular.ui.IStateService) {

    }

    $onInit(): void {
        this.year = this.$stateParams['year'];
        this.month = this.$stateParams['month'];
        this._startDate = new Date(this.year, this.month - 1, 1);

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
            .then(c => this.$http.put(`${c.budgetApiUrl}/budgets/${this.year}/${this.month}`, this.budget))
            .then(c => this.toastr.success('Budget saved successfully', 'Budget'));
    }
}

angular.module('budget-buddy')
    .component('budget', {
        controller: BudgetComponent,
        template: require('./templates/budget.template')
    });