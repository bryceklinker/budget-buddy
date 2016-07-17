import { Route } from '../shared';

export const BudgetRoute: Route = {
    template: '<budget></budget>',
    url: '/budgets/:year/:month',
    name: 'Budget',
    order: 1
}

angular.module('budget-buddy')
    .config([
        '$stateProvider',
        '$urlRouterProvider',
        ($stateProvider: angular.ui.IStateProvider, $urlRouterProvider: angular.ui.IUrlRouterProvider) => {
            const year = new Date().getFullYear();
            const month = new Date().getMonth() + 1;
            var defaultRoute = BudgetRoute.url.toString().replace(':year', year).replace(':month', month);
            $urlRouterProvider.otherwise(defaultRoute);;
            $stateProvider.state(BudgetRoute);
        }
    ])