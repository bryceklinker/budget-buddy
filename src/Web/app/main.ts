const budgetBuddyModule = angular.module('budget-buddy', ['ui.router', 'ui.bootstrap', 'ngAnimate', 'toastr']);
budgetBuddyModule.config([
    'toastrConfig',
    (toastrConfig: angular.toastr.IToastBaseConfig) =>{
        angular.extend(toastrConfig, {
            positionClass: 'toast-bottom-right'
        })
    }
])
import './shared';
import './navigation';
import './budget';