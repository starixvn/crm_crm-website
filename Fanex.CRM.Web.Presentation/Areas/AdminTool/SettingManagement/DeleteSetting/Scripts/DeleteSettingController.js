(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    settingInfo */
    /* jshint unused: vars,
    camelcase: false */

    angular.module('addmintool-settingmanagement-deletesetting').controller('deleteSettingController',
    ['$scope', '$uiBlockerService', 'deleteSettingService',
    function ($scope, $uiBlockerService, deleteSettingService) {
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
                function () {
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
})();