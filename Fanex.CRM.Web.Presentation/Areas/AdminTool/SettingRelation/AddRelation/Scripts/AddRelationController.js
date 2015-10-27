(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    viewResources,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions,
    mainSetting */
    /* jshint unused: vars,
    camelcase: false */

    angular.module('addmintool-settingrelation-addrelation-application').controller('addRelationController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'addRelationService',
    function ($scope, $uiBlockerService, $resultMessageService, addRelationService) {
        $scope.getMainSettingRelationsParams = {
            ParentId: mainSetting.Id,
            MainSetting: mainSetting
        };
        $scope.addSettingRelationsParams = {
            ParentId: mainSetting.Id,
            SettingName: '',
            SubSettings: null
        };

        $scope.mainSettingRelationView = null;
        $scope.oldMainSettingRelationView = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getMainSettingForAddRelation();
        };
        $scope.getMainSettingForAddRelation = function () {
            $uiBlockerService.blockUI();
            addRelationService.getMainSettingForAddRelation($scope.getMainSettingRelationsParams)
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
            $uiBlockerService.unblockUI();
            $scope.getMainSettingRelations();
        };
        $scope.getMainSettingRelations = function () {
            $uiBlockerService.blockUI();

            addRelationService.getMainSettingRelations($scope.getMainSettingRelationsParams)
                .then(function (data) {
                    $scope.mainSettingRelationView = data.Result;
                    $scope.oldMainSettingRelationView = angular.copy($scope.mainSettingRelationView);
                    $scope.addSettingRelationsParams.SettingName = $scope.getMainSettingRelationsParams.MainSetting.SettingType;
                    $scope.addSettingRelationsParams.SubSettings = $scope.mainSettingRelationView;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.mainSettingRelationView = null;
                    $scope.oldMainSettingRelationView = angular.copy($scope.mainSettingRelationView);
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
        $scope.addSettingRelations = function () {
            $uiBlockerService.blockUI();

            var subSettingLength = $scope.addSettingRelationsParams.SubSettings.length;
            for (var i = 0; i < subSettingLength; i++) {
                var subSetting = $scope.addSettingRelationsParams.SubSettings[i];
                $scope.addSettingRelationsParams.SubSettings[i] = {
                    Id: subSetting.Id,
                    DisplayOrder: subSetting.DisplayOrder,
                    IsSubSetting: subSetting.IsSubSetting
                };
            }

            addRelationService.addSettingRelations($scope.addSettingRelationsParams)
                .then(function (data) {
                    $scope.showAddResult(data.Result);
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.showAddResult = function (result) {
            if (result.Code !== 0) {
                $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                return;
            }

            $scope.messageResultOptions = new MessageResult(result.Message, true, true);
            parent.homeManager.CachedData.shouldReloadView = true;
            $scope.oldMainSettingRelationView = angular.copy($scope.mainSettingRelationView);
            setTimeout(function () {
                $scope.messageResultOptions = new MessageResult('', false, false);
                parent.ClosePopup();
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };

        $scope.updateChangeFlag = function () {
            var isChange = false;
            var settingLength = $scope.mainSettingRelationView &&
                $scope.oldMainSettingRelationView ?
                $scope.mainSettingRelationView.length :
                0;
            for (var i = 0; i < settingLength; i++) {
                var setting = $scope.mainSettingRelationView[i];
                var oldSetting = $scope.oldMainSettingRelationView[i];
                if (setting.IsSubSetting !== oldSetting.IsSubSetting ||
                    setting.DisplayOrder !== oldSetting.DisplayOrder) {
                    isChange = true;
                    break;
                }
            }

            if (isChange) {
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_AddRelation_Changes;
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                return;
            }

            parent.homeManager.CachedData.isChangingDataPopUp = false;
        };
        $scope.ignoreChanges = function () {
            $scope.oldMainSettingRelationView = angular.copy($scope.mainSettingRelationView);
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
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });
            $scope.setFocus();
        });
    } ]);
})();