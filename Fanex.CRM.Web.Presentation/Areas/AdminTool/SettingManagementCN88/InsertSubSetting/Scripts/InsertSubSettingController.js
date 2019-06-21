(function () {
    "use strict";
    /* global sharedScriptManager,
    viewResources,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions,
    MessageResult,
    crmConstants,
    settingInfo */
    /* jshint camelcase: false */

    angular.module('addmintool-settingmanagement-insertsubsetting-application').controller('insertSubSettingController',
    ['$scope', '$uiBlockerService', 'insertSubSettingService',
    function ($scope, $uiBlockerService, insertSubSettingService) {
        $scope.sharedScriptManager = sharedScriptManager;
        $scope.insertSettingParams = {
            NewSetting: {
                ParentId: 0,
                SettingName: settingInfo.SettingName,
                SettingValue: '',
                SettingNameCS: settingInfo.SettingNameCS,
                SettingValueCS: '',
                SettingNameCT: settingInfo.SettingNameCT,
                SettingValueCT: '',
                DisplayOrder: ''
            }
        };
        $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getParentSetting();
        };
        $scope.getParentSetting = function () {
            $uiBlockerService.blockUI();
            var params = {};
            insertSubSettingService.getParentSetting(params)
                .then(function (data) {
                    $scope.parentSettingOptions.DataList = data.Result;
                    $scope.completedLoadFilterInfo();
                },
                function () {
                    $scope.completedLoadFilterInfo();
                });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.updateSelectedParentSetting({
                Id: crmConstants.Enums.Setting.NotApply,
                SettingType: viewResources.Symbol_NotApply,
                Value: viewResources.Symbol_NotApply
            });
            $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
            $uiBlockerService.unblockUI();
        };
        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };

        $scope.insertSubSetting = function () {
            $uiBlockerService.blockUI();
            if (!$scope.checkInsertParam($scope.insertSettingParams)) {
                $uiBlockerService.unblockUI();
                return;
            }

            insertSubSettingService.insertSubSetting($scope.insertSettingParams)
                .then(function (data) {
                    $scope.showInsertResult(data.Result);
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.checkInsertParam = function (insertParam) {
            if (insertParam.NewSetting.SettingName === ""
                || insertParam.NewSetting.SettingNameCS === ""
                || insertParam.NewSetting.SettingNameCT === "") {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingName, true, false);
                return false;
            }

            if (insertParam.NewSetting.SettingValue === ""
                || insertParam.NewSetting.SettingValueCS === ""
                || insertParam.NewSetting.SettingValueCT === "") {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingValue, true, false);
                return false;
            }

            if (insertParam.NewSetting.SettingName.length > 100
                || insertParam.NewSetting.SettingNameCS.length > 100
                || insertParam.NewSetting.SettingNameCT.length > 100) {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedName, true, false);
                return false;
            }

            if (insertParam.NewSetting.SettingValue.length > 100
                || insertParam.NewSetting.SettingValueCS.length > 100
                || insertParam.NewSetting.SettingValueCT.length > 100) {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedValue, true, false);
                return false;
            }

            if (insertParam.NewSetting.DisplayOrder > 2100000000) {
                $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_DisplayOrder, true, false);
                return false;
            }

            $scope.messageResultOptions = new MessageResult("", false, false);
            return true;
        };
        $scope.showInsertResult = function (result) {
            if (result.Code !== 0) {
                $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                return;
            }

            $scope.messageResultOptions = new MessageResult(result.Message, true, true);
            parent.homeManager.CachedData.shouldReloadView = true;
            $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
            setTimeout(function () {
                $scope.messageResultOptions = new MessageResult('', false, false);
                parent.ClosePopup();
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };
        $scope.updateChangeFlag = function () {
            if ($scope.oldInsertSettingParams.NewSetting &&
                ($scope.oldInsertSettingParams.NewSetting.SettingName !== $scope.insertSettingParams.NewSetting.SettingName ||
                $scope.oldInsertSettingParams.NewSetting.SettingNameCS !== $scope.insertSettingParams.NewSetting.SettingNameCS ||
                $scope.oldInsertSettingParams.NewSetting.SettingNameCT !== $scope.insertSettingParams.NewSetting.SettingNameCT ||
                $scope.oldInsertSettingParams.NewSetting.SettingValue !== $scope.insertSettingParams.NewSetting.SettingValue ||
                $scope.oldInsertSettingParams.NewSetting.SettingValueCS !== $scope.insertSettingParams.NewSetting.SettingValueCS ||
                $scope.oldInsertSettingParams.NewSetting.SettingValueCT !== $scope.insertSettingParams.NewSetting.SettingValueCT ||
                $scope.oldInsertSettingParams.NewSetting.DisplayOrder !== $scope.insertSettingParams.NewSetting.DisplayOrder ||
                $scope.oldInsertSettingParams.NewSetting.ParentId !== $scope.insertSettingParams.NewSetting.ParentId)) {
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_InsertSetting_Changes;
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                return;
            }

            parent.homeManager.CachedData.isChangingDataPopUp = false;
        };
        $scope.ignoreChanges = function () {
            $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
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