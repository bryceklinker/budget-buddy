import { Route, RouteFinderService } from './';

describe('RouteFinderService', () => {
    let routeFinderService: RouteFinderService;

    beforeEach(angular.mock.inject((_RouteFinderService_) => {
        routeFinderService = _RouteFinderService_;
    }))

    it('should get all routes in app', () => {
        const expected = getRoutes();

        const actual = routeFinderService.getRoutes();
        expect(actual).toEqual(expected);
    })

    function getRoutes(): Route[] {
        const context = (<any>require).context('../..', true, /\.route\.ts$/);
        return context.keys()
            .map(context)
            .map(getRoute)
            .filter(r => r !== undefined)
            .sort((a, b) => a.order - b.order);
    }

    function getRoute(obj: Object): Route {
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop)) {
                return obj[prop];
            }
        }
        return undefined;
    }
})