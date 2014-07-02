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
        $scope.selectedStatusIds = '';
        $scope.titleId = titleId;
        $scope.title = '';
        $scope.oldTitle = '';
        $scope.informs = [];
        $scope.defaultItemPerPage = 20;
        $scope.pagerOption = {
            itemsPerPage: $scope.defaultItemPerPage,
            currentPage: 0,
            itemsPerPageArray: sharedScriptManager.cachedData.informHeadlinePagingOptions,
            totalItem: 0
        };
        $scope.isStatusChanged = false;
        $scope.isTitleChanged = false;

        $scope.isDataChanged = function () {
            return $scope.isStatusChanged || $scope.isTitleChanged;
        };

        $scope.initController = function () {
            $scope.getStatusList();
        };

        $scope.getStatusList = function () {
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

            updateInformService.getStatusList()
                .then(function (status) {
                    $scope.statusOptions.DataList = status.Result;
                    $scope.getInforms(0, $scope.defaultItemPerPage);
                },
                function (status) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.updateSelectedStatus = function (statusIdList) {
            $scope.selectedStatusIds = statusIdList;
        };

        $scope.getInforms = function (pageIndex, itemsPerPage) {
            $uiBlockerService.blockUI();
            $scope.pagerOption.currentPage = pageIndex;
            $scope.pagerOption.itemsPerPage = itemsPerPage;
            var informCriteria = { TitleId: $scope.titleId, Status: $scope.selectedStatusIds, PageIndex: ++pageIndex, ItemsPerPage: itemsPerPage };
            updateInformService.getInforms(informCriteria)
                .then(function (data) {
                    var result = data.Result;
                    $scope.title = result.Title;
                    $scope.oldTitle = result.Title;
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
            var updateCriteria = { TitleId: $scope.titleId, Title: $scope.title, Informs: updatedInforms, IsTitleChanged: $scope.title !== $scope.oldTitle };
            updateInformService.updateInforms(updateCriteria)
                .then(function (data) {
                    var result = sharedScriptManager.formatAjaxResult(data.Result);
                    var alertResult;
                    if (result.Code === 0) {
                        alertResult = { 
                            type: crmConstants.Enums.MessageType.Success,
                            message: updateInformResources.UpdateInformsSuccess };
                    }
                    else {
                        alertResult = {
                            type: crmConstants.Enums.MessageType.Error,
                            message: result.Message };
                    }

                    alertBoxService.show(alertResult);
                    $uiBlockerService.unblockUI();

                    // reload after saving.
                    $scope.getInforms($scope.pagerOption.currentPage, $scope.pagerOption.itemsPerPage);
                    $scope.isTitleChanged = false;
                    $scope.isStatusChanged = false;
                    parent.homeManager.CachedData.isChangingDataPopUp = false;
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

            $scope.getInforms(0, $scope.defaultItemPerPage);
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