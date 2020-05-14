(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('picofheadline-application').controller('picOfHeadlineController',
        ['$scope', '$window', '$picOfHeadlineService', '$uibModal',
        function ($scope, $window, $picOfHeadlineService, $uibModal) {
            $scope.errorMessage = '';
            $scope.siteOptions = null;
            $scope.statusOptions = null;
            $scope.positionOptions = null;
            $scope.currencyOptions = null;
            $scope.defaultFocusOptions = null;
            $scope.downloadFileOptions = null;
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
                IsGetFromCached: false,
                ExcludeIgnoredAccounts: true
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
                    OnChangeEvent: $scope.updateSelectedPosition,
                    AllText: viewResources.All,
                    MinWidth: 200,
                    MaxHeight: 450,
                    NoItemText: viewResources.No_languages_found,
                    NoneSelectedText: viewResources.Symbol_NotApply,
                    NumberOfItemShowShortText: 2
                };

                $scope.currencyOptions = {
                    DataList: []
                };

                $scope.statusOptions = {
                    DataList: [
                        {
                            Id: 0,
                            Name: 'All'
                        },
                        {
                            Id: 1,
                            Name: 'Open'
                        },
                        {
                            Id: 2,
                            Name: 'Suspended'
                        },
                        {
                            Id: 3,
                            Name: 'Closed'
                        },
                        {
                            Id: 4,
                            Name: 'Disabled'
                        }
                    ]
                };
            };
            $scope.getAllSetting = function () {
                $scope.BlockUI();
                $scope.isLoadedAllSite = false;
                $picOfHeadlineService.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.siteOptions.DataList = allSetting.SiteList;
                        $scope.positionOptions.DataList = allSetting.PositionList;
                        $scope.currencyOptions.DataList = allSetting.CurrencyList;
                        $scope.completedLoadFilterInfo();
                    }, function () {
                        $scope.completedLoadFilterInfo();
                    });
            };
            $scope.completedLoadFilterInfo = function () {
                $scope.unblockUI();
            };
            $scope.updateSelectedSite = function (siteIdList) {
                if (!siteIdList) {
                    $('#site--1').prop('checked', true);
                    return;
                }
                $scope.searchCriteria.SiteIds = siteIdList;
            };
            $scope.updateSelectedPosition = function (positionIdList) {
                $scope.searchCriteria.PositionIds = positionIdList;
                if (!positionIdList) {
                    $('#position--1').prop('checked', true);
                    return;
                }
            };

            $scope.invokeHotKey_CtrlS = function () {
                $scope.getCustomerReport();
            };
            $scope.getCustomerReport = function () {
                if (!$scope.searchCriteria.PositionIds) {
                    $scope.errorMessage = "Please choose a position";
                    return;
                }
                else {
                    $scope.errorMessage = '';
                }

                $scope.BlockUI();
                var getReportCriteria = {
                    SiteIds: $scope.searchCriteria.SiteIds,
                    Headline: $scope.searchCriteria.Headline,
                    PositionIds: $scope.searchCriteria.PositionIds,
                    ExcludeIgnoredAccounts: $scope.searchCriteria.ExcludeIgnoredAccounts,
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
                    }, function () {
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
                    }, function () {
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


            $scope.openAssignModal = function () {
                const modalInstance = $uibModal.open({
                    template: angular.element('#assign-pic-modal').html(),
                    controller: 'assignPICController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    backdropClass: 'related-customer-modal-backdrop',
                    windowClass: 'related-customer-modal'
                });
                modalInstance.result.then(function () {
                });
            };

            angular.element(document).ready(function () {
                $scope.initController();
                const parentHead = $window.parent.$('head');
                const style = $('head link[href*="/Areas/PicAndHeadline/PicOfHeadline/Styles/PicOfHeadline.less"]').clone();

                parentHead.append(style);
            });
        }]);
})();