(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    template */

    angular.module('changepicstate-application')
    .controller('changePicStateController', ['$scope', '$uiBlockerService', 'changePicStateService',
        function ($scope, $uiBlockerService, changePicStateService) {
        $scope.messageResultOptions = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
        };
        $scope.changeState = function () {
            $uiBlockerService.blockUI();
            changePicStateService.changePicState({ Id: item.Id, IsActive: item.IsActive })
                .then(function (data) {
                    $scope.showResult(data.Result);
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

            $scope.showResult = function (result) {
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
        $scope.cancelChangeState = function () {
            parent.ClosePopup();
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();