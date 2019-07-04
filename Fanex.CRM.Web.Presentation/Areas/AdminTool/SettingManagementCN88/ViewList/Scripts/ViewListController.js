(function () {
    "use strict";
    /* global sharedScriptManager,
    viewResources,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions */
    /* jshint camelcase: false */

    angular.module('addmintool-settingmanagement-application').controller('viewListController',
    ['$scope', '$uiBlockerService', 'viewListService',
    function ($scope, $uiBlockerService, viewListService) {
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
                function () {
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
                function () {
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
                window.rootUrl + "SettingManagementCN88/OpenInsertNewSettingView",
                viewResources.Insert_New_Setting,
                880,
                230,
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
                window.rootUrl + "SettingManagementCN88/OpenInsertSubSettingView?" + $.param({
                    SettingName: $scope.getSettingRelationsParams.MainSetting.SettingType
                }),
                viewResources.Insert_New_Setting,
                880,
                230,
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
            parent.homeManager.CachedData.shouldReloadView = true;
            var title = settingInfo.IsActive === true ? viewResources.DeactiveSetting : viewResources.ActiveSetting;
            parent.OpenPopup(
                window.rootUrl + "SettingManagementCN88/OpenDeleteSettingView?" +
                    $.param({
                        settingId: settingInfo.SettingID,                        
                        isActive: settingInfo.IsActive
                    }),
                title,
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
                window.rootUrl + "SettingManagementCN88/OpenEditSettingView?" +
                    $.param({
                        settingId: settingInfo.SettingID
                    }),
                viewResources.Edit_Setting_Value,
                880,
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
            $scope.getSettingRelations();
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
            $scope.getSettingRelations();
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
            $scope.getSettingRelations();
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
})();