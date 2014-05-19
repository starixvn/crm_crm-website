'use strict';
/* jshint camelcase: false */
/* global informHeadlineStatisticApplication,
hotKeyManager,
informStatisticResourceView */
informHeadlineStatisticApplication.controller('informHeadlineStatisticController', ['$scope', 'informHeadlineStatisticService', '$uiBlockerService', '$timeout',
function ($scope, informStatisticService, $uiBlockerService, $timeout) {
    $scope.statistic = null;
    $scope.alertResult = {
        IsSuccess: true,
        Message: ''
    };
    $scope.defaultFocusOptions = null;

    $scope.getInformStatistic = function () {
        $uiBlockerService.blockUI();
        informStatisticService.getInformStatistic()
            .then(function (data) {
                $scope.statistic = data.Result;
                $uiBlockerService.unblockUI();
            },
            function (data) {
                /*jshint unused: vars*/
                $uiBlockerService.unblockUI();
            });
    };

    $scope.exportInformStatisticExcel = function () {
        $scope.downloadFileOptions = {
            URL: window.rootUrl + 'InformHeadlines/ExportInformStatistic',
            Params: { IsCached: true }
        };

        $scope.$apply();
    };

    $scope.openCreateTitle = function () {
        parent.OpenPopup(
            window.rootUrl + 'InformHeadlines/OpenCreateTitle',
            informStatisticResourceView.NewCase,
            400,
            180,
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeCreateTitle,
            ['center', 120],
            null);
    };

    $scope.openUpdateInform = function (titleId, title) {
        parent.homeManager.CachedData.shouldReloadView = false;
        parent.OpenPopup(
            window.rootUrl + 'InformHeadlines/OpenUpdateInform?' + $.param({ titleId: titleId, title: title }),
            informStatisticResourceView.UpdateInformHeadlines,
            $(parent.document).width() - 400,
            $(parent.document).height(),
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeUpdateInform,
            null,
            $scope.beforeCloseUpdateInform);
    };

    $scope.closeCreateTitle = function () {
        $scope.$apply(function () {
            $scope.defaultFocusOptions.IsSetFocus = true;
        });

        if (!parent.homeManager.CachedData.shouldReloadView) {
            return;
        }

        $scope.alertResult = {
            IsSuccess: true,
            Message: informStatisticResourceView.CreateTitleSuccess
        };
        $timeout(function () {
            $scope.alertResult.Message = '';
        }, 3000);

        $scope.getInformStatistic();
        parent.homeManager.CachedData.shouldReloadView = false;
    };

    $scope.closeUpdateInform = function () {
        $scope.$apply(function () {
            $scope.defaultFocusOptions.IsSetFocus = true;
        });

        if (!parent.homeManager.CachedData.shouldReloadView) {
            return;
        }

        $scope.getInformStatistic();
        parent.homeManager.CachedData.shouldReloadView = false;
    };

    $scope.beforeCloseUpdateInform = function () {
        var homeManager = parent.homeManager;
        if (!homeManager.CachedData.isChangingDataPopUp) {
            return true;
        }

        homeManager.CachedData.ignoreConfirmMessagePopUp = informStatisticResourceView.Confirm_Ignore_UpdateInforms_Changes;
        homeManager.showConfirmIgnoreChangePopUp(function () {
            parent.ClosePopup();
        });

        return false;
    };

    $scope.invokeHotKey_CtrlE = function () {
        $scope.openCreateTitle();
    };

    $scope.completeSetFocus = function () {
        $scope.defaultFocusOptions.IsSetFocus = false;
    };

    angular.element(document).ready(function () {
        $scope.getInformStatistic();

        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'InformHeadlinesStatistic'
        });

        $scope.defaultFocusOptions = {
            IsSetFocus: false,
            OnChangeEvent: $scope.completeSetFocus
        };

        $scope.defaultFocusOptions.IsSetFocus = true;
        $scope.$apply();
    });
} ]);