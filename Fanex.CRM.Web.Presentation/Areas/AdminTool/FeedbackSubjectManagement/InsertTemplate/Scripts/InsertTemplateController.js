(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    viewResources,
    hotKeyManager */
    /* jshint unused: vars,
    camelcase: false */

    angular.module('addmintool-feedbacksubjectmanagement-inserttemplate').controller('insertTemplateController',
    ['$scope', '$uiBlockerService', 'insertTemplateService',
    function ($scope, $uiBlockerService, insertTemplateService) {
        $scope.messageResultOptions = null;
        $scope.feedbackSubjectTemplate = {
            TemplateContent: ""
        };
        $scope.oldFeedbackSubjectTemplate = angular.copy($scope.feedbackSubjectTemplate);
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
        };
        $scope.insertTemplate = function () {
            $uiBlockerService.blockUI();
            if (!$scope.checkTemplateParam($scope.feedbackSubjectTemplate)) {
                $uiBlockerService.unblockUI();
                return;
            }

            insertTemplateService.insertTemplate($scope.feedbackSubjectTemplate)
                .then(function (data) {
                    $scope.showInsertResult(data.Result);
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.checkTemplateParam = function (template) {
            if (template.TemplateContent === "") {
                $scope.messageResultOptions = new MessageResult(viewResources.FeedbackSubjectManagement_Error_EmptyTemplate, true, false);
                return false;
            }

            if (template.TemplateContent.length > 200) {
                $scope.messageResultOptions = new MessageResult(viewResources.FeedbackSubjectManagement_Error_ExceedTemplate, true, false);
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
            $scope.oldFeedbackSubjectTemplate = angular.copy($scope.feedbackSubjectTemplate);
            setTimeout(function () {
                $scope.messageResultOptions = new MessageResult('', false, false);
                parent.ClosePopup();
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };
        $scope.cancelInsertTemplate = function () {
            parent.ClosePopup();
        };
        $scope.updateChangeFlag = function () {
            if ($scope.feedbackSubjectTemplate.TemplateContent !== $scope.oldFeedbackSubjectTemplate.TemplateContent) {
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_InsertTemplate_Changes;
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                return;
            }

            parent.homeManager.CachedData.isChangingDataPopUp = false;
        };

        $scope.ignoreChanges = function () {
            $scope.oldFeedbackSubjectTemplate = angular.copy($scope.feedbackSubjectTemplate);
            $scope.$apply();
            $scope.updateChangeFlag();
            parent.ClosePopup();
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.insertTemplate();
            $scope.safeApply();
        };
        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
        angular.element(document).ready(function () {
            $scope.initController();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "AdminTool_FeedbackSubjectManagement_InsertTemplate"
            });

            $scope.setFocus();
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });
        });
    } ]);
})();