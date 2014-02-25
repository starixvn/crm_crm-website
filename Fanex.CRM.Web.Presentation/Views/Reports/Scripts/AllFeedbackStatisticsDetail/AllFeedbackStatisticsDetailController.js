"use strict";
/* global allFeedbackStatisticsDetailApplication,
paramsRequest,
gup,
decodeURIComponent*/
allFeedbackStatisticsDetailApplication.controller('allFeedbackStatisticsDetailController', ['$scope', '$http', '$allFeedbackStatisticsDetailService',
function (
    $scope,
    $http,
    $allFeedbackStatisticsDetailService) {
    $scope.feedbacks = null;
    $scope.parentPage;
    $scope.requestParams = {};
    $scope.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    };

    $scope.BlockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };

    $scope.editFeedback = function (index) {
        var url = '../Feedback/EditFeedback?id=' + $scope.feedbacks.rows[index].ID;
        $scope.parentPage.OpenPopup(url,
            "Update Feedback",
            $($scope.parentPage.document).width() - 300,
            $($scope.parentPage.document).height() - 40,
            true,
            false,
            window,
            2000,
            function () {
                $scope.returnFeedbackStatisticsDetailPage();
            },
            function () {
                $scope.returnFeedbackStatisticsDetailPage();
            }
        );
    };

    $scope.returnFeedbackStatisticsDetailPage = function () {
        var inputParams =
                    "type=" + $scope.requestParams.Type +
                    "&fromDate=" + $scope.requestParams.FromDate +
                    "&toDate=" + $scope.requestParams.ToDate +
                    "&fDay=" + $scope.requestParams.FromDuration.Day +
                    "&fHour=" + $scope.requestParams.FromDuration.Hour +
                    "&fMinute=" + $scope.requestParams.FromDuration.Minute +
                    "&fSecond=" + $scope.requestParams.FromDuration.Second +
                    "&tDay=" + $scope.requestParams.ToDuration.Day +
                    "&tHour=" + $scope.requestParams.ToDuration.Hour +
                    "&tMinute=" + $scope.requestParams.ToDuration.Minute +
                    "&tSecond=" + $scope.requestParams.ToDuration.Second;

        var url = "../Reports/FeedbackStatisticsDetailView?";

        $scope.parentPage.OpenPopup(
            url,
            "Feedback Statistic Detail Report",
            $($scope.parentPage.document).width() - 300,
            $($scope.parentPage.document).height() - 40,
            true,
            false,
            window,
            2000,
            null,
            null);
    };

    $scope.getAllFeedbackStatisticsDetail = function () {
        $scope.BlockUI();

        $allFeedbackStatisticsDetailService.getAllFeedbackStatisticsDetail($scope.requestParams)
                .then(function (data) {
                    $scope.feedbacks = data.Result;
                    $scope.unblockUI();
                },
                function (data) {
                    /* jshint unused: vars */
                    $scope.unblockUI();
                });
    };

    $scope.pagingFeedbackStatisticsDetail = function (pageIndex, itemsPerPage) {
        $scope.BlockUI();

        $scope.requestParams.CurrentPage = pageIndex;
        $scope.requestParams.FromCache = true;
        $scope.requestParams.ItemPerPage = itemsPerPage;

        $allFeedbackStatisticsDetailService.getAllFeedbackStatisticsDetail($scope.requestParams)
                .then(function (data) {
                    $scope.feedbacks = data.Result;
                    $scope.unblockUI();
                },
                function (data) {
                    /* jshint unused: vars */
                    $scope.unblockUI();
                });
    };

    angular.element(document).ready(function () {
        $scope.parentPage = parent;
        $scope.requestParams = {
            'param': {
                'Type': parseInt(gup("type")),
                'ValueId': 0,
                'MainId': 0,
                'SubId': 0,
                'FromDate': decodeURIComponent(gup("fromDate").replace(/\+/g, " ")),
                'ToDate': decodeURIComponent(gup("toDate").replace(/\+/g, " ")),
                'FromCache': false,
                'ItemPerPage': sharedScriptManager.cachedData.defaultItemsPerPage,
                'CurrentPage': 0,
                'FromDuration': {
                    "Day": parseInt(gup("fDay")),
                    "Hour": parseInt(gup("fHour")),
                    "Minute": parseInt(gup("fMinute")),
                    "Second": parseInt(gup("fSecond"))
                },
                'ToDuration': {
                    "Day": parseInt(gup("tDay")),
                    "Hour": parseInt(gup("tHour")),
                    "Minute": parseInt(gup("tMinute")),
                    "Second": parseInt(gup("tSecond"))
                }
            }
        };

        $scope.getAllFeedbackStatisticsDetail();
    });

} ]);