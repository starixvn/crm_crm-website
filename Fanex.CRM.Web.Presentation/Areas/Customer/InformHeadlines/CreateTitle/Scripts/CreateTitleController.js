'use strict';
/*jshint unused: vars*/
/* jshint camelcase: false */
/* global createInformHeadlinelApplication,
hotKeyManager,
createInformResource */
createInformHeadlinelApplication.controller('createInformHeadlineTitleController', ['$scope', 'createInformHeadlineTitleService', '$uiBlockerService',
function ($scope, createTitleService, $uiBlockerService) {
    $scope.title = '';
    $scope.alertResult = {
        IsSuccess: true,
        Message: ''
    };

    $scope.createTitle = function () {
        if ($scope.title.length === 0) {
            $scope.alertResult = {
                IsSuccess: false,
                Message: createInformResource.InformHeadline_TitleEmpty
            };
            return;
        }

        $uiBlockerService.blockUI();
        var criteria = { Title: $scope.title };
        createTitleService.createTitle(criteria)
            .then(function (data) {
                var isSuccess = data.Result.Code === 0;
                $uiBlockerService.unblockUI();
                if (isSuccess) {
                    parent.homeManager.CachedData.shouldReloadView = true;
                    parent.ClosePopup();
                }
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
    };

    $scope.invokeHotKey_CtrlS = function () {
        $scope.createTitle();
    };

    angular.element(document).ready(function () {
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'CreateNewCase'
        });
    });
} ]);