var quoteControllers = angular.module('quoteControllers', ['ui.bootstrap', 'ui.bootstrap.pagination']);

quoteControllers.controller("QuoteController", ['$scope', 'quoteRepository', '$location', '$modal', 'filterFilter',
    function ($scope, quoteRepository, $location, $modal, filterFilter) {

        // Get Asset List paging
        $scope.quotes = quoteRepository.getQuoteList().$promise.then(
            function (d) {
                $scope.quotes = d;
                $scope.totalItems = d.length;
            },
            function () {
            }
        );

        $scope.search = '';

        $scope.resetFilters = function () {
            // needs to be a function or it won't trigger a $watch
            $scope.search = {};
        };

        // pagination controls
        $scope.currentPage = 1;
        $scope.totalItems = $scope.quotes.length;
        $scope.entryLimit = 10; // items per page
        $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);

        $scope.$watch('search', function (newVal, oldVal) {
            $scope.filtered = filterFilter($scope.quotes, newVal);
            $scope.totalItems = $scope.filtered.length;
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
            $scope.currentPage = 1;
        }, true);



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
            var modalInstance = $modal.open({
                animation: $scope.animationsEnabled,
                templateUrl: '/Scripts/App/Views/Assets/edit.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    asset: function () {
                        var result = quoteRepository.getQuote(id);
                        return $scope.$parent.quoteRequestViewModel = result;
                        //return $scope.items;
                    }
                }
            });

            modalInstance.result.then(function (res) {
                /*console.log('hit: ' + res);*/
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });

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
                },
                function (response) {
                    $scope.errors = response.data;
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