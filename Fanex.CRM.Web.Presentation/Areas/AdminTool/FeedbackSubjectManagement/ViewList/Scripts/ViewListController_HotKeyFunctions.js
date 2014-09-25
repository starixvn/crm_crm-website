"use strict";
/* jshint unused: vars,
camelcase: false */
/* global hotKeyManager */
/* exported registerHotkeyFunctions */
function registerHotkeyFunctions($scope) {
    $scope.registerHotKey = function () {
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "AdminTool_FeedbackSubjectManagement"
        });
    };
    $scope.invokeHotKey_CtrlS = function () {
        $scope.searchTemplate();
        $scope.safeApply();
    };
    $scope.invokeHotKey_CtrlI = function () {
        $scope.showInsertPopup();
    };
}