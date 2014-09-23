"use strict";
/* global verifyFlagApplication,
hotKeyManager,
viewResources,
crmConstants,
numeral,
sharedScriptManager */
verifyFlagApplication.controller('verifyFlagController', ['$scope',
    '$http',
    '$verifyFlagService',
    '$bootstrappedData',
    function ($scope,
        $http,
        $verifyFlagService,
        $bootstrappedData) {
        $scope.customerInfo = $bootstrappedData.customerInfo;
        $scope.oldCustomerInfo = angular.copy($scope.customerInfo);
        $scope.isSetFocus = false;
        $scope.isSuccess = false;
        $scope.isShowResultMessage = false;
        $scope.resultMessage = '';

        $scope.crmConstants = crmConstants;
        $scope.updateCustomerVerifyFlag = function () {
            $scope.BlockUI();
            var selectedCriteria = {
                CustomerId: $scope.customerInfo.Id,
                IsVerifyEmail: $scope.customerInfo.IsVerifyEmail,
                IsVerifyPassport: $scope.customerInfo.IsVerifyPassport,
                IsVerifyBirthday: $scope.customerInfo.IsVerifyBirthday,
                IsVerifySecretCode: $scope.customerInfo.IsVerifySecretCode,
                IsVerifyBetCredit: $scope.customerInfo.IsVerifyBetCredit,
                IsVerifyGivenCredit: $scope.customerInfo.IsVerifyGivenCredit
            };
            $verifyFlagService.updateCustomerVerifyFlag(selectedCriteria)
                .then(function (data) {
                    var updateResult = data.Result;
                    $scope.completedUpdate(updateResult);
                },
                function (data) {
                    /* jshint unused: vars */
                    $scope.unblockUI();
                });
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
        $scope.completedUpdate = function (updateResult) {
            updateResult = sharedScriptManager.formatAjaxResult(updateResult);
            if (updateResult.Code !== 0) {
                $scope.resultMessage = updateResult.Message;
                $scope.isShowResultMessage = true;
                $scope.isSuccess = false;
                $scope.unblockUI();
                return;
            }

            $scope.oldCustomerInfo = angular.copy($scope.customerInfo);
            parent.homeManager.CachedData.shareInfoBetweenPopup = updateResult;
            parent.homeManager.CachedData.shouldReloadView = true;
            $scope.unblockUI();
            parent.ClosePopup();
        };
        $scope.ignoreChanges = function () {
            $scope.oldCustomerInfo = angular.copy($scope.customerInfo);
            $scope.$apply();
            parent.ClosePopup();
        };
        $scope.updateChangeFlag = function () {
            if ($scope.oldCustomerInfo.IsVerifyEmail !== $scope.customerInfo.IsVerifyEmail ||
                $scope.oldCustomerInfo.IsVerifyPassport !== $scope.customerInfo.IsVerifyPassport ||
                $scope.oldCustomerInfo.IsVerifyBirthday !== $scope.customerInfo.IsVerifyBirthday ||
                $scope.oldCustomerInfo.IsVerifySecretCode !== $scope.customerInfo.IsVerifySecretCode ||
                $scope.oldCustomerInfo.IsVerifyBetCredit !== $scope.customerInfo.IsVerifyBetCredit ||
                $scope.oldCustomerInfo.IsVerifyGivenCredit !== $scope.customerInfo.IsVerifyGivenCredit) {
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                /* jshint camelcase: false */
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_EditVerifyFlag_Changes;
            } else {
                parent.homeManager.CachedData.isChangingDataPopUp = false;
            }
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
        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlS = function () {
            $scope.updateCustomerVerifyFlag();
        };

        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
        $scope.getFormatedCredit = function (creditAmount) {
            return numeral(creditAmount).format(crmConstants.Enums.FormatConstant.TwoDecimalNumber);
        };

        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
        };
        angular.element(document).ready(function () {
            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });

            $scope.setFocus();
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Dashboard_UpdateVerifyFlag"
            });
        });
    } ]);