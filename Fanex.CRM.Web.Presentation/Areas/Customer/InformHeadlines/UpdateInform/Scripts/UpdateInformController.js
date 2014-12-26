'use strict';
/* jshint unused: vars */
/* jshint camelcase: false */
/* global updateInformHeadlineApplication,
updateInformResources,
sharedScriptManager,
hotKeyManager,
crmConstants,
titleId */
updateInformHeadlineApplication.controller('updateInformHeadlineController',
    ['$scope',
    'updateInformHeadlineService',
    '$uiBlockerService',
    'alertBoxService',
    function ($scope, updateInformService, $uiBlockerService, alertBoxService) {
        $scope.statusOptions = {};
        $scope.positionOptions = {};
        $scope.currencyOptions = {};
        $scope.siteOptions = {};
        $scope.selectedStatusIds = '';
        $scope.selectedPositionIds = '';
        $scope.selectedCurrencyIds = '';
        $scope.selectedSiteIds = '';
        $scope.productStatus = [];
        $scope.extensions = [];
        $scope.titleId = titleId;
        $scope.title = '';
        $scope.NoHeadlinesToInform = 0;
        $scope.oldTitle = '';
        $scope.oldNoHeadlinesToInform = 0;
        $scope.username = '';
        $scope.informs = [];
        $scope.defaultItemPerPage = 10;
        $scope.pagerOption = {
            itemsPerPage: $scope.defaultItemPerPage,
            currentPage: 0,
            itemsPerPageArray: sharedScriptManager.cachedData.informHeadlinePagingOptions,
            totalItem: 0
        };
        $scope.isStatusChanged = false;
        $scope.isTitleChanged = false;
        $scope.isNoHeadlinesToInformChanged = false;

        $scope.isDataChanged = function () {
            return $scope.isStatusChanged ||
                    $scope.isTitleChanged ||
                    $scope.isNoHeadlinesToInformChanged;
        };

        $scope.initController = function () {
            $scope.getAllInformHeadlineSettings();
        };

        $scope.getAllInformHeadlineSettings = function () {
            $uiBlockerService.blockUI();
            $scope.statusOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedStatus,
                AllText: updateInformResources.AllStatus,
                MaxHeight: 450,
                NoItemText: updateInformResources.Symbol_NotApply,
                NoneSelectedText: updateInformResources.No_status_found,
                NumberOfItemShowShortText: 0
            };

            $scope.positionOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedPosition,
                AllText: updateInformResources.AllPosition,
                MaxHeight: 450,
                NoItemText: updateInformResources.Symbol_NotApply,
                NoneSelectedText: updateInformResources.No_status_found,
                NumberOfItemShowShortText: 0
            };

            $scope.currencyOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCurrency,
                AllText: updateInformResources.AllCurrency,
                MaxHeight: 450,
                NoItemText: updateInformResources.Symbol_NotApply,
                NoneSelectedText: updateInformResources.No_status_found,
                NumberOfItemShowShortText: 0
            };

            $scope.siteOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedSite,
                AllText: updateInformResources.AllSite,
                MaxHeight: 450,
                NoItemText: updateInformResources.Symbol_NotApply,
                NoneSelectedText: updateInformResources.No_status_found,
                NumberOfItemShowShortText: 0,
                AutoCheckAll: false
            };

            updateInformService.getAllInformHeadlineSettings()
                .then(function (settings) {
                    $scope.statusOptions.DataList = settings.Result.Status;
                    $scope.positionOptions.DataList = settings.Result.Position;
                    $scope.currencyOptions.DataList = settings.Result.Currency;
                    $scope.siteOptions.DataList = settings.Result.Site;
                    $scope.productStatus = settings.Result.ProductStatus;
                    $scope.extensions = settings.Result.Extensions;
                    $scope.getInforms(0, $scope.defaultItemPerPage);
                },
                function (settings) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.updateSelectedStatus = function (statusIdList) {
            $scope.selectedStatusIds = statusIdList;
        };

        $scope.updateSelectedPosition = function (positionIdList) {
            $scope.selectedPositionIds = positionIdList;
        };

        $scope.updateSelectedCurrency = function (currencyIdList) {
            $scope.selectedCurrencyIds = currencyIdList;
        };

        $scope.updateSelectedSite = function (siteIdList) {
            $scope.selectedSiteIds = siteIdList;
        };

        $scope.getInforms = function (pageIndex, itemsPerPage) {
            $uiBlockerService.blockUI();
            $scope.pagerOption.currentPage = pageIndex;
            $scope.pagerOption.itemsPerPage = itemsPerPage;
            var informCriteria = { TitleId: $scope.titleId,
                Status: $scope.selectedStatusIds,
                Position: $scope.selectedPositionIds,
                CurrencyId: $scope.selectedCurrencyIds,
                Site: updateInformService.isSiteCheckAll() ? '' : $scope.selectedSiteIds,
                Username: $scope.username,
                PageIndex: ++pageIndex,
                ItemsPerPage: itemsPerPage
            };
            updateInformService.getInforms(informCriteria)
                .then(function (data) {
                    var result = data.Result;
                    $scope.title = result.Title;
                    $scope.NoHeadlinesToInform = result.NoHeadlinesToInform;
                    $scope.oldTitle = result.Title;
                    $scope.oldNoHeadlinesToInform = result.NoHeadlinesToInform;
                    $scope.informs = result.ChildItems;
                    $scope.pagerOption.totalItem = result.TotalItem;

                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.updateIsStatusChanged = function () {
            $scope.isStatusChanged = true;
            parent.homeManager.CachedData.isChangingDataPopUp = true;
        };

        $scope.showConfirmIgnoreChange = function (okCallback) {
            var homeManager = parent.homeManager;
            homeManager.CachedData.ignoreConfirmMessagePopUp = updateInformResources.Confirm_Ignore_UpdateInforms_Changes;
            homeManager.showConfirmIgnoreChangePopUp(function () {
                $scope.title = $scope.oldTitle;
                $scope.NoHeadlinesToInform = $scope.oldNoHeadlinesToInform;
                $scope.isStatusChanged = false;
                okCallback();
            });
        };

        $scope.updateInforms = function () {
            alertBoxService.hide();
            if ($scope.title.length === 0) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: updateInformResources.UpdateInforms_TitleEmpty
                });
                return;
            }

            if (!$scope.isDataChanged()) { // informs are not changed
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: updateInformResources.InformHeadline_NoDataChanged
                });
                return;
            }

            // filter updated informs
            $uiBlockerService.blockUI();
            var updatedInforms = $.grep($scope.informs, function (item, i) {
                return item.IsChanged;
            });
            var updateCriteria = {
                TitleId: $scope.titleId,
                NoHeadLine: $scope.NoHeadlinesToInform,
                Title: $scope.title,
                Informs: updatedInforms,
                IsTitleChanged: $scope.title !== $scope.oldTitle,
                IsNoHeadlinesToInformChanged: $scope.NoHeadlinesToInform !== $scope.oldNoHeadlinesToInform
            };
            updateInformService.updateInforms(updateCriteria)
                .then(function (data) {
                    var result = sharedScriptManager.formatAjaxResult(data.Result);
                    var alertResult;
                    if (result.Code === 0) {
                        alertResult = {
                            type: crmConstants.Enums.MessageType.Success,
                            message: updateInformResources.UpdateInformsSuccess
                        };
                    }
                    else {
                        alertResult = {
                            type: crmConstants.Enums.MessageType.Error,
                            message: result.Message
                        };
                    }

                    alertBoxService.show(alertResult);
                    $uiBlockerService.unblockUI();

                    // reload after saving.
                    $scope.getInforms($scope.pagerOption.currentPage, $scope.pagerOption.itemsPerPage);
                    $scope.isTitleChanged = false;
                    $scope.isStatusChanged = false;
                    $scope.isNoHeadlinesToInformChanged = false;
                    parent.homeManager.CachedData.isChangingDataPopUp = false;
                    // reload parent form (Inform Statistic)
                    parent.homeManager.CachedData.shouldReloadView = true;
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.searchInform = function () {
            alertBoxService.hide();
            if ($scope.selectedStatusIds.length === 0) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: updateInformResources.InformHeadline_NoStatusFound
                });
                return;
            }

            if ($scope.selectedPositionIds.length === 0) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: updateInformResources.InformHeadline_NoPositionFound
                });
                return;
            }

            if ($scope.selectedCurrencyIds.length === 0) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: updateInformResources.InformHeadline_NoCurrencyFound
                });
                return;
            }

            if ($scope.selectedSiteIds.length === 0) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: updateInformResources.InformHeadline_NoSiteFound
                });
                return;
            }

            $scope.getInforms(0, $scope.defaultItemPerPage);
        };

        $scope.exportInformsExcel = function () {
            var params = { 
                TitleId: $scope.titleId,
                Status: $scope.selectedStatusIds,
                Position: $scope.selectedPositionIds,
                CurrencyId: $scope.selectedCurrencyIds,
                Site: updateInformService.isSiteCheckAll() ? '' : $scope.selectedSiteIds,
                Username: $scope.username
            };

            var homeManager = parent.homeManager;
            if (!homeManager.CachedData.isChangingDataPopUp) {
                $scope.downloadFileOptions = {
                    URL: window.rootUrl + 'InformHeadlines/ExportInforms',
                    Params: params
                };
                return;
            }

            homeManager.CachedData.ignoreConfirmMessagePopUp = updateInformResources.Confirm_Export_InformStatus;
            homeManager.showConfirmExportExcelPopUp(function (result) {
                if (result) {
                    $scope.updateInforms();
                    setTimeout(function () {
                        $scope.downloadFileOptions = {
                            URL: window.rootUrl + 'InformHeadlines/ExportInforms',
                            Params: params
                        };
                    }, 3000);
                }
                else {
                    $scope.downloadFileOptions = {
                        URL: window.rootUrl + 'InformHeadlines/ExportInforms',
                        Params: params
                    };

                    $scope.$apply();
                }
            });
        };

        $scope.invokeHotKey_CtrlE = function () {
            $scope.searchInform();
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.updateInforms();
        };

        angular.element(document).ready(function () {
            $scope.initController();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'UpdateNewInformHeadlines'
            });

            $scope.$watch('title', function (newValue, oldValue) {
                $scope.isTitleChanged = newValue !== $scope.oldTitle;
                parent.homeManager.CachedData.isChangingDataPopUp = $scope.isDataChanged();
            });

            $scope.$watch('NoHeadlinesToInform', function (newValue, oldValue) {
                var arr = String(newValue).split('');
                var length = arr.length;
                if (length === 0) { return; }
                if (length === 1 && (arr[0] === '-' || arr[0] === '.')) { return; }
                if (length === 2 && newValue === '-.') { return; }
                if (isNaN(newValue)) {
                    $scope.NoHeadlinesToInform = oldValue;
                }
                else {
                    $scope.isNoHeadlinesToInformChanged = newValue !== $scope.oldNoHeadlinesToInform;
                    parent.homeManager.CachedData.isChangingDataPopUp = $scope.isDataChanged();
                }
            });
        });
    } ]);