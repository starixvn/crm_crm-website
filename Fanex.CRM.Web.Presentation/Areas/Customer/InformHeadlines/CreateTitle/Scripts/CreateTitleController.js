'use strict';
/* jshint unused: vars */
/* jshint camelcase: false */
/* global createInformHeadlinelApplication,
hotKeyManager,
crmConstants,
createInformResource */
createInformHeadlinelApplication.controller('createInformHeadlineTitleController',
    ['$scope',
    'createInformHeadlineTitleService',
    '$uiBlockerService',
    'alertBoxService',
    function ($scope, createTitleService, $uiBlockerService, alertBoxService) {
        $scope.title = '';

        $scope.createTitle = function () {
            if ($scope.title.length === 0) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: createInformResource.InformHeadline_TitleEmpty
                });
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