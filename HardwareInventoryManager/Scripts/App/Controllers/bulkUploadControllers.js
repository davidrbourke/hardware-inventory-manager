var bulkUploadControllers = angular.module('bulkUploadControllers', ['ngFileUpload']);

bulkUploadControllers.controller('bulkUploadController', ['$scope', 'Upload', '$location', 'importService',
    function ($scope, Upload, $location, importService) {
      
        $scope.uploadFile = function () {

            var files = $scope.files;
            var mfile = files[0];

            if (files !== null) {
                if (!files.$error) {
                    Upload.upload({
                        url: '/BulkUploads/Upload',
                        fileFormDataName: 'FileUpload',
                        file: mfile
                    })
                    .progress(function (evt) {

                    })
                    .success(function (data, status, headers, config) {
                        importService.set(data);
                        $location.path('BulkUploadReview');
                    });
                }
            }
        };


    }]
);

bulkUploadControllers.controller('reviewBulkUploadController', ['$scope', 'Upload', '$location', 'importService', 'filterFilter',
    function ($scope, Upload, $location, importService, filterFilter) {
        $scope.assets = importService.get();

        if ($scope.assets instanceof Array) {
            $scope.$watch('search', function (newVal, oldVal) {
                $scope.filtered = filterFilter($scope.assets, newVal);
                $scope.filteredAssetsLength = $scope.filtered.length;
                $scope.totalItems = $scope.filtered.length;
                $scope.noOfPages = 5;//Math.ceil($scope.totalItems / $scope.entryLimit);
                //$scope.numPages = Math.ceil($scope.totalItems / $scope.entryLimit);
                $scope.currentPage = 1;
            }, true);


            $scope.currentPage = 1;
            $scope.totalItems = $scope.assets.length;
            $scope.entryLimit = 10;
            //$scope.noOfPages = 5;// Math.ceil($scope.totalItems / $scope.entryLimit);
            $scope.numPages = Math.ceil($scope.totalItems / $scope.entryLimit);
        }
        $scope.resetFilters = function () {
            // needs to be a function or it won't trigger a $watch
            $scope.search = null;
        };
       
    }
]);