var quoteControllers = angular.module('quoteControllers', ['ui.bootstrap', 'ui.bootstrap.pagination']);

quoteControllers.controller("QuoteController", ['$scope', 'quoteRepository', '$location', '$modal', 'filterFilter',
    function ($scope, quoteRepository, $location, $modal, filterFilter) {

        
        // Get Asset List paging
        $scope.quotes = [];
        quoteRepository.getQuoteList().$promise.then(
            function (result) {
                $scope.quotes = result;
                //$scope.totalItems = result.length;

                $scope.$watch('search', function (newVal, oldVal) {
                    $scope.filtered = filterFilter($scope.quotes, newVal);
                    $scope.filteredQuotesLength = $scope.filtered.length;
                    $scope.totalItems = $scope.filtered.length;
                    $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
                    $scope.currentPage = 1;
                }, true);
            },
            function () {
            }
        );

        $scope.search = '';

        $scope.currentpage = 1;
        $scope.totalItems = $scope.quotes.length;
        $scope.entryLimit = 4; // items per page
        $scope.noOfpages = Math.ceil($scope.totalitems / $scope.entryLimit);
        $scope.resetFilters = function () {
            // needs to be a function or it won't trigger a $watch
            $scope.search = null;
        };

        // Create Asset - Blank Form with Dropdowns
        $scope.loadModalQuote = function () {
            quoteRepository.createAssetModal().$promise.then(
                function (quote) {
                    f(quote);
                },
                function () {
                }
            );
        };

        var f = function (quote) {
            $scope.$parent.quoteRequestViewModel = quote;
            $scope.$parent.quoteRequestViewModel.selectedTenant = $scope.$parent.quoteRequestViewModel.tenants[0];
        };

        // GET existing asset - open in Modal
        $scope.getQuote = function (id) {
            quoteRepository.getQuote(id).$promise.then(
                function (quote) {
                    f(quote);
                },
                function (quote) {
                    q(quote);
                }
            );
        };

        $scope.deleteQuote = function (id) {
            quoteRepository.getQuoteToDelete(id).$promise.then(
                function (quote) {
                    f(quote);
                },
                function (quote) {
                }
            );
        };

    }]);

// Please note that $modalInstance represents a modal window (instance) dependency.
// It is not the same as the $modal service used above.

quoteControllers.controller('ModalInstanceCtrl',
    function ($scope, $modalInstance, $location, quoteRepository) {

        $scope.postQuote = function (quote) {
            $scope.errors = [];
            quote.providers = null;
            quoteRepository.postQuote(quote).$promise.then(
                function () {
                    $location.url('QuoteList/');
                    $scope.saved = true;
                    $modalInstance.close();
                    toastr["success"]("Saved");
                },
                function (response) {
                    var errors = Array();
                    if (response.data.modelState["value.quantity"]) {
                        errors.push(response.data.modelState["value.quantity"][0]);
                    }
                    $scope.errors = errors;
                    toastr["error"]("Not saved");
                });
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.deleteQuote = function (quote) {
            $scope.errors = [];
            quoteRepository.deleteQuote(quote).$promise.then(
                function () {
                    $location.url('QuoteList/');
                    $scope.saved = true;
                    $modalInstance.close();
                    toastr["success"]("Saved");
                },
                function (response) {
                    $scope.errors = response.data;
                    toastr["error"]("Not saved");
                });
        };
    });