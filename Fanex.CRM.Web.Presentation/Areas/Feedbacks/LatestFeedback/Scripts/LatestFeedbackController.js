'use strict';
/* global latestFBApplication,
latestFeedbackResources,
custId,
username,
tops */
/* jshint camelcase: false */
/* jshint unused: vars */
latestFBApplication.controller('latestFeedbackController', ['$scope', 'latestFeedbackService', '$uiBlockerService', function (
    $scope, latestFeedbackService, $uiBlockerService) {
    $scope.custId = custId;
    $scope.username = username;

    $scope.tops = tops;
    $scope.top = $scope.tops[0];
    $scope.report;

    $scope.getLatestFeedback = function () {
        $uiBlockerService.blockUI();
        var criteria = { top: $scope.top.Id };
        if ($scope.custId > 0) {
            criteria.custId = $scope.custId;
            criteria.username = $scope.username; 
        }

        latestFeedbackService.getLatestFeedback(criteria)
            .then(function (data) {
                $scope.report = data.Result;
                $uiBlockerService.unblockUI();
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
    };

    $scope.openEditFeedbackPopup = function (id) {
        var url = window.rootUrl + 'Feedback/EditFeedback?id=' + id + '&rep=2';
        var maxWidthOfPopUp = 987;
        var maxHeightOfPopUp = $(parent.document).height();
        parent.OpenPopup(url, latestFeedbackResources.UpdateFeedbackTitle, maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, function () {
            $scope.searchManageFeedback();
        });
    };

    angular.element(document).ready(function () {
        $scope.getLatestFeedback();
    });
} ]);