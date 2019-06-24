(function () {
    "use strict";
    /* global sharedScriptManager,
    viewResources,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions */
    /* jshint camelcase: false */

    angular.module('addmintool-settingrelation-application').controller('viewListController',
    ['$scope', '$uiBlockerService', 'viewListService',
    function ($scope, $uiBlockerService, viewListService) {
        $scope.getSettingRelationsParams = {
            MainSetting: null
        };
        $scope.settingRelationView = null;
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
            $scope.getSettingRelations();
        };
        $scope.getSettingRelations = function () {
            $uiBlockerService.blockUI();

            viewListService.getSettingRelations($scope.getSettingRelationsParams)
                .then(function (data) {
                    $scope.settingRelationView = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $scope.settingRelationView = null;
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
        $scope.addRelation = function (setting) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "SettingRelationCN88/OpenInsertRelationView?" +
                    $.param({
                        settingId: setting.Id,
                        settingValue: setting.Value,
                        settingType: setting.SettingType
                    }),
                viewResources.Setting_Relation_Management,
                585,
                600,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeInsertView,
                { my: "center top+200", at: "center top" },
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