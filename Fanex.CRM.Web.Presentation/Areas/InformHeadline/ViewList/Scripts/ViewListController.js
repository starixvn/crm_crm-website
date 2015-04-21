"use strict";
/* global informHeadlineApplication,
sharedScriptManager,
registerOptions,
registerUpdateFunctions,
registerHotkeyFunctions,
viewResources, Site */
/* jshint unused: vars,
camelcase: false */

informHeadlineApplication.controller('ViewListController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'viewListService',
    function ($scope, $uiBlockerService, $resultMessageService, viewListService) {

        function handleUpdateFailed() {
            Site.showMessageBox(viewResources.UpdateFailed);
            $uiBlockerService.unblockUI();
        }

        $scope.listData = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getInforms();
        };

        $scope.getInforms = function () {
            $uiBlockerService.blockUI();

            viewListService.getInforms()
                .then(function (data) {
                    $scope.listData = data.Result.Items;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.listData = null;
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.exportInformHeadlines = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'InformHeadline/ExportInforms',
                Params: { IsCached: true }
            };

            $scope.$apply();
        };
        $scope.updateInformStatus = function (titleId, status) {
            $uiBlockerService.blockUI();

            viewListService.updateInformStatus({
                titleId: titleId,
                statusType: status.toString() === "1" // false : Open, true: Close
            })
                .then(function (data) {
                    if (data.Result.Code > 0) {
                        handleUpdateFailed();
                    } else {
                        $scope.getInforms();
                    }
                },
                handleUpdateFailed);
        };
        $scope.showAddCaseDialog = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "InformHeadline/NewInform",
                viewResources.NewInformHeadlines,
                1000,
                800,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditView,
                { my: "center", at: "center" },
                $scope.beforeCloseEditView);
        };
        $scope.showUpdateCaseDialog = function (titleId) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "InformHeadline/UpdateInform?" +
                    $.param({
                        titleId: titleId
                    }),
                viewResources.UpdateInformHeadlines,
                1000,
                800,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditView,
                { my: "center", at: "center" },
                $scope.beforeCloseEditView);
        };
        $scope.showContactDialog = function (titleId) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "InformHeadline/ContactInform?" +
                    $.param({
                        titleId: titleId
                    }),
                viewResources.UpdateInformHeadlines,
                1000,
                800,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditView,
                { my: "center", at: "center" },
                $scope.beforeCloseEditView);
        };
        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);
            $scope.initController();
            $scope.registerHotKey();
        });
    } ]);