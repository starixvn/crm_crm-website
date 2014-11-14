"use strict";
/* global dashboardApplication,
customerId,
hotKeyManager,
viewResources,
crmConstants */
dashboardApplication.controller('dashboardController', ['$scope',
    '$http',
    '$dashboardService',
    function ($scope,
        $http,
        $dashboardService) {
        $scope.customerId = customerId;

        $scope.customerInfo = null;
        $scope.customerRemarks = null;
        $scope.redemptionLogs = null;
        $scope.communicationLogs = null;

        $scope.isSetFocus = false;
        $scope.isSuccess = false;
        $scope.isShowResultMessage = false;
        $scope.resultMessage = '';

        $scope.isLoadedCustomerInfo = false;
        $scope.isLoadedCustomerRemarks = false;
        $scope.isLoadedRedemptionLogs = false;
        $scope.isLoadedCommunicationLogs = false;
        $scope.initController = function () {
            $scope.BlockUI();
            $scope.getCustomerRemarks();
            $scope.getCustomerInfo();
            $scope.getRedemptionLogs();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Dashboard_MainTab"
            });
        };

        $scope.completedLoadDashboard = function () {
            if ($scope.isLoadedRedemptionLogs &&
                !$scope.isLoadedCommunicationLogs) {
                $scope.getCommunicationLogs();
                return;
            }

            if ($scope.isLoadedCustomerInfo &&
                $scope.isLoadedCustomerRemarks &&
                $scope.isLoadedRedemptionLogs &&
                $scope.isLoadedCommunicationLogs) {
                $scope.unblockUI();
            }
        };
        $scope.getCustomerInfo = function () {
            $scope.BlockUI();
            $scope.isLoadedCustomerInfo = false;
            var selectedCriteria = {
                CustomerId: $scope.customerId
            };
            $dashboardService.getCustomerInfo(selectedCriteria)
                .then(function (data) {
                    $scope.isLoadedCustomerInfo = true;
                    $scope.customerInfo = data.Result;
                    $scope.completedLoadDashboard();
                },
                function (data) {
                    /* jshint unused: vars */
                    $scope.isLoadedCustomerInfo = true;
                    $scope.completedLoadDashboard();
                });
        };
        $scope.getCustomerRemarks = function () {
            $scope.isLoadedCustomerRemarks = false;
            var selectedCriteria = {
                CustomerId: $scope.customerId
            };
            $dashboardService.getCustomerRemarks(selectedCriteria)
                .then(function (data) {
                    $scope.isLoadedCustomerRemarks = true;
                    $scope.customerRemarks = data.Result;
                    $scope.completedLoadDashboard();
                },
                function (data) {
                    /* jshint unused: vars */
                    $scope.isLoadedCustomerRemarks = true;
                    $scope.completedLoadDashboard();
                });
        };
        $scope.getRedemptionLogs = function () {
            $scope.isLoadedRedemptionLogs = false;
            var selectedCriteria = {
                CustomerId: $scope.customerId
            };
            $dashboardService.getRedemptionLogs(selectedCriteria)
                .then(function (data) {
                    $scope.isLoadedRedemptionLogs = true;
                    $scope.redemptionLogs = data.Result;
                    $scope.completedLoadDashboard();
                },
                function (data) {
                    /* jshint unused: vars */
                    $scope.isLoadedRedemptionLogs = true;
                    $scope.completedLoadDashboard();
                });
        };
        $scope.getCommunicationLogs = function () {
            $scope.isLoadedCommunicationLogs = false;
            var selectedCriteria = {
                CustomerId: $scope.customerId
            };
            $dashboardService.getCommunicationLogs(selectedCriteria)
                .then(function (data) {
                    $scope.isLoadedCommunicationLogs = true;
                    $scope.communicationLogs = data.Result;
                    $scope.completedLoadDashboard();
                },
                function (data) {
                    /* jshint unused: vars */
                    $scope.isLoadedCommunicationLogs = true;
                    $scope.completedLoadDashboard();
                });
        };
        $scope.getNumberOfVerifiedInfo = function () {
            if ($scope.customerInfo) {
                return ($scope.customerInfo.IsVerifyEmail ? 1 : 0) +
                    ($scope.customerInfo.IsVerifyPassport ? 1 : 0) +
                    ($scope.customerInfo.IsVerifyBirthday ? 1 : 0) +
                    ($scope.customerInfo.IsVerifySecretCode ? 1 : 0) +
                    ($scope.isShowBetCredit() && $scope.customerInfo.IsVerifyBetCredit ? 1 : 0) +
                    ($scope.isShowGivenCredit() && $scope.customerInfo.IsVerifyGivenCredit ? 1 : 0);
            }

            return 0;
        };
        $scope.closeEditRedemptionLogView = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            var popUpResult = parent.homeManager.CachedData.shareInfoBetweenPopup;
            $scope.isShowResultMessage = true;
            $scope.isSuccess = popUpResult.Code === crmConstants.Enums.Error.None;
            $scope.resultMessage = popUpResult.Message;
            window.scroll(1, 1);
            if ($scope.isSuccess) {
                setTimeout(function () {
                    $scope.isShowResultMessage = false;
                    $scope.$apply();
                }, 3000);
            }

            $scope.$apply();
            $scope.getRedemptionLogs();
        };
        $scope.editRedemptionLog = function (logId) {
            parent.homeManager.CachedData.shouldReloadView = false;
            /* jshint camelcase: false */
            parent.OpenPopup(
                window.rootUrl + "Redemption/OpenEditView?" + $.param({
                    logId: logId
                }),
                viewResources.Edit_Redemption_Log,
                $(parent.document).width() - 600,
                450,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditRedemptionLogView,
                ['center', 120],
                $scope.beforeCloseEditRedemptionLogView);
        };
        $scope.closeEditCommunicationLogView = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            var popUpResult = parent.homeManager.CachedData.shareInfoBetweenPopup;
            $scope.isShowResultMessage = true;
            $scope.isSuccess = popUpResult.Code === crmConstants.Enums.Error.None;
            $scope.resultMessage = popUpResult.Message;
            window.scroll(1, 1);
            if ($scope.isSuccess) {
                setTimeout(function () {
                    $scope.isShowResultMessage = false;
                    $scope.$apply();
                }, 3000);
            }

            $scope.$apply();
            $scope.getCommunicationLogs();
        };
        $scope.editCommunicationLog = function (logId) {
            parent.homeManager.CachedData.shouldReloadView = false;
            var url = window.rootUrl + "CommunicationLog/OpenEditView?" + $.param({
                logRunningNumber: logId
            });
            /* jshint camelcase: false */
            parent.OpenPopup(
                url,
                viewResources.Edit_Communication_Log,
                $(parent.document).width() - 300,
                $(parent.document).height(),
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditCommunicationLogView,
                null,
                $scope.beforeCloseEditCommunicationLogView);
        };
        $scope.beforeCloseEditCommunicationLogView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges,
                    popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.beforeCloseEditRedemptionLogView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges,
                    popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.editVerifyFlag = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            /* jshint camelcase: false */
            parent.OpenPopup(
                window.rootUrl + "Dashboard/OpenEditVerifyFlagView?" + $.param({
                    customerId: $scope.customerId
                }),
                viewResources.Verify_Customer_Info,
                500,
                260 + ($scope.getNumberOfNeededVerifiedInfo() - 4) * 30,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeEditView,
                ['center', 120],
                $scope.beforeCloseEditView);
        };
        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlE = function () {
            $scope.editVerifyFlag();
        };
        $scope.beforeCloseEditView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges,
                    popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.closeEditView = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            var popUpResult = parent.homeManager.CachedData.shareInfoBetweenPopup;
            $scope.isShowResultMessage = true;
            $scope.isSuccess = popUpResult.Code === crmConstants.Enums.Error.None;
            $scope.resultMessage = popUpResult.Message;
            window.scroll(1, 1);
            if ($scope.isSuccess) {
                setTimeout(function () {
                    $scope.isShowResultMessage = false;
                    $scope.$apply();
                }, 3000);
            }

            $scope.$apply();
            $scope.getCustomerInfo();
        };

        $scope.getNumberOfNeededVerifiedInfo = function () {
            return 4 +
                ($scope.isShowBetCredit() ? 1 : 0) +
                ($scope.isShowGivenCredit() ? 1 : 0);
        };
        $scope.isShowBetCredit = function () {
            return $scope.customerInfo && $scope.customerInfo.Id > 0 &&
               ($scope.customerInfo.RoleId === crmConstants.Enums.RoleId.Member ||
                $scope.customerInfo.RoleId === crmConstants.Enums.RoleId.CRMMember);
        };
        $scope.isShowGivenCredit = function () {
            return $scope.customerInfo && $scope.customerInfo.Id > 0 &&
               ($scope.customerInfo.RoleId === crmConstants.Enums.RoleId.Member ||
                $scope.customerInfo.RoleId === crmConstants.Enums.RoleId.CRMMember) &&
                $scope.customerInfo.IsCredit;
        };
        $scope.getVerifiedPercent = function () {
            return ($scope.getNumberOfVerifiedInfo() / $scope.getNumberOfNeededVerifiedInfo() * 100).toFixed(0);
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

        $scope.replaceLineBreak = function (text) {
            return text ? text.replace(/\r/g, '<br />').replace(/\n/g, '<br />') : "";
        };
        $scope.getRemarkText = function (index) {
            return '<span>' + (index + 1) + '.</span> ' +
                   '<span>' + $scope.replaceLineBreak($scope.customerRemarks[index]) + '</span>';
        };

        angular.element(document).ready(function () {
            $scope.initController();
            $scope.setFocus();
        });
    } ]);