var inventoryManagerApp = angular.module('inventoryManagerApp', [
    'ngRoute',
    'ngResource',
    'ngAnimate',
    'datatables',
    'quoteControllers',
    'bulkUploadControllers',
    'ui.bootstrap',
    'inventoryManagerFilters']);

inventoryManagerApp.config(['$routeProvider', '$locationProvider',
    function ($routeProvider, $locationProvider) {
        $routeProvider.
            when('/QuoteList', {
                templateUrl: '/Scripts/App/Views/Quotes/list.html',
                controller: 'QuoteController'
            }).
            when('/BulkUpload', {
                templateUrl: '/Scripts/App/Views/BulkUploads/list.html',
                controller: 'bulkUploadController'
            });
        /*$locationProvider.html5Mode(true);*/
    }]);