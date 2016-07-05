import { ROUTER_DIRECTIVES } from '@angular/router';
import { Component } from '@angular/core';

@Component({
    selector: 'navigation',
    template: require('./templates/navigation.template'),
    directives: [ROUTER_DIRECTIVES]
})
export class NavigationComponent {

}