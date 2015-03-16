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
                { my: "center top+200", at: "center top" },
                $scope.beforeCloseInsertView);
        };
        $scope.insertSubSetting = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "SettingManagement/OpenInsertSubSettingView?" + $.param({
                    settingType: $scope.getSettingRelationsParams.MainSetting.SettingType 
                }),
                viewResources.Insert_New_Setting,
                480,
                280,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeInsertView,
                { my: "center top+200", at: "center top" },
                $scope.beforeCloseInsertView);
        };
        $scope.deleteSetting = function (settingInfo) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "SettingManagement/OpenDeleteSettingView?" +
                    $.param({
                        settingId: settingInfo.Id,
                        settingType: settingInfo.SettingType,
                        settingValue: settingInfo.Value
                    }),
                viewResources.Delete_Setting,
                380,
                150,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeDeleteView,
                { my: "center top+200", at: "center top" },
                $scope.beforeCloseDeleteView);
        };
        $scope.editSetting = function (settingInfo) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "SettingManagement/OpenEditSettingView?" +
                    $.param({
                        settingId: settingInfo.Id
                    }),
                viewResources.Edit_Setting_Value,
                480,
                230,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditView,
                { my: "center top+200", at: "center top" },
                $scope.beforeCloseEditView);
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
        $scope.beforeCloseDeleteView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges, popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.closeDeleteView = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.$apply();
            $scope.getSettingRelationsParams.MainSetting = null;
            $scope.getMainSetting();
        };
        $scope.beforeCloseEditView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges, popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.closeEditView = function () {
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