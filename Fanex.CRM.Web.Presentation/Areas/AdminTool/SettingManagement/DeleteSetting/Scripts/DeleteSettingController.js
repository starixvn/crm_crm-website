"use strict";
/* global deleteSettingApplication,
sharedScriptManager,
MessageResult,
settingInfo */
/* jshint unused: vars,
camelcase: false */

deleteSettingApplication.controller('deleteSettingController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'deleteSettingService',
    function ($scope, $uiBlockerService, $resultMessageService, deleteSettingService) {
        $scope.deleteParams = {
            OldSetting: settingInfo
        };
        $scope.messageResultOptions = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
        };
        $scope.deleteSetting = function () {
            $uiBlockerService.blockUI();
            deleteSettingService.deleteSetting($scope.deleteParams)
                .then(function (data) {
                    $scope.showDeleteResult(data.Result);
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.showDeleteResult = function (result) {
            if (result.Code !== 0) {
                $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                return;
            }

            $scope.messageResultOptions = new MessageResult(result.Message, true, true);
            parent.homeManager.CachedData.shouldReloadView = true;
            setTimeout(function () {
                $scope.messageResultOptions = new MessageResult('', false, false);
                parent.ClosePopup();
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };
        $scope.cancelDeleteSetting = function () {
            parent.ClosePopup();
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);