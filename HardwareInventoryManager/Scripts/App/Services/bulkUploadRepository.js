inventoryManagerApp.factory('bulkUploadRepository',
    function ($resource) {
        return {
            confirmImport: function (batch) {
                return $resource('ConfirmImport/').save(batch);
            },
            refreshReview: function (batchId) {
                return $resource('RefreshReview/' + batchId).get(batchId);
            }
        };
    });