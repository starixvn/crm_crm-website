'use strict';
/* jshint unused: vars */
/* jshint camelcase: false */
/* global informHeadlineStatisticApplication,
hotKeyManager,
crmConstants,
informStatisticResourceView */
informHeadlineStatisticApplication.controller('informHeadlineStatisticController',
    ['$scope',
    'informHeadlineStatisticService',
    '$uiBlockerService',
    'alertBoxService',
    function ($scope, informStatisticService, $uiBlockerService, alertBoxService) {
        $scope.statistic = null;
        $scope.defaultFocusOptions = null;

        $scope.getInformStatistic = function () {
            $uiBlockerService.blockUI();
            informStatisticService.getInformStatistic()
                .then(function (data) {
                    $scope.statistic = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
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

        $scope.openUpdateInform = function (titleId) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + 'InformHeadlines/OpenUpdateInform?titleId=' + titleId,
                informStatisticResourceView.UpdateInformHeadlines,
                $(parent.document).width() - 95,
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

            alertBoxService.show({
                type: crmConstants.Enums.MessageType.Success,
                message: informStatisticResourceView.CreateTitleSuccess
            });

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