"use strict";
/* global insertLogApplication,
sharedScriptManager,
hotKeyManager,
currentDate,
customerId,
viewResources,
moment,
crmConstants */
/* jshint camelcase: false */
insertLogApplication.controller('insertLogController', ['$scope', '$http', '$insertLogService',
function (
    $scope,
    $http,
    $insertLogService) {
    $scope.redemptionLog = {
        ClaimedTime: new Date(currentDate),
        Currency: {
            Id: 0,
            Name: ''
        },
        DoneBy: '',
        Amount: 0,
        Subject: '',
        Status: {
            Id: 0,
            Name: ''
        },
        Remark: ''
    };
    $scope.oldRedemptionLog = angular.copy($scope.redemptionLog);
    $scope.currencies = [];
    $scope.statusList = null;
    $scope.customerId = customerId;
    $scope.isSetFocus = false;
    $scope.isSuccess = false;
    $scope.isShowResultMessage = false;
    $scope.resultMessage = '';
    $scope.amountString = $scope.redemptionLog.Amount;

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

        hotKeyManager.CachedData.isBubbleToParent = false;
    };
    $scope.getAllStatus = function () {
        $scope.BlockUI();
        $insertLogService.getAllStatus()
            .then(function (data) {
                $scope.currencies = data.Result.Currencies;
                $scope.redemptionLog.Currency = $scope.currencies[0];
                $scope.statusList = data.Result.StatusList;
                $scope.redemptionLog.Status.Id = $scope.statusList[0].Id;
                $scope.oldRedemptionLog.Status.Id = $scope.statusList[0].Id;
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
    $scope.updateSelectedStatus = function (statusId) {
        statusId = parseInt(statusId);
        var statusLength = $scope.statusList.length;
        var selectedStatus = null;
        for (var i = 0; i < statusLength; i++) {
            var status = $scope.statusList[i];
            if (status.Id === statusId) {
                selectedStatus = status;
                break;
            }
        }

        $scope.redemptionLog.Status = selectedStatus;
        $scope.updateChangeFlag();
    };
    $scope.updateSelectedSubject = function (subject) {
        $scope.redemptionLog.Subject = subject;
    };
    $scope.insertLog = function () {
        $scope.BlockUI();
        hotKeyManager.documentBlur();
        var insertLogCriteria = {
            ClaimedTime: $scope.redemptionLog.ClaimedTime ?
                    moment($scope.redemptionLog.ClaimedTime).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment) :
                    '',
            CurrencyId: $scope.redemptionLog.Currency.Id,
            DoneBy: $scope.redemptionLog.DoneBy,
            Amount: $scope.getAmount(),
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
        if (insertLogCriteria.DoneBy.length === 0) {
            $scope.isShowResultMessage = true;
            $scope.isSuccess = false;
            /* jshint camelcase: false */
            $scope.resultMessage = viewResources.Redemption_Log_EmptyDoneBy;
            result = false;
        }

        if (result && (insertLogCriteria.Subject === '' ||
            insertLogCriteria.Subject === '<br>')) {
            $scope.isShowResultMessage = true;
            $scope.isSuccess = false;
            /* jshint camelcase: false */
            $scope.resultMessage = viewResources.Redemption_Log_EmptySubject;
            result = false;
        }

        if (result &&
            (insertLogCriteria.Remark === '' ||
            insertLogCriteria.Remark === '<br>')) {
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
        parent.homeManager.CachedData.shouldReloadView = true;
        $scope.unblockUI();
        parent.ClosePopup();
    };
    $scope.ignoreChanges = function () {
        $scope.oldRedemptionLog = angular.copy($scope.redemptionLog);
        $scope.$apply();
        parent.ClosePopup();
    };
    $scope.getAmount = function () {
        return parseFloat($scope.amountString.replace(/,/g, ''));
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

        hotKeyManager.CachedData.isBlockingUI = false;
    };
    $scope.BlockUI = function () {
        hotKeyManager.CachedData.isBlockingUI = true;
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };
    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };

    $scope.setFocus = function () {
        $scope.$apply(function () {
            $scope.isSetFocus = true;
        });
    };
    $scope.updateChangeFlag = function () {
        if ($scope.redemptionLog.Subject !== $scope.oldRedemptionLog.Subject ||
                $scope.redemptionLog.Status.Id !== $scope.oldRedemptionLog.Status.Id ||
                $scope.redemptionLog.Remark !== $scope.oldRedemptionLog.Remark ||
                $scope.redemptionLog.Currency.Id !== $scope.oldRedemptionLog.Currency.Id ||
                $scope.redemptionLog.DoneBy !== $scope.oldRedemptionLog.DoneBy ||
                $scope.redemptionLog.Amount !== $scope.getAmount()) {
            parent.homeManager.CachedData.isChangingDataPopUp = true;
            parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_InsertRedemption_Changes;
        } else {
            parent.homeManager.CachedData.isChangingDataPopUp = false;
        }
    };
    angular.element(document).ready(function () {
        $scope.$watch(function ($scope) {
            $scope.updateChangeFlag();
        });

        parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
        $scope.initController();
        $scope.setFocus();
    });
} ]);