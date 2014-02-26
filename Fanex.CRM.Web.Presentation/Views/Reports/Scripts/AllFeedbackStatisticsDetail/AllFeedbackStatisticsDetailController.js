"use strict";
/* global allFeedbackStatisticsDetailApplication,
gup,
sharedScriptManager,
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

        $scope.requestParams.param.CurrentPage = pageIndex;
        $scope.requestParams.param.FromCache = true;
        $scope.requestParams.param.ItemPerPage = itemsPerPage;

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

    $scope.exportDataToExcelFile = function () {
        $allFeedbackStatisticsDetailService.checkingExportStatus()
            .then(
                function (data) {
                    if (data.Result.ErrCode === 0) {
                        var url = 'FeedbackStatisticsDetailView?export=true&type=' + $scope.requestParams.param.Type +
                            '&fdate=' + $scope.requestParams.param.FromDate +
                            '&tdate=' + $scope.requestParams.param.ToDate +
                            '&title=' + $('#reporttile').html();

                        $('#frm').attr('src', url);
                    }
                },
                function () {
                }
            );

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