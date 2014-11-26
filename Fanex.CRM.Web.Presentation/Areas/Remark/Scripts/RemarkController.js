"use strict";
/* global remarkApplication,
hotKeyManager,
remarkResources,
crmConstants,
customerId,
sharedScriptManager */
remarkApplication.controller('remarkController',
    ['$scope',
    '$remarkService',
    '$uiBlockerService',
    'alertBoxService',
    function ($scope, $remarkService, $uiBlockerService, alertBoxService) {
        $scope.customerId = customerId;
        $scope.remarks = null;
        $scope.oldRemarks = null;
        $scope.lastSpecialRemark = '';
        $scope.lastRemark = '';
        $scope.isShowSpecialRemark = true;
        $scope.isShowRemark = true;
        $scope.isSetFocus = false;

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
                    var result = sharedScriptManager.formatAjaxResult(data.Result);
                    var alertResult;
                    if (result.Code === 0) {
                        alertResult = {
                            type: crmConstants.Enums.MessageType.Success,
                            message: remarkResources.UpdateRemarkSuccess
                        };
                    }
                    else {
                        alertResult = {
                            type: crmConstants.Enums.MessageType.Error,
                            message: result.Message
                        };
                    }
                    alertBoxService.show(alertResult);
                    //$scope.setPlaceHolder();
                    /*Must call getRemarks() function to reset change*/
                    $scope.lastSpecialRemark = '';
                    $scope.lastRemark = '';
                    $scope.resetLastRemark(0);
                    $scope.resetLastRemark(1);
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
            if ($scope.lastSpecialRemark.length === 0 ||
                $scope.lastSpecialRemark === "<br>" ||
                $scope.lastSpecialRemark === remarkResources.RemarkPlaceHolder) {
                return;
            }

            $scope.remarks.SpecialRemarks.push($scope.lastSpecialRemark);
            $scope.lastSpecialRemark = '';
            $scope.resetLastRemark(1);
        };

        $scope.insertRemark = function () {
            if ($scope.lastRemark.length === 0 ||
                $scope.lastRemark === "<br>" ||
                $scope.lastRemark === remarkResources.RemarkPlaceHolder) {
                return;
            }

            $scope.remarks.NormalRemarks.push($scope.lastRemark);
            $scope.lastRemark = '';
            $scope.resetLastRemark(0);
        };

        $scope.resetLastRemark = function (isSpecialRemark) {
            // Fix Angular Problem
            $('.last-remark .rich-remark-editor-directive[data-isspecialremark=' + isSpecialRemark + '] textarea')
                .val(remarkResources.RemarkPlaceHolder)
                .htmlarea('updateHtmlArea');
        };

        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlS = function () {
            $scope.updateRemarks();
        };

        $scope.setPlaceHolder = function () {
            $('textarea[defaulttext]').each(function () {
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
            $uiBlockerService.blockUI();
        };

        $scope.unblockUI = function () {
            $uiBlockerService.unblockUI();
        };

        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };

        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
        };

        $scope.updateRemark = function (remark, isSpecialRemark, remarkIndex) {
            if (isSpecialRemark) {
                if (remarkIndex !== $scope.remarks.SpecialRemarks.length) {
                    $scope.remarks.SpecialRemarks[remarkIndex] = remark;
                }
                else {
                    $scope.lastSpecialRemark = remark;
                }
            } else {
                if (remarkIndex !== $scope.remarks.NormalRemarks.length) {
                    $scope.remarks.NormalRemarks[remarkIndex] = remark;
                } else {
                    $scope.lastRemark = remark;
                }
            }

            $scope.safeApply();
        };

        angular.element(document).ready(function () {
            sharedScriptManager.safeApply($scope);
            $scope.getRemarks();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'SpecialRemarks'
            });
            $scope.setFocus();
        });
    } ]);