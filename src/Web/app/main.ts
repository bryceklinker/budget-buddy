const budgetBuddyModule = angular.module('budget-buddy', ['ui.router', 'ui.bootstrap', 'ngAnimate', 'toastr']);
budgetBuddyModule.config([
    'toastrConfig',
    (toastrConfig: angular.toastr.IToastConfig) =>{
        angular.extend(toastrConfig, {
            positionClass: 'toast-bottom-right'
        })
    }
])
import './shared';
import './navigation';
import './budget';