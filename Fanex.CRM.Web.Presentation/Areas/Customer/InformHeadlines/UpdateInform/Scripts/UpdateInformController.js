'use strict';
/*jshint unused: vars*/
/* jshint camelcase: false */
/* global updateInformHeadlineApplication,
updateInformResources,
sharedScriptManager,
hotKeyManager,
titleId,
title */
updateInformHeadlineApplication.controller('updateInformHeadlineController', ['$scope', 'updateInformHeadlineService', '$uiBlockerService', '$timeout',
function ($scope, updateInformService, $uiBlockerService, $timeout) {
    $scope.statusOptions = {};
    $scope.selectedStatusIds = '';
    $scope.titleId = titleId;
    $scope.title = title;
    $scope.oldTitle = title;
    $scope.informs = [];
    $scope.pagerOption = {
        itemsPerPage: 10,
        currentPage: 0,
        itemsPerPageArray: sharedScriptManager.cachedData.informHeadlinePagingOptions,
        totalItem: 0
    };
    $scope.alertResult = {
        IsSuccess: true,
        Message: ''
    };
    $scope.isStatusChanged = false;
    $scope.isTitleChanged = false;

    $scope.initController = function () {
        $uiBlockerService.blockUI();
        $scope.getStatusList();
    };

    $scope.getStatusList = function () {
        $scope.statusOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedStatus,
            AllText: updateInformResources.AllStatus,
            MaxHeight: 450,
            NoItemText: updateInformResources.Symbol_NotApply,
            NoneSelectedText: updateInformResources.No_status_found,
            NumberOfItemShowShortText: 0
        };

        updateInformService.getStatusList()
            .then(function (status) {
                $scope.statusOptions.DataList = status.Result;
                $scope.getInforms(0, 10);
            },
            function (status) {
                $uiBlockerService.unblockUI();
            });
    };

    $scope.getInforms = function (pageIndex, itemsPerPage) {
        $uiBlockerService.blockUI();
        $scope.pagerOption.currentPage = pageIndex;
        $scope.pagerOption.itemsPerPage = itemsPerPage;
        var informCriteria = { TitleId: $scope.titleId, Status: $scope.selectedStatusIds, PageIndex: ++pageIndex, ItemsPerPage: itemsPerPage };
        updateInformService.getInforms(informCriteria)
            .then(function (data) {
                var result = data.Result;
                $scope.informs = result.ChildItems;
                $scope.pagerOption.totalItem = result.TotalItem;

                $uiBlockerService.unblockUI();
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
    };

    $scope.getUpdatedInforms = function () {
        return $.grep($scope.informs, function (item, i) {
            return item.IsChanged;
        });
    };

    $scope.updateIsStatusChanged = function () {
        $scope.isStatusChanged = true;
        parent.homeManager.CachedData.isChangingDataPopUp = true;
    };

    $scope.isDataChanged = function () {
        return $scope.isStatusChanged || $scope.isTitleChanged;
    };

    $scope.showConfirmIgnoreChange = function (okCallback) {
        var homeManager = parent.homeManager;
        homeManager.CachedData.ignoreConfirmMessagePopUp = updateInformResources.Confirm_Ignore_UpdateInforms_Changes;
        homeManager.showConfirmIgnoreChangePopUp(function () {
            $scope.title = $scope.oldTitle;
            $scope.isStatusChanged = false;
            okCallback();
        });
    };

    $scope.updateInforms = function () {
        $scope.alertResult.Message = '';
        if ($scope.title.length === 0) {
            $scope.alertResult = {
                IsSuccess: false,
                Message: updateInformResources.UpdateInforms_TitleEmpty
            };
            return;
        }

        if (!$scope.isDataChanged()) { // informs are not changed
            $scope.alertResult = {
                IsSuccess: false,
                Message: updateInformResources.InformHeadline_NoDataChanged
            };
            return;
        }

        // filter updated informs
        $uiBlockerService.blockUI();
        var updatedInforms = $scope.getUpdatedInforms();
        var updateCriteria = { TitleId: $scope.titleId, Title: $scope.title, Informs: updatedInforms };
        updateInformService.updateInforms(updateCriteria)
            .then(function (data) {
                var result = sharedScriptManager.formatAjaxResult(data.Result);
                var isSuccess = result.Code === 0;
                $scope.alertResult = {
                    IsSuccess: isSuccess,
                    Message: isSuccess ? updateInformResources.UpdateInformsSuccess : result.Message
                };
                if (isSuccess) {
                    $timeout(function () {
                        $scope.alertResult.Message = '';
                    }, 3000);
                }

                $uiBlockerService.unblockUI();
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
    };

    $scope.updateSelectedStatus = function (statusIdList) {
        $scope.selectedStatusIds = statusIdList;
    };

    $scope.searchInform = function () {
        $scope.alertResult.Message = '';
        if ($scope.selectedStatusIds.length === 0) {
            $scope.alertResult = {
                IsSuccess: false,
                Message: updateInformResources.InformHeadline_NoStatusFound
            };
            return;
        }

        $scope.getInforms(0, 10);
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
    });
} ]);