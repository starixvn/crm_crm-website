(function () {
    "use strict";
    /* global sharedScriptManager,
    viewResources,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions,
    MessageResult,
    settingInfo */
    /* jshint unused: vars,
    camelcase: false */

    angular.module('addmintool-settingmanagement-editsetting-application').controller('editSettingController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'editSettingService',
    function ($scope, $uiBlockerService, $resultMessageService, editSettingService) {
        $scope.sharedScriptManager = sharedScriptManager;
        $scope.editSettingParams = {
            OldSetting: angular.copy(settingInfo),
            UpdatedSetting: angular.copy(settingInfo)
        };
        $scope.oldEditSettingParams = angular.copy($scope.editSettingParams);
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
        };

        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.editSetting = function () {
            $uiBlockerService.blockUI();
            if (!$scope.checkEditParam($scope.editSettingParams)) {
                $uiBlockerService.unblockUI();
                return;
            }

            editSettingService.editSetting($scope.editSettingParams)
                .then(function (data) {
                    $scope.showEditResult(data.Result);
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.checkEditParam = function (editParam) {
            if (editParam.UpdatedSetting.SettingType === "") {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingName, true, false);
                return false;
            }

            if (editParam.UpdatedSetting.Value === "") {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingValue, true, false);
                return false;
            }

            if (editParam.UpdatedSetting.SettingType.length > 100) {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedName, true, false);
                return false;
            }

            if (editParam.UpdatedSetting.Value.length > 100) {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedValue, true, false);
                return false;
            }

            if (editParam.UpdatedSetting.DisplayOrder > 2100000000) {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_DisplayOrder, true, false);
                return false;
            }

            $scope.messageResultOptions = new MessageResult("", false, false);
            return true;
        };
        $scope.showEditResult = function (result) {
            if (result.Code !== 0) {
                $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                return;
            }

            $scope.messageResultOptions = new MessageResult(result.Message, true, true);
            parent.homeManager.CachedData.shouldReloadView = true;
            $scope.oldEditSettingParams = angular.copy($scope.editSettingParams);
            setTimeout(function () {
                $scope.messageResultOptions = new MessageResult('', false, false);
                parent.ClosePopup();
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };
        $scope.updateChangeFlag = function () {
            if ($scope.oldEditSettingParams.UpdatedSetting &&
                ($scope.oldEditSettingParams.UpdatedSetting.SettingType !== $scope.editSettingParams.UpdatedSetting.SettingType ||
                $scope.oldEditSettingParams.UpdatedSetting.Value !== $scope.editSettingParams.UpdatedSetting.Value ||
                $scope.oldEditSettingParams.UpdatedSetting.DisplayOrder !== $scope.editSettingParams.UpdatedSetting.DisplayOrder ||
                $scope.oldEditSettingParams.UpdatedSetting.ParentId !== $scope.editSettingParams.UpdatedSetting.ParentId)) {
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_EditSetting_Changes;
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                return;
            }

            parent.homeManager.CachedData.isChangingDataPopUp = false;
        };
        $scope.ignoreChanges = function () {
            $scope.oldEditSettingParams = angular.copy($scope.editSettingParams);
            $scope.$apply();
            $scope.updateChangeFlag();
            parent.ClosePopup();
        };
        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);
            $scope.initController();
            $scope.registerHotKey();
            $scope.setFocus();
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });
        });
    } ]);
})();