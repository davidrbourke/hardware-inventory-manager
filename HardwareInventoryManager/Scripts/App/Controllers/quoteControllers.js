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
        $scope.entryLimit = 10; // items per page
        $scope.noOfpages = Math.ceil($scope.totalitems / $scope.entryLimit);




        $scope.resetFilters = function () {
            // needs to be a function or it won't trigger a $watch
            $scope.search = null;
        };

        // pagination controls
        //$scope.currentpage = 1;
        ////$scope.totalitems = $scope.quotes.length;
        //$scope.entrylimit = 10; // items per page
        //$scope.noofpages = Math.ceil($scope.totalitems / $scope.entrylimit);




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
            $scope.$parent.quoteRequestViewModel.SelectedTenant = $scope.$parent.quoteRequestViewModel.Tenants[0];
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

        $scope.deleteQuote = function (assetId) {
            var modalInstance = $modal.open({
                templateUrl: '/Scripts/App/Views/Assets/delete.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    asset: function () {
                        return $scope.$parent.assetViewModel = quoteRepository.getAsset(assetId);
                    }
                }
            });
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
                    if (response.data.ModelState["value.Quantity"]) {
                        errors.push(response.data.ModelState["value.Quantity"][0]);
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
                    $location.url('AssetList/');
                    $scope.saved = true;
                    $modalInstance.close();
                },
                function (response) {
                    $scope.errors = response.data;
                });
        };
    });