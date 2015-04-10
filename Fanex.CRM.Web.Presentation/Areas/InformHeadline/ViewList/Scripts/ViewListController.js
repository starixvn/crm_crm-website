"use strict";
/* global viewListApplication,
sharedScriptManager,
registerOptions,
registerUpdateFunctions,
registerHotkeyFunctions,
viewResources, Site */
/* jshint unused: vars,
camelcase: false */

viewListApplication.controller('viewListController',
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
            console.log(parent);
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "InformHeadline/NewInform?" +
                    $.param({
                        settingId: 1
                    }),
                viewResources.NewInformHeadlines,
                900,
                700,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditView,
                ['center', 50],
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