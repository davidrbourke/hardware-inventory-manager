var bulkUploadControllers = angular.module('bulkUploadControllers', ['ngFileUpload']);

bulkUploadControllers.controller('bulkUploadController', ['$scope', 'Upload',
    function ($scope, Upload) {
      
        $scope.uploadFile = function () {

            var files = $scope.files;
            var mfile = files[0];

            if (files !== null) {
                if (!files.$error) {
                    Upload.upload({
                        url: 'http://localhost:10862/BulkUploads/Upload',
                        fileFormDataName: 'FileUpload',
                        file: mfile
                    })
                    .progress(function (evt) {

                    })
                    .success(function (data, status, headers, config) {

                    });
                }
            }
        };
    }]
);