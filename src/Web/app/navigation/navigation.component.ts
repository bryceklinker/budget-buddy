import { Route, RouteFinderService } from '../shared';

export class NavigationComponent implements angular.IComponentController {
    routes: Route[]

    constructor(private routeFinderService: RouteFinderService) {

    }

    $onInit() {
        this.routes = this.routeFinderService.getRoutes();
    }
}
NavigationComponent.$inject = ['RouteFinderService'];

angular.module('budget-buddy')
    .component('navigation', {
        controller: NavigationComponent,
        template: require('./templates/navigation.template')
    })