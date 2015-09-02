inventoryManagerApp.factory('bulkUploadRepository',
    function ($resource) {
        return {
            confirmImport: function (batch) {
                return $resource('ConfirmImport/').save(batch);
            }
        };
    });