(function () {
    "use strict";
    /* global latestFeedbackResources,
    custId,
    username,
    tops */
    angular.module('feedback-latest-application').controller('latestFeedbackController', ['$scope', 'latestFeedbackService', '$uiBlockerService', function (
        $scope, latestFeedbackService, $uiBlockerService) {
        $scope.custId = custId;
        $scope.username = username;

        $scope.tops = tops;
        $scope.top = $scope.tops[0];
        $scope.report;

        $scope.getLatestFeedback = function () {
            $uiBlockerService.blockUI();
            var criteria = { top: $scope.top.Id };
            if ($scope.custId !== 0) {
                criteria.custId = $scope.custId;
                criteria.username = $scope.username;
            }

            latestFeedbackService.getLatestFeedback(criteria)
                .then(function (data) {
                    $scope.report = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.openEditFeedbackPopup = function (id) {
            var url = window.rootUrl + 'UpdateCustomerFeedback/Index?id=' + id;
            var maxWidthOfPopUp = 987;
            var maxHeightOfPopUp = $(parent.document).height();
            parent.OpenPopup(url, latestFeedbackResources.UpdateFeedbackTitle, maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, null, function () {
                $scope.getLatestFeedback();
            });
        };

        $scope.openQuickUpdateFeedbackPopup = function (id) {
            var url = window.rootUrl + 'QuickUpdateFeedback/Index?feedbackId=' + id;
            var maxWidthOfPopUp = 400;
            var maxHeightOfPopUp = 150;

            parent.OpenPopup(
                url,
                latestFeedbackResources.QuickUpdateTitle,
                maxWidthOfPopUp,
                maxHeightOfPopUp,
                true,
                false,
                parent.window,
                2000,
                null,
                function () {
                    $scope.getLatestFeedback();
                });
        };

        $scope.exportFeedbackExcel = function () {
            var criteria = { top: $scope.top.Id };
            if ($scope.custId !== 0) {
                criteria.custId = $scope.custId;
                criteria.username = $scope.username;
            }

            $scope.downloadFileOptions = {
                URL: window.rootUrl + "Feedbacks/ExportLatestFeedbacks",
                Params: criteria
            };

            $scope.$apply();
        };

        $scope.showRelatedCustomer = function (feedbackId) {
            var index = $scope.report.Feedbacks.findIndex(feedback => feedback.FeedbackId === feedbackId);
            var popupElement = parent.$('#div-resultpopup');
            popupElement.removeClass('display-none').html($scope.report.Feedbacks[index].RelatedCustomer.split(',').join(', '));
            popupElement.dialog({
                width: 900,
                minHeight: 300,
                maxHeight: 500,
                zIndex: 500,
                position: { my: 'center', at: 'center' },
                resizable: true,
                modal: true,
                title: latestFeedbackResources.RelatedCustomer,
                close: function (event, ui) {
                    popupElement.dialog('destroy');
                    popupElement.addClass('display-none').html('');
                },
                buttons: [{
                    text: latestFeedbackResources.Close,
                    click: function () {
                        popupElement.dialog('close');
                    }
                }]
            });
        };

        angular.element(document).ready(function () {
            $scope.getLatestFeedback();
        });
    }]);
})();