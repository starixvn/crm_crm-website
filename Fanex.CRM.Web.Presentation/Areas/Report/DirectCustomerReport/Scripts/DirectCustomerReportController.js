(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('report-directcustomerreport-application').controller('directCustomerReportController', ['$scope', '$directCustomerReportService',
    function ($scope, $directCustomerReportService) {
        $scope.downloadFileOptions = null;
        $scope.isShowFilter = true;
        $scope.customerReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };
        $scope.searchCriteria = {
            Headline: '',
            PageIndex: 1,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
            IsGetFromCached: false,
            ExcludeIgnoredAccounts: true
        };


        $scope.initController = function () {
            $scope.$apply();
        };

        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "DirectCustomerReport/ExportReport",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply(); console.log("export");
        };

        var getReport = function (criteria) {
            $scope.BlockUI();

            $directCustomerReportService.getReport(criteria)
           .then(function (data) {
               $scope.customerReportOptions.DataList = data.Result;
               $scope.customerReportOptions.IsUpdateGrid = true;
               $scope.unblockUI();
           },
           function () {
               $scope.unblockUI();
           });
        }

        $scope.getReport = function () {
            var getReportCriteria = {
                Headline: $scope.searchCriteria.Headline,
                ExcludeIgnoredAccounts: $scope.searchCriteria.ExcludeIgnoredAccounts,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize,
                IsGetFromCached: false
            };
            $scope.lastSelectedParams = getReportCriteria;

            getReport(getReportCriteria);
        }

        $scope.pagingCustomerReport = function (pageIndex, pageSize) {
            $scope.lastSelectedParams.PageIndex = pageIndex;
            $scope.lastSelectedParams.PageSize = pageSize;
            $scope.lastSelectedParams.IsGetFromCached = true;

            getReport($scope.lastSelectedParams);            
        };

        $scope.showHideFilterBox = function () {
            $scope.isShowFilter = !$scope.isShowFilter;
            $scope.customerReportOptions.IsUpdateGrid = true;
            $scope.customerReportOptions.IsResize = true;
        };

        $scope.unblockUI = function () {
            try {
                if (parent && parent.$.unblockUI) {
                    parent.$.unblockUI();
                }
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };
        $scope.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                if (parent && parent.BlockUI) {
                    parent.BlockUI();
                }
            } catch (e) {
            }
        };
       
        angular.element(document).ready(function () {
            $scope.initController();
        });
    }]);   
})();