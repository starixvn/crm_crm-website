(function () {
    "use strict";
    /* global sharedScriptManager,
    crmConstants,
    viewResources,
    hotKeyManager,
    hasReviewPermission*/
    angular.module('edit-multiple-log-statuses-application').controller('editMultipleLogStatusesController', ['$scope', '$editMultipleLogStatusesService',
        function ($scope, $editMultipleLogsService) {
            $scope.multipleLogs = multipleLogs;
            $scope.oldmultipleLogs = angular.copy(multipleLogs);
            $scope.statusList = null;
            $scope.isShowRemarkBox = true;
            $scope.hasReviewPermission = hasReviewPermission;
            $scope.feedbackList = '';

            $scope.requiredManager = {
                subject: true,
                remark: true,
                solution: true,
                review: true
            };

            $scope.initController = function () {
                $scope.getSettings();
            };

            $scope.initSelectedLogInfo = function () {
                $scope.feedbackList = $scope.getFeedbackIdList(parent.homeManager.CachedData.feedbacks);
                parent.homeManager.CachedData.feedbacks = null;
            };

            $scope.getFeedbackIdList = function (feedbacks) {
                var feedbackIds = [];
                angular.forEach(feedbacks, function (feedback) {
                    this.push(feedback.FeedbackId);
                }, feedbackIds);

                return feedbackIds;
            };

            /* jshint camelcase: false */
            $scope.invokeHotKey_CtrlS = function () {
                $scope.editMultipleLogs();
                $scope.$apply();
            };
            /* jshint camelcase: false */

            $scope.updateReview = function (review) {
                $scope.multipleLogs.Review = review;
            };

            $scope.editMultipleLogs = function () {
                $scope.isShowResultMessage = false;
                $scope.isSuccess = true;
                var isResetReview = $scope.oldmultipleLogs.IsReviewed && !$scope.multipleLogs.IsReviewed;
                var isReviewNotChanged = !$scope.oldmultipleLogs.IsReviewed && !$scope.multipleLogs.IsReviewed;
                var updateReviewedMode = (!$scope.hasReviewPermission || isReviewNotChanged) ? crmConstants.Enums.UpdateReviewedMode.NotChange :
                    (isResetReview ? crmConstants.Enums.UpdateReviewedMode.Reset : crmConstants.Enums.UpdateReviewedMode.Change);

                var editCriteria = {
                    ListFeedbackId: $scope.feedbackList.join(','),
                    StatusId: $scope.multipleLogs.Status.Id,
                    IsReviewed: $scope.multipleLogs.IsReviewed,
                    Review: $scope.multipleLogs.Review,
                    UpdateReviewedMode: updateReviewedMode
                };
                if ($scope.validateEditMultipleLogsCriteria(editCriteria)) {
                    $scope.BlockUI();
                    $editMultipleLogsService.editMultipleLogStatuses(editCriteria)
                        .then(function (data) {
                            $scope.showResult(data.Result);
                            $scope.unblockUI();
                        }, function () {
                            $scope.unblockUI();
                        });
                }
            };

            $scope.showResult = function (result) {
                $scope.resultMessage = result.Message;
                $scope.isShowResultMessage = true;
                $scope.unblockUI();

                if (result.Code !== 0) {
                    $scope.isSuccess = false;
                    return;
                }

                parent.homeManager.CachedData.shareInfoBetweenPopup = result;
                parent.homeManager.CachedData.shouldReloadView = true;

                setTimeout(function () {
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            };

            $scope.updateChangeFlag = function () {
                if ($scope.isParamsChange()) {
                    parent.homeManager.CachedData.isChangingDataPopUp = true;
                    parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_MultipleLogs_Changes;
                } else {
                    parent.homeManager.CachedData.isChangingDataPopUp = false;
                }

                $scope.safeApply();
            };

            $scope.isParamsChange = function () {
                return $scope.multipleLogs.Status.Id !== $scope.oldmultipleLogs.Status.Id;
            };

            $scope.validateEditMultipleLogsCriteria = function (criteria) {
                var result = true;

                $scope.requiredManager.review = !(criteria.IsReviewed && ($.trim(criteria.Review) === '' || criteria.Review === '<br>'));
                if (result && !$scope.requiredManager.review) {
                    $scope.resultMessage = viewResources.Multiple_Logs_EmptyReview;
                    result = false;
                }

                if (result) {
                    $scope.isShowResultMessage = false;
                } else {
                    $scope.isShowResultMessage = true;
                    $scope.isSuccess = false;
                }

                return result;
            };

            $scope.unblockUI = function () {
                try {
                    parent.$.unblockUI();
                } catch (e) {
                }

                hotKeyManager.CachedData.isBlockingUI = false;
            };

            $scope.BlockUI = function () {
                hotKeyManager.CachedData.isBlockingUI = true;
                try {
                    parent.BlockUI();
                } catch (e) {
                }
            };

            $scope.getSettings = function () {
                $scope.BlockUI();
                $scope.isLoadedAllSite = false;
                $editMultipleLogsService.getSettings()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.allSettings = data.Result;

                        $scope.statusList = allSetting.StatusList;

                        $scope.updateAllSettingsForLog();
                        $scope.completedLoadFilterInfo();
                    }, function () {
                        $scope.completedLoadFilterInfo();
                    });
            };

            $scope.completedLoadFilterInfo = function () {
                $scope.initSelectedLogInfo();
                $scope.unblockUI();
            };

            $scope.updateAllSettingsForLog = function () {
                SettingManager.updateSettingSelectList($scope.multipleLogs.Status, $scope.statusList);
            };

            $scope.getReviewedInfo = function () {
                return sharedScriptManager.stringFormat(
                    viewResources.ReviewedInformation,
                    [$scope.multipleLogs.Reviewer, $scope.multipleLogs.ReviewedTimeString]);
            };

            angular.element(document).ready(function () {
                sharedScriptManager.safeApply($scope);
                $scope.$watch(function ($scope) {
                    $scope.updateChangeFlag();
                });

                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;

                $scope.initController();
                hotKeyManager.CachedData.ActiveDocumentManager.push({
                    Value: $scope,
                    Id: "MultipleLogs_Edit"
                });

                hotKeyManager.CachedData.isBubbleToParent = false;
            });
        }]);
})();