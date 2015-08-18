angular.module('inventoryManagerFilters', []).filter('startFrom', function () {
    return function (input, start) {
        if (input) {
            start = +start;
            if (angular.isArray(input)) {
                return input.slice(start);
            }
        }
        return [];
    };
});