"use strict";
/* global insertLogApplication,
sharedScriptManager,
hotKeyManager,
currentDate,
customerId,
viewResources */
insertLogApplication.controller('insertLogController', ['$scope', '$http', '$insertLogService',
function (
    $scope,
    $http,
    $insertLogService) {
    $scope.redemptionLog = {
        ClaimedTime: new Date(currentDate),
        Subject: '',
        Status: null,
        Remark: ''
    };
    $scope.oldRedemptionLog = {
        ClaimedTime: new Date(currentDate),
        Subject: '',
        Status: null,
        Remark: ''
    };
    $scope.statusList = null;
    $scope.customerId = customerId;
    $scope.isSuccess = false;
    $scope.isShowResultMessage = false;
    $scope.resultMessage = '';

    $scope.loadedFilter = [];
    $scope.feedbackSubjectTemplateList = [];
    $scope.feedbackSubjectTemplateTotalPage = 0;
    $scope.isLoadedFeedbackSubjectTemplate = false;

    $scope.initController = function () {
        $scope.getAllStatus();
        $scope.getNumberOfFeedbackSubjectTemplate();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs_Insert"
        });
    };
    $scope.getAllStatus = function () {
        $scope.BlockUI();
        $insertLogService.getAllStatus()
            .then(function (data) {
                $scope.statusList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Status'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Status'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getNumberOfFeedbackSubjectTemplate = function () {
        $scope.BlockUI();
        $insertLogService.getNumberOfFeedbackSubjectTemplate()
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
            function (data) {
                /* jshint unused: vars */
                $scope.isLoadedFeedbackSubjectTemplate = true;
            });
    };
    $scope.getFeedbackSubjectTemplateByPagingIndex = function (pagingIndex) {
        $scope.BlockUI();
        $insertLogService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
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
            function (data) {
                /* jshint unused: vars */
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            });
    };
    $scope.completedLoadFilterInfo = function () {
        var numberOfLoadFilter = 1;
        if ($scope.loadedFilter.length >= numberOfLoadFilter &&
            $scope.isLoadedFeedbackSubjectTemplate) {
            $scope.redemptionLog.Status = $scope.statusList[0];
            $scope.unblockUI();
        }
    };
    $scope.updateRemark = function (remark) {
        $scope.redemptionLog.Remark = remark;
    };
    $scope.updateSelectedDate = function (selectedDate) {
        $scope.redemptionLog.ClaimedTime = selectedDate;
    };
    $scope.updateSelectedStatus = function (status) {
        $scope.redemptionLog.Status = status;
    };
    $scope.updateSelectedSubject = function (subject) {
        $scope.redemptionLog.Subject = subject;
    };
    $scope.insertLog = function () {
        $scope.BlockUI();
        var insertLogCriteria = {
            ClaimedTime: $scope.redemptionLog.ClaimedTime,
            Subject: $scope.redemptionLog.Subject,
            StatusId: $scope.redemptionLog.Status.Id,
            Remark: $scope.redemptionLog.Remark,
            CustomerId: $scope.customerId
        };

        if ($scope.validateInsertLogCriteria(insertLogCriteria)) {
            $insertLogService.insertLog(insertLogCriteria)
            .then(function (data) {
                $scope.completedInsertLog(data.Result);
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
        } else {
            $scope.unblockUI();
        }
    };
    $scope.validateInsertLogCriteria = function (insertLogCriteria) {
        var result = true;
        if (insertLogCriteria.Subject === '') {
            $scope.isShowResultMessage = true;
            $scope.isSuccess = false;
            /* jshint camelcase: false */
            $scope.resultMessage = viewResources.Redemption_Log_EmptySubject;
            result = false;
        }

        if (result && insertLogCriteria.Remark === '') {
            $scope.isShowResultMessage = true;
            $scope.isSuccess = false;
            /* jshint camelcase: false */
            $scope.resultMessage = viewResources.Redemption_Log_EmptyRemark;
            result = false;
        }

        if (result) {
            $scope.isShowResultMessage = false;
        }

        $scope.$apply();
        return result;
    };
    $scope.completedInsertLog = function (insertResult) {
        $scope.oldRedemptionLog = angular.copy($scope.redemptionLog);
        parent.homeManager.CachedData.shareInfoBetweenPopup = insertResult;
        parent.homeManager.CachedData.shouldReloadLogs = true;
        $scope.unblockUI();
        parent.ClosePopup();
    };
    $scope.ignoreChanges = function () {
        $scope.oldRedemptionLog = angular.copy($scope.redemptionLog);
        $scope.$apply();
        parent.ClosePopup();
    };
    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlS = function () {
        $scope.insertLog();
    };
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
    angular.element(document).ready(function () {
        $scope.$watch(function ($scope) {
            if ($scope.redemptionLog.Subject !== $scope.oldRedemptionLog.Subject ||
                $scope.redemptionLog.Remark !== $scope.oldRedemptionLog.Remark) {
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_InsertRedemption_Changes;
            } else {
                parent.homeManager.CachedData.isChangingDataPopUp = false;
            }
        });

        parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
        $scope.initController();
    });
} ]);