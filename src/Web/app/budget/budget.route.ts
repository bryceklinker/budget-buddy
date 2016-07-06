import { Route } from '../shared';

export const BudgetRoute: Route = {
    template: '<budget></budget>',
    url: '/budget',
    name: 'Budget',
    order: 1
}

angular.module('budget-buddy')
    .config([
        '$stateProvider',
        '$urlRouterProvider',
        ($stateProvider: angular.ui.IStateProvider, $urlRouterProvider: angular.ui.IUrlRouterProvider) => {
            $urlRouterProvider.otherwise(BudgetRoute.name);;
            $stateProvider.state(BudgetRoute);
        }
    ])