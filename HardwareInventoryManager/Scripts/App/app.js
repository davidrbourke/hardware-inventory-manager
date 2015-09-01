var inventoryManagerApp = angular.module('inventoryManagerApp', [
    'ngRoute',
    'ngResource',
    'ngAnimate',
    'datatables',
    'quoteControllers',
    'ui.bootstrap',
    'inventoryManagerFilters']);

inventoryManagerApp.config(['$routeProvider', '$locationProvider',
    function ($routeProvider, $locationProvider) {
        $routeProvider.
            when('/QuoteList', {
                templateUrl: '/Scripts/App/Views/Quotes/list.html',
                controller: 'QuoteController'
            });
        /*$locationProvider.html5Mode(true);*/
    }]);