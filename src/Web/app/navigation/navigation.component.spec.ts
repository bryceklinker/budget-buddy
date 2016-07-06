import { NavigationComponent } from './navigation.component';
import { RouteFinderService } from '../shared';

describe('NavigationComponent', () => {
    let componentOptions: angular.IComponentOptions;
    let routeFinderService: RouteFinderService;
    let navigationComponent: NavigationComponent;

    beforeEach(angular.mock.inject((_$controller_, _navigationDirective_, _RouteFinderService_) => {
        componentOptions = _navigationDirective_[0];
        routeFinderService = _RouteFinderService_;
        navigationComponent = _$controller_(NavigationComponent);
    }));

    it('should use navigation component as controller', () => {
        expect(componentOptions.controller).toBe(NavigationComponent);
    });

    it('should use navigation template', () => {
        expect(componentOptions.template).toBe(require('./templates/navigation.template'));
    });

    it('should get routes for application', () => {
        navigationComponent.$onInit();
        expect(navigationComponent.routes).toEqual(routeFinderService.getRoutes());
    })
})