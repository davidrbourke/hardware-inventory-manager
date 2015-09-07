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
            }).
            when('/BulkUploadReview/:batchId', {
                templateUrl: '/Scripts/App/Views/BulkUploads/review.html',
                controller: 'reviewBulkUploadController'
            });
        /*$locationProvider.html5Mode(true);*/
    }]);

inventoryManagerApp.factory('importService', function () {
    var assetsToReview = {}
    function set(data) {
        assetsToReview = data;
    }
    function get() {
        return assetsToReview;
    }

    return {
        set: set,
        get: get
    }
});