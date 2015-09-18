inventoryManagerApp.factory('quoteRepository', ['$resource', '$modal',
    function ($resource, $modal) {
        return {
            getQuoteList: function () {
                return $resource('/api/QuoteRequests').query();
            },
            createAssetModal: function (resp) {
                var res = null;
                var modalInstance = $modal.open({
                    templateUrl: '/Scripts/App/Views/Quotes/edit.html',
                    controller: 'ModalInstanceCtrl',
                    resolve: {
                        asset: function () {
                            return res = $resource('/api/QuoteRequests/-1').get();
                        }
                    }
                });
                return res;
            },
            getQuote: function (id) {
                var res = null;
                var modalInstance = $modal.open({
                    templateUrl: '/Scripts/App/Views/Quotes/edit.html',
                    controller: 'ModalInstanceCtrl',
                    resolve: {
                        asset: function () {
                            return res = $resource('/api/QuoteRequests/'+id).get();
                        }
                    }
                });
                return res;
            },
            postQuote: function (quote) {
                return $resource('/api/QuoteRequests/').save(quote);
            },
            getQuoteToDelete: function (id) {
                var res = null;
                var modalInstance = $modal.open({
                    templateUrl: '/Scripts/App/Views/Quotes/delete.html',
                    controller: 'ModalInstanceCtrl',
                    resolve: {
                        asset: function () {
                            return res = $resource('/api/QuoteRequests/' + id).get();
                        }
                    }
                });
                return res;
            },
            deleteQuote: function (quote) {
                return $resource('/api/QuoteRequests/:id', { id: quote.QuoteRequestId }).delete();
            }
        };
    }]);