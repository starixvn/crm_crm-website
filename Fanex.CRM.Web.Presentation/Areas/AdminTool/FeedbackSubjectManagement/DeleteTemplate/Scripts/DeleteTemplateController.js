(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    templateId */
    /* jshint unused: vars,
    camelcase: false */

    angular.module('addmintool-feedbacksubjectmanagement-deletetemplate')
    .controller('deleteTemplateController', ['$scope', '$uiBlockerService', 'deleteTemplateService',
    function ($scope, $uiBlockerService, deleteTemplateService) {
        $scope.messageResultOptions = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
        };
        $scope.deleteTemplate = function () {
            $uiBlockerService.blockUI();
            deleteTemplateService.deleteTemplate({ TemplateId: templateId })
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
        $scope.cancelDeleteTemplate = function () {
            parent.ClosePopup();
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();