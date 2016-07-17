import * as moment from 'moment';

import { ConfigService } from '../shared';
import { BudgetRoute } from './budget.route';

export class CopyBudgetComponent implements angular.IComponentController {
    static $inject = ['$http', '$state', '$stateParams', 'ConfigService']

    fromDate: Date;

    constructor(private $http: angular.IHttpService,
        private $state: angular.ui.IStateService,
        private $stateParams: angular.ui.IStateParamsService,
        private configService: ConfigService) {

    }

    $onInit(){
        const year = this.$stateParams['year'];
        const month = Number(this.$stateParams['month']) - 1;
        const date = moment(new Date(year, month, 1)); 
        this.fromDate = date.subtract(1, 'month').toDate();
    }

    copy(): void {
        const toYear = this.$stateParams['year'];
        const toMonth = this.$stateParams['month'];
        const copyOptions = {
            fromYear: this.fromDate.getFullYear(),
            fromMonth: this.fromDate.getMonth() + 1,
            toYear: toYear,
            toMonth: toMonth
        };
        
        this.configService.getConfig()
            .then(c => this.$http.post(`${c.budgetApiUrl}/budgets/copy`, copyOptions))
            .then(() => this.$state.go(BudgetRoute, { year: toYear, month: toMonth }, { reload: true }));
    }
}