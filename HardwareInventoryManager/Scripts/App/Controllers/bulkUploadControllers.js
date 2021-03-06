﻿var bulkUploadControllers = angular.module('bulkUploadControllers', ['ngFileUpload']);

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
                        $location.path('BulkUploadReview/' + data.batchId);
                    });
                }
            }
        };
    }]
);

bulkUploadControllers.controller('reviewBulkUploadController', ['$scope', 'Upload', '$location', 'importService', 'filterFilter', 'bulkUploadRepository',
    function ($scope, Upload, $location, importService, filterFilter, bulkUploadRepository) {
        $scope.batch = importService.get();

        $scope.buildResults = function () {
            $scope.assets = $scope.batch.assets;
            $scope.batchId = $scope.batch.batchId
            $scope.tenants = $scope.batch.tenants;
            $scope.selectedTenant = $scope.batch.tenants[0];
            $scope.errors = $scope.batch.errors;

            if ($scope.assets instanceof Array) {
                $scope.$watch('search', function (newVal, oldVal) {
                    $scope.filtered = filterFilter($scope.assets, newVal);
                    $scope.filteredAssetsLength = $scope.filtered.length;
                    $scope.totalItems = $scope.filtered.length;
                    $scope.noOfPages = 5;
                    $scope.currentPage = 1;
                }, true);


                $scope.currentPage = 1;
                $scope.totalItems = $scope.assets.length;
                $scope.entryLimit = 10;
                $scope.numPages = Math.ceil($scope.totalItems / $scope.entryLimit);
            }
        };

        if ($scope.batch.assets === undefined) {

            var batchId = getUrlParameter('BulkUploadReview');

            bulkUploadRepository.refreshReview(batchId).$promise.then(
                function (resp) {
                    $scope.batch = resp;
                    $scope.buildResults();
                },
                function (respError) {

                });
        } else {
            $scope.buildResults();
        }
        $scope.resetFilters = function () {
            // needs to be a function or it won't trigger a $watch
            $scope.search = null;
        };

        $scope.confirmImport = function () {
            $scope.batchUpload = $scope.batch;
            $scope.batchUpload.assets = null;
            $scope.batchUpload.selectedTenant = $scope.selectedTenant;
            bulkUploadRepository.confirmImport($scope.batchUpload).$promise.then(
                function (resp) {
                    toastr.success(resp.message);
                },
                function (respErr) {
                    toastr.error("Import failed...");
                }
            );
        };
    }
]);

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.href),
        sURLVariables = sPageURL.split('/'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i];

        if (sParameterName === sParam) {
            var requiredId = sURLVariables[i + 1];
            return requiredId === undefined ? true : requiredId;
        }
    }
};