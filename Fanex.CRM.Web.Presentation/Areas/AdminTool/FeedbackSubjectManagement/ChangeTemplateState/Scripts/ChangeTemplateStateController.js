(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    template */

    angular.module('addmintool-feedbacksubjectmanagement-changeTemplateState')
    .controller('changeTemplateStateController', ['$scope', '$uiBlockerService', 'changeTemplateStateService',
    function ($scope, $uiBlockerService, changeTemplateStateService) {
        $scope.messageResultOptions = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
        };
        $scope.changeTemplateState = function () {
            $uiBlockerService.blockUI();
            changeTemplateStateService.changeTemplateState({ TemplateId: template.TemplateId, IsActive: template.IsActive })
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
        $scope.cancelChangeTemplateState = function () {
            parent.ClosePopup();
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();