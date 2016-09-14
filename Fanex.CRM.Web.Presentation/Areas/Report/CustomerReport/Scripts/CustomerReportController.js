(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('report-customerreport-application').controller('customerReportController', ['$scope', '$customerReportService',
    function ($scope, $customerReportService) {
        $scope.isShowFilter = true;
        $scope.siteOptions = null;
        $scope.languageOptions = null;
        $scope.defaultFocusOptions = null;
        $scope.downloadFileOptions = null;
        $scope.vipTypeOptions = null;
        $scope.customerReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };

        $scope.searchCriteria = {
            Username: '',
            LanguageIds: '',
            SiteIds: '',
            VIPType: null,
            PageIndex: 1,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
            IsGetFromCached: false
        };
        $scope.lastSelectedParams = angular.copy($scope.searchCriteria);

        $scope.initController = function () {
            $scope.initPicker();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Report_CustomerReport"
            });

            $scope.getAllSetting();
            $scope.defaultFocusOptions = {
                IsSetFocus: false,
                OnChangeEvent: $scope.completeSetFocus
            };

            $scope.defaultFocusOptions.IsSetFocus = true;
            $scope.$apply();
        };
        $scope.initPicker = function () {
            $scope.vipTypeOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedVIPType,
                AllText: viewResources.All
            };

            $scope.languageOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedLanguage,
                AllText: viewResources.All_Languages,
                MinWidth: 200,
                MaxHeight: 450,
                NoItemText: viewResources.No_languages_found,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 2
            };

            $scope.siteOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedSite,
                AllText: viewResources.All_Sites,
                MinWidth: 350,
                MaxHeight: 450,
                NoItemText: viewResources.No_site_found,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 2,

                SubOptionsName: 'ChildSites',
                NumberOfSelectedLevel: 2
            };

            $scope.positionOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedLanguage,
                AllText: viewResources.All,
                MinWidth: 200,
                MaxHeight: 450,
                NoItemText: viewResources.No_languages_found,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 2
            };
        };
        $scope.getAllSetting = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $customerReportService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.siteOptions.DataList = allSetting.SiteList;
                $scope.vipTypeOptions.DataList = allSetting.VIPTypeList;
                $scope.languageOptions.DataList = allSetting.LanguageList;
                $scope.positionOptions.DataList = allSetting.PositionList;
                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.completedLoadFilterInfo();
            });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.searchCriteria.VIPType = $scope.vipTypeOptions.DataList[0];
            $scope.unblockUI();
        };
        $scope.updateSelectedLanguage = function (languageIdList) {
            if (!languageIdList) {
                $('#language--1').prop('checked', true);
                return;
            }

            $scope.searchCriteria.LanguageIds = languageIdList;
        };
        $scope.updateSelectedSite = function (siteIdList) {
            if (!siteIdList) {
                $('#site--1').prop('checked', true);
                return;
            }
            $scope.searchCriteria.SiteIds = siteIdList;
        };
        $scope.updateSelectedPosition = function (positionIdList) {
            if (!positionIdList) {
                $('#position--1').prop('checked', true);
                return;
            }
            $scope.searchCriteria.PositionIds = positionIdList;
        };
        $scope.updateSelectedVIPType = function (vipType) {
            $scope.searchCriteria.VIPType = vipType;
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.getCustomerReport();
        };
        $scope.getCustomerReport = function () {
            $scope.BlockUI();
            var getReportCriteria = {
                Username: $scope.searchCriteria.Username,
                LanguageIds: $scope.searchCriteria.LanguageIds,
                SiteIds: $scope.searchCriteria.SiteIds,
                VIPTypeId: $scope.searchCriteria.VIPType.Id,
                Headline: $scope.searchCriteria.Headline,
                PositionIds: $scope.searchCriteria.PositionIds,
                IsExcludeIgnoredAccounts: $scope.searchCriteria.IsExcludeIgnoredAccounts,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize,
                IsGetFromCached: false
            };

            $scope.lastSelectedParams = getReportCriteria;
            $customerReportService.getCustomerReport(getReportCriteria)
            .then(function (data) {
                $scope.customerReportOptions.DataList = data.Result;
                $scope.customerReportOptions.IsUpdateGrid = true;
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
        };
        $scope.pagingCustomerReport = function (pageIndex, pageSize) {
            $scope.BlockUI();
            $scope.lastSelectedParams.PageIndex = pageIndex;
            $scope.lastSelectedParams.PageSize = pageSize;
            $scope.lastSelectedParams.IsGetFromCached = true;
            $customerReportService.getCustomerReport($scope.lastSelectedParams)
            .then(function (data) {
                $scope.customerReportOptions.DataList = data.Result;
                $scope.customerReportOptions.IsUpdateGrid = true;
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
        };
        
        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "CustomerReport/ExportCustomerReport",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply();
        };
        $scope.showHideFilterBox = function () {
            $scope.isShowFilter = !$scope.isShowFilter;
            $scope.customerReportOptions.IsUpdateGrid = true;
            $scope.customerReportOptions.IsResize = true;
        };
        $scope.completeSetFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = false;
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
    } ]);
})();