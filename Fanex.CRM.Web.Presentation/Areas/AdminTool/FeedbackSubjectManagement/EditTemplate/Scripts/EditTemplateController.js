(function () {
    "use strict";
    /* global sharedScriptManager,
    MessageResult,
    templateId,
    viewResources,
    hotKeyManager */
    /* jshint unused: vars,
    camelcase: false */

    angular.module('addmintool-feedbacksubjectmanagement-edittemplate').controller('editTemplateController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'editTemplateService',
    function ($scope, $uiBlockerService, $resultMessageService, editTemplateService) {
        $scope.messageResultOptions = null;
        $scope.isSetFocus = false;
        $scope.feedbackSubjectTemplate = {
            Id: 0,
            TemplateContent: ""
        };
        $scope.oldFeedbackSubjectTemplate = angular.copy($scope.feedbackSubjectTemplate);
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.loadTemplate();
        };
        $scope.editTemplate = function () {
            $uiBlockerService.blockUI();
            if (!$scope.checkTemplateParam($scope.feedbackSubjectTemplate)) {
                $uiBlockerService.unblockUI();
                return;
            }

            editTemplateService.editTemplate($scope.feedbackSubjectTemplate)
                .then(function (data) {
                    $scope.showEditResult(data.Result);
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.loadTemplate = function () {
            $uiBlockerService.blockUI();
            editTemplateService.loadTemplate({ TemplateId: templateId })
                .then(function (data) {
                    $scope.feedbackSubjectTemplate = data.Result;
                    $scope.oldFeedbackSubjectTemplate = angular.copy($scope.feedbackSubjectTemplate);
                    $scope.updateChangeFlag();
                    $uiBlockerService.unblockUI();
                },
                function (data) {
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

        $scope.showEditResult = function (result) {
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
        $scope.cancelEditTemplate = function () {
            parent.ClosePopup();
        };
        $scope.updateChangeFlag = function () {
            if ($scope.feedbackSubjectTemplate.TemplateContent !== $scope.oldFeedbackSubjectTemplate.TemplateContent) {
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_EditTemplate_Changes;
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
            $scope.editTemplate();
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
                Id: "AdminTool_FeedbackSubjectManagement_EditTemplate"
            });

            $scope.setFocus();
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });
        });
    } ]);
})();