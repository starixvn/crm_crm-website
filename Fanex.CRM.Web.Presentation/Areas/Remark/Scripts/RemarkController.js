"use strict";
/* global remarkApplication,
hotKeyManager,
remarkResources,
customerId */
remarkApplication.controller('remarkController', ['$scope', '$remarkService', function (
    $scope, $remarkService) {
    $scope.remarks = null;
    $scope.customerId = customerId;
    $scope.lastSpecialRemark = '';
    $scope.lastRemark = '';
    $scope.isShowSpecialRemark = true;
    $scope.isShowRemark = true;

    $scope.getRemarks = function () {
        $scope.blockUI();
        $remarkService.getRemarks($scope.customerId)
            .then(function (data) {
                $scope.remarks = data.Result;
                $scope.unblockUI();
            },
            function (data) {
                /*jshint unused: vars*/
                $scope.unblockUI();
            });
    };

    $scope.updateRemarks = function () {
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
                parent.dialogManager.alert(
                    remarkResources.Alert,
                    data.Result.Code === 0 ? remarkResources.UpdateRemarkSuccess : data.Result.Message,
                    remarkResources.OK);
                $scope.setPlaceHolder();
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

    angular.element(document).ready(function () {
        $scope.getRemarks();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'SpecialRemarks'
        });
    });
}]);