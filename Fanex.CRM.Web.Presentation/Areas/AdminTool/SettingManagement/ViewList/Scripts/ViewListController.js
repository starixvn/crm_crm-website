"use strict";
/* global viewListApplication,
sharedScriptManager,
viewResources,
registerOptions,
registerUpdateFunctions,
registerHotkeyFunctions */
/* jshint unused: vars,
camelcase: false */

viewListApplication.controller('viewListController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'viewListService',
    function ($scope, $uiBlockerService, $resultMessageService, viewListService) {
        $scope.sharedScriptManager = sharedScriptManager;
        $scope.getSettingRelationsParams = {
            MainSetting: null
        };
        $scope.settingManagementView = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getMainSetting();
        };
        $scope.getMainSetting = function () {
            $uiBlockerService.blockUI();
            var params = {};
            viewListService.getMainSetting(params)
                .then(function (data) {
                    $scope.mainSettingOptions.DataList = data.Result;
                    $scope.completedLoadFilterInfo();
                },
                function (data) {
                    $scope.completedLoadFilterInfo();
                });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.updateSelectedMainSetting($scope.mainSettingOptions.DataList[0]);
        };
        $scope.getSettingRelations = function () {
            $uiBlockerService.blockUI();

            viewListService.getSettingRelations($scope.getSettingRelationsParams)
                .then(function (data) {
                    $scope.settingManagementView = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.settingManagementView = null;
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };

        $scope.insertNewSetting = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "SettingManagement/OpenInsertNewSettingView",
                viewResources.Insert_New_Setting,
                480,
                260,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeInsertView,
                ['center', 200],
                $scope.beforeCloseInsertView);
        };
        $scope.insertSubSetting = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "SettingManagement/OpenInsertSubSettingView?" + $.param({ settingType: $scope.getSettingRelationsParams.MainSetting.Type }),
                viewResources.Insert_New_Setting,
                480,
                280,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeInsertView,
                ['center', 200],
                $scope.beforeCloseInsertView);
        };

        $scope.beforeCloseInsertView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges, popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.closeInsertView = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.$apply();
            $scope.getSettingRelationsParams.MainSetting = null;
            $scope.getMainSetting();
        };
        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);
            $scope.initController();
            $scope.registerHotKey();
            $scope.setFocus();
        });
    } ]);