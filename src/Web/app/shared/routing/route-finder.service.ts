import { Route } from './';

export class RouteFinderService {
    getRoutes(): Route[] {
        const context = (<any>require).context('../..', true, /\.route\.ts$/);
        return context.keys()
            .map(context)
            .map(this.getRoute)
            .filter(r => r !== undefined)
            .sort((a, b) => a.order - b.order);
               
    }

    private getRoute(obj: Object): Route {
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop)) {
                return obj[prop];                
            }
        }
        return undefined;
    }
}
angular.module('budget-buddy')
    .service('RouteFinderService', RouteFinderService);