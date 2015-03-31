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
                function (data) {
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
                function (data) {
                    $scope.settingRelationView = null;
                    $uiBlockerService.unblockUI();
                });
        };

        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);
            $scope.initController();
            $scope.registerHotKey();
        });
    } ]);