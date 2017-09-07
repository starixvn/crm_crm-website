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

        $scope.getReport = function () {
            console.log("get report");

            $scope.BlockUI();
            var getReportCriteria = {
                Headline: $scope.searchCriteria.Headline,
                ExcludeIgnoredAccounts: $scope.searchCriteria.ExcludeIgnoredAccounts,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize,
                IsGetFromCached: false
            };

            $scope.lastSelectedParams = getReportCriteria;
            $directCustomerReportService.getReport(getReportCriteria)
            .then(function (data) {
                console.log(data);
                $scope.customerReportOptions.DataList = data.Result;
                $scope.customerReportOptions.IsUpdateGrid = true;
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
        }

        $scope.showHideFilterBox = function () {
            $scope.isShowFilter = !$scope.isShowFilter;
            //$scope.customerReportOptions.IsUpdateGrid = true;
            //$scope.customerReportOptions.IsResize = true;
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
            console.log("inint controller");
            $scope.initController();
        });
    }]);   
})();