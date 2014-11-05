'use strict';
/* global fbStatisticDetailApplication,
sharedScriptManager,
criteria */
/* jshint camelcase: false */
/* jshint unused: vars */
fbStatisticDetailApplication.controller('feedbackStatisticDetailController', ['$scope', 'FeedbackStatisticDetailService', '$uiBlockerService',
function ($scope, feedbackStatisticDetailService, $uiBlockerService) {
    criteria.PageIndex = 0;
    criteria.PageSize = sharedScriptManager.cachedData.defaultItemsPerPage;
    $scope.searchCriteria = criteria;

    $scope.feedbackStatisticDetail = null;
    $scope.downloadFileOptions = null;

    $scope.initController = function () {
        $scope.getFeedbackStatisticDetail();
    };

    $scope.getFeedbackStatisticDetail = function (page, size) {
        $uiBlockerService.blockUI();
        $scope.searchCriteria.PageIndex = page || 0;
        $scope.searchCriteria.PageSize = size || sharedScriptManager.cachedData.defaultItemsPerPage;
        $scope.searchCriteria.FromCache = $scope.searchCriteria.PageIndex !== 0;

        feedbackStatisticDetailService.getFeedbackStatisticDetail($scope.searchCriteria)
            .then(function (data) {
                $scope.feedbackStatisticDetail = data.Result;
                $uiBlockerService.unblockUI();
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
    };

    $scope.exportFeedbackStatisticDetail = function () {
        $scope.downloadFileOptions = {
            URL: window.rootUrl + 'FeedbackStatisticReport/ExportFeedbackStatisticDetail',
            Params: $scope.searchCriteria
        };

        $scope.$apply();
    };
    
    angular.element(document).ready(function () {
        $scope.initController();
    });
} ]);