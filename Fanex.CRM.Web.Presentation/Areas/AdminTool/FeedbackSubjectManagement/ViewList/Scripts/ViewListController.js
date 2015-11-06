(function () {
    "use strict";
    /* global registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions,
    sharedScriptManager,
    viewResources */
    /* jshint unused: vars,
    camelcase: false */
    angular.module('addmintool-feedbacksubjectmanagement-application').controller('feedbackSubjectManagementController',
    ['$scope', '$uiBlockerService', 'feedbackSubjectManagementService',
    function ($scope, $uiBlockerService, feedbackSubjectManagementService) {
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.feedbackSubjectTemplateView = null;
        $scope.isSetFocus = false;
        $scope.gridDirectiveOptions = {
            IsUpdateGrid: false
        };

        $scope.searchTemplateParams = {
            Subject: null,
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getNumberOfFeedbackSubjectTemplate();
        };
        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            $uiBlockerService.blockUI();
            feedbackSubjectManagementService.getNumberOfFeedbackSubjectTemplate()
            .then(function (data) {
                var numberOfFeedbackItem = data.Result;
                $scope.feedbackSubjectTemplateList = [];
                var totalPage = Math.round((numberOfFeedbackItem / sharedScriptManager.cachedData.maxFeedbackSubjectTemplateNumberOnLoad) + 0.5);
                $scope.feedbackSubjectTemplateTotalPage = totalPage;
                $scope.isLoadedFeedbackSubjectTemplate = false;
                for (var i = 1; i <= totalPage; i++) {
                    $scope.getFeedbackSubjectTemplateByPagingIndex(i);
                }
            },
            function () {
                $scope.isLoadedFeedbackSubjectTemplate = true;
            });
        };
        $scope.getFeedbackSubjectTemplateByPagingIndex = function (pagingIndex) {
            $uiBlockerService.blockUI();
            feedbackSubjectManagementService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
            .then(function (data) {
                var feedbackSubjectTemplate = data.Result;
                if (feedbackSubjectTemplate != null && feedbackSubjectTemplate.length > 0) {
                    $scope.feedbackSubjectTemplateList = $scope.feedbackSubjectTemplateList.concat(feedbackSubjectTemplate);
                }

                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            },
            function () {
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            });
        };
        $scope.completedLoadFilterInfo = function () {
            if (!$scope.isLoadedFeedbackSubjectTemplate) {
                return;
            }

            $scope.subjectOptions = {
                NumberOfSuggestItems: 15,
                SubjectList: $scope.feedbackSubjectTemplateList,
                Subject: '',
                IsUpdate: true,
                OnChangeEvent: $scope.updateSelectedSubject
            };

            $scope.searchTemplate();
        };
        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.searchTemplate = function () {
            $uiBlockerService.blockUI();

            $scope.lastSelectedParams = angular.copy($scope.searchTemplateParams);
            var param = angular.copy($scope.searchTemplateParams);
            param.PageIndex += 1;
            feedbackSubjectManagementService.searchTemplate(param)
                .then(function (data) {
                    $scope.feedbackSubjectTemplateView = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $scope.feedbackSubjectTemplateView = null;
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.pagingFeedbackSubjectTemplateView = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();
            $scope.lastSelectedParams.PageIndex = parseInt(pageIndex);
            $scope.lastSelectedParams.PageSize = pageSize;
            $scope.lastSelectedParams.IsGetFromCached = true;

            var param = angular.copy($scope.lastSelectedParams);
            param.PageIndex += 1;
            feedbackSubjectManagementService.searchTemplate(param)
                .then(function (data) {
                    $scope.feedbackSubjectTemplateView = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.editTemplate = function (template) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "FeedbackSubjectManagement/OpenEditTemplateView?" + $.param({ templateId: template.Id }),
                viewResources.Edit_Feedback_Subject_Template,
                680,
                200,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditView,
                { my: "center top+200", at: "center top" },
                $scope.beforeCloseEditView);
        };
        $scope.deleteTemplate = function (template) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "FeedbackSubjectManagement/OpenDeleteTemplateView?" + $.param({ templateId: template.Id }),
                viewResources.Delete_Feedback_Subject_Template,
                450,
                160,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeDeleteView,
                { my: "center top+200", at: "center top" },
                null);
        };
        $scope.closeDeleteView = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.reloadTemplate();
        };
        $scope.beforeCloseEditView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges, popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.closeEditView = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.$apply();
            $scope.reloadTemplate();
        };
        $scope.reloadTemplate = function () {
            $uiBlockerService.blockUI();
            var param = angular.copy($scope.lastSelectedParams);
            param.PageIndex += 1;
            feedbackSubjectManagementService.searchTemplate(param)
                .then(function (data) {
                    $scope.feedbackSubjectTemplateView = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.showInsertPopup = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "FeedbackSubjectManagement/OpenInsertTemplateView",
                viewResources.Insert_Feedback_Subject_Template,
                780,
                200,
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
            $scope.reloadTemplate();
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