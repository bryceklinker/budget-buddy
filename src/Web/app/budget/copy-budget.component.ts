import * as moment from 'moment';

import { ConfigService } from '../shared';
import { BudgetRoute } from './budget.route';

export class CopyBudgetComponent implements angular.IComponentController {
    static $inject = ['$http', '$state', '$stateParams', '$uibModalInstance', 'ConfigService']

    fromDate: Date;
    toDate: Date;

    constructor(private $http: angular.IHttpService,
        private $state: angular.ui.IStateService,
        private $stateParams: angular.ui.IStateParamsService,
        private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance,
        private configService: ConfigService) {

    }

    $onInit(){
        const year = this.$stateParams['year'];
        const month = Number(this.$stateParams['month']) - 1;
        const date = moment(new Date(year, month, 1));
        this.toDate = date.toDate(); 
        this.fromDate = date.subtract(1, 'month').toDate();
    }

    copy(): void {
        const copyOptions = {
            fromYear: this.fromDate.getFullYear(),
            fromMonth: this.fromDate.getMonth() + 1,
            toYear: this.toDate.getFullYear(),
            toMonth: this.toDate.getMonth() + 1
        };
        
        this.configService.getConfig()
            .then(c => this.$http.post(`${c.budgetApiUrl}/budgets/copy`, copyOptions))
            .then(() => this.$state.go(BudgetRoute, { year: copyOptions.toYear, month: copyOptions.toMonth }, { reload: true }))
            .then(() => this.$uibModalInstance.close());
    }

    create(): void {
        const year = this.toDate.getFullYear();
        const month = this.toDate.getMonth() + 1;
        
        this.configService.getConfig()
            .then(c => this.$http.post(`${c.budgetApiUrl}/budgets`, { startDate: this.toDate }))
            .then(() => this.$state.go(BudgetRoute, { year: year, month: month}, { reload: true }))
            .then(() => this.$uibModalInstance.close());
    }
}