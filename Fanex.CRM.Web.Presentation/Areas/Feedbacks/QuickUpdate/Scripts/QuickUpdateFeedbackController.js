(function () {
    "use strict";
    /* global sharedScriptManager,
    crmConstants,
    viewResources,
    hotKeyManager,
    hasReviewPermission*/
    angular.module('quick-update-feedback-application').controller('quickUpdateFeedbackController', ['$scope', '$quickUpdateFeedbackService',
        function ($scope, $quickUpdateFeedbackService) {
            $scope.isShowResultMessage = false;
            $scope.feedback = {
                feedbackId: feedbackId
            };

            $scope.initController = function () {
                $scope.getFeedback();
            };

            $scope.getFeedback = function () {
                $scope.BlockUI();
                $quickUpdateFeedbackService.getFeedback($scope.feedback.feedbackId)
                    .then(function (data) {
                        $scope.unblockUI();
                        $scope.feedback = data.Result;
                        $scope.feedback.NoInquiries = $scope.feedback.NoInquiries === 0 ? null : $scope.feedback.NoInquiries;
                    }, function () {
                        $scope.unblockUI();
                    });
            };

            $scope.updateFeedback = function () {
                $scope.BlockUI();
                $quickUpdateFeedbackService.updateFeedback($scope.feedback)
                    .then(function (data) {
                        $scope.showResult(data.Result);
                        $scope.unblockUI();
                    }, function () {
                        $scope.unblockUI();
                    });
            };

            $scope.showResult = function (result) {
                $scope.resultMessage = result.Message;
                $scope.isShowResultMessage = true;
                $scope.unblockUI();

                if (result.Code !== 0) {
                    $scope.isSuccess = false;
                    return;
                }
                else {
                    $scope.isSuccess = true;
                    $scope.resultMessage = viewResources.UpdateFeedbackSuccessfully;
                }

                parent.homeManager.CachedData.shareInfoBetweenPopup = result;
                parent.homeManager.CachedData.shouldReloadView = true;

                setTimeout(function () {
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
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

            angular.element(document).ready(function () {
                sharedScriptManager.safeApply($scope);
                $scope.initController();
            });
        }]);
})();