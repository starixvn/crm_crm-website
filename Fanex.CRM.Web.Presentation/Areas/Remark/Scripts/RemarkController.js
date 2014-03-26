"use strict";
/* global remarkApplication,
hotKeyManager,
remarkResources,
customerId,
crmConstants */
remarkApplication.controller('remarkController', ['$scope', '$remarkService', '$timeout', function (
    $scope, $remarkService, $timeout) {
    $scope.customerId = customerId;
    $scope.remarks = null;
    $scope.oldRemarks = null;
    $scope.lastSpecialRemark = '';
    $scope.lastRemark = '';
    $scope.isShowSpecialRemark = true;
    $scope.isShowRemark = true;
    $scope.isSetFocus = false;
    $scope.alertResult = {
        IsSuccess: true,
        Message: ''
    };

    $scope.getRemarks = function () {
        $scope.blockUI();
        $remarkService.getRemarks($scope.customerId)
            .then(function (data) {
                $scope.remarks = data.Result;
                $scope.oldRemarks = angular.copy($scope.remarks);
                /*trigger form data change*/
                $scope.$watchCollection('[lastSpecialRemark, lastRemark]', function () {
                    $scope.checkFormDataChange();
                });
                $scope.$watchCollection('remarks.SpecialRemarks', function () {
                    $scope.checkFormDataChange();
                });
                $scope.$watchCollection('remarks.NormalRemarks', function () {
                    $scope.checkFormDataChange();
                });
                $scope.unblockUI();
            },
            function (data) {
                /*jshint unused: vars*/
                $scope.unblockUI();
            });
    };

    $scope.updateRemarks = function () {
        if (parent.homeManager.CachedData.isChangingDataLeftPanel === true) {
            parent.homeManager.showConfirmIgnoreChange($scope.updateRemarksAfterPreventUnsavedAction);
            return;
        }

        $scope.updateRemarksAfterPreventUnsavedAction();
    };

    $scope.updateRemarksAfterPreventUnsavedAction = function () {
        $scope.blockUI();
        if ($scope.lastSpecialRemark.length !== 0) {
            $scope.remarks.SpecialRemarks.push($scope.lastSpecialRemark);
            $scope.lastSpecialRemark = '';
        }
        if ($scope.lastRemark.length !== 0) {
            $scope.remarks.NormalRemarks.push($scope.lastRemark);
            $scope.lastRemark = '';
        }
        $remarkService.updateRemarks($scope.remarks)
            .then(function (data) {
                var result = data.Result;
                if (result.Code === undefined || result.Code === null) {
                    result = {
                        Code: crmConstants.Enums.Error.Forbidden,
                        Message: "You don't have permission on this function!"
                    };
                }

                var isSuccess = result.Code === 0;
                $scope.alertResult = {
                    IsSuccess: isSuccess,
                    Message: isSuccess ? remarkResources.UpdateRemarkSuccess : result.Message
                };
                if (isSuccess) {
                    $timeout(function () {
                        $scope.alertResult.Message = '';
                    }, 3000);
                }
                $scope.setPlaceHolder();
                /*Must call getRemarks() function to reset change*/
                $scope.getRemarks();
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
    };

    $scope.deleteRemark = function (isSpecial, index) {
        var remarkList = isSpecial ? $scope.remarks.SpecialRemarks : $scope.remarks.NormalRemarks;
        remarkList.splice(index, 1);
    };

    $scope.insertSpecialRemark = function () {
        if ($scope.lastSpecialRemark.length === 0) {
            return;
        }
        $scope.remarks.SpecialRemarks.push($scope.lastSpecialRemark);
        $scope.lastSpecialRemark = '';
    };

    $scope.insertRemark = function () {
        if ($scope.lastRemark.length === 0) {
            return;
        }
        $scope.remarks.NormalRemarks.push($scope.lastRemark);
        $scope.lastRemark = '';
    };

    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlS = function () {
        $scope.updateRemarks();
    };

    $scope.setPlaceHolder = function () {
        $('input[defaulttext]').each(function () {
            var input = $(this);
            $(input).val(input.attr('defaulttext'));
        });
    };

    $scope.checkFormDataChange = function () {
        var isDataChange = $scope.compareOldNewValues();
        parent.homeManager.CachedData.isChangingData = isDataChange;
        if (isDataChange) {
            parent.homeManager.CachedData.ignoreConfirmMessage = remarkResources.Confirm_Ignore_EditCustomer_ChangesTabRemark;
        }
    };

    $scope.compareOldNewValues = function () {
        /*check last remark change*/
        if ($scope.lastSpecialRemark.length !== 0 || $scope.lastRemark.length !== 0) {
            return true;
        }

        /*check remark collection change*/
        if ($scope.remarks === null) {
            return false;
        }

        var specialLength = $scope.remarks.SpecialRemarks.length;
        if (specialLength !== $scope.oldRemarks.SpecialRemarks.length) {
            return true;
        }

        for (var i = 0; i < specialLength; i++) {
            if ($scope.remarks.SpecialRemarks[i] !== $scope.oldRemarks.SpecialRemarks[i]) {
                return true;
            }
        }

        var normalLength = $scope.remarks.NormalRemarks.length;
        if (normalLength !== $scope.oldRemarks.NormalRemarks.length) {
            return true;
        }

        for (var j = 0; j < normalLength; j++) {
            if ($scope.remarks.NormalRemarks[j] !== $scope.oldRemarks.NormalRemarks[j]) {
                return true;
            }
        }

        return false;
    };

    $scope.blockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) { }
    };

    $scope.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) { }
    };

    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };

    $scope.setFocus = function () {
        $scope.$apply(function () {
            $scope.isSetFocus = true;
        });
    };

    angular.element(document).ready(function () {
        $scope.getRemarks();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'SpecialRemarks'
        });
        $scope.setFocus();
    });
} ]);