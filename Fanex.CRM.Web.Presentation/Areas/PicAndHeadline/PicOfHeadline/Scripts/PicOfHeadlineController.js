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
            $scope.picOfHeadlineOptions = {
                DataList: null,
                IsResize: false,
                IsUpdateGrid: false
            };
            $scope.searchCriteria = {
                SiteID: '',
                Currency: '',
                Status: '',
                HeadlineType: '',
                PIC: '',
                DirectAccount: '',
                GroupHeadline: '',
                Position: '',
                ExcludeIgnoredAcc: false,
                ExcludeIsTest: false,
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
                    OnChangeEvent: $scope.updateSelectedSite
                };

                $scope.positionOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedPosition,
                    AllText: viewResources.All
                };

                $scope.currencyOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedCurrency
                };

                $scope.statusOptions = {
                    DataList: [
                        {
                            Id: 'Open',
                            Name: 'Open'
                        },
                        {
                            Id: 'Suspended',
                            Name: 'Suspended'
                        },
                        {
                            Id: 'Closed',
                            Name: 'Closed'
                        },
                        {
                            Id: 'Disabled',
                            Name: 'Disabled'
                        }
                    ],
                    OnChangeEvent: $scope.updateSelectedStatus
                };
            };
            $scope.getAllSetting = function () {
                $scope.BlockUI();
                $scope.isLoadedAllSite = false;
                $picOfHeadlineService.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.siteOptions.DataList = allSetting.SiteList.map(site => {
                            return {
                                Id: site.SiteID,
                                Name: site.SiteName
                            };
                        });
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
                $scope.searchCriteria.SiteID = siteIdList;

                if (!siteIdList) {
                    $('#site--1').prop('checked', true);
                    return;
                }
            };

            $scope.updateSelectedPosition = function (positionIdList) {
                $scope.searchCriteria.Position = positionIdList;
                if (!positionIdList) {
                    $('#position--1').prop('checked', true);
                    return;
                }
            };

            $scope.updateSelectedCurrency = function (currencyList) {
                $scope.searchCriteria.Currency = currencyList;
                if (!currencyList) {
                    $('#currency--1').prop('checked', true);
                    return;
                }
            }

            $scope.updateSelectedStatus = function (statusList) {
                $scope.searchCriteria.Status = statusList;
                if (!statusList) {
                    $('#status--1').prop('checked', true);
                    return;
                }
            }

            $scope.invokeHotKey_CtrlS = function () {
                $scope.getPICAssignment();
            };

            $scope.getPICAssignment = function () {
                $scope.BlockUI();
                var getReportCriteria = {
                    SiteID: $scope.searchCriteria.SiteID,
                    Currency: $scope.searchCriteria.Currency,
                    Status: $scope.searchCriteria.Status,
                    HeadlineType: $scope.searchCriteria.HeadlineType,
                    PIC: $scope.searchCriteria.PIC,
                    DirectAccount: $scope.searchCriteria.DirectAccount,
                    GroupHeadline: $scope.searchCriteria.GroupHeadline,
                    Position: $scope.searchCriteria.Position,
                    ExcludeIgnoredAcc: $scope.searchCriteria.ExcludeIgnoredAcc,
                    ExcludeIsTest: $scope.searchCriteria.ExcludeIsTest,
                    PageIndex: $scope.searchCriteria.PageIndex,
                    PageSize: $scope.searchCriteria.PageSize,
                };

                $scope.lastSelectedParams = getReportCriteria;
                $picOfHeadlineService.getPICAssignment(getReportCriteria)
                    .then(function (data) {
                        $scope.picOfHeadlineOptions = {
                            DataList : data.Result,
                            IsUpdateGrid : true
                        };
                        $scope.unblockUI();

                    }, function () {
                        $scope.unblockUI();
                    });
            };
            $scope.pagingCustomerReport = function (pageIndex, pageSize) {
                $scope.BlockUI();
                $scope.searchCriteria.PageIndex = pageIndex;
                $scope.searchCriteria.PageSize = pageSize;
                $picOfHeadlineService.getPICAssignment($scope.searchCriteria)
                    .then(function (data) {
                        $scope.picOfHeadlineOptions = {
                            DataList: data.Result,
                            IsUpdateGrid: true
                        };
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