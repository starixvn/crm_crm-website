/*global hotKeyManager*/
"use strict";
/* global sharedScriptManager,
viewResources, Site */

angular.module('informHeadlineApplication').controller('ViewListController',
    ['$scope', '$uiBlockerService', 'viewListService',
    function ($scope, $uiBlockerService, viewListService) {

        var POPUP_WIDTH = 1200, POPUP_HEIGHT = 800;

        $scope.contactPolicy = null;
        $scope.searchPolicy = null;
        $scope.listData = null;

        function handleUpdateFailed() {
            Site.showMessageBox(viewResources.UpdateFailed);
            $uiBlockerService.unblockUI();
        }

        function registerHotKey() {
            $scope.registerHotKey = function () {
                hotKeyManager.CachedData.ActiveDocumentManager.push({
                    Value: $scope,
                    Id: "InformHeadline_ViewInform"
                });
            };
        }

        $scope.initController = function () {
            registerHotKey();
            sharedScriptManager.safeApply($scope);
            $scope.getInforms();
        };

        $scope.getInforms = function () {
            $uiBlockerService.blockUI();

            viewListService.getInforms()
                .then(function (data) {
                    $scope.listData = data.Result.Items;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $scope.listData = null;
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.exportInformHeadlines = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'InformHeadline/ExportInforms',
                Params: { IsCached: true }
            };

            $scope.$apply();
        };
        $scope.updateInformStatus = function (titleId, status) {
            $uiBlockerService.blockUI();

            viewListService.updateInformStatus({
                titleId: titleId,
                statusType: status.toString() === "1" // false : Open, true: Close
            })
                .then(function (data) {
                    if (data.Result.Code > 0) {
                        handleUpdateFailed();
                    } else {
                        $scope.getInforms();
                    }
                },
                handleUpdateFailed);
        };
        $scope.showAddCaseDialog = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "InformHeadline/NewInform",
                viewResources.NewInformHeadlines,
                POPUP_WIDTH,
                POPUP_HEIGHT,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.getInforms,
                { my: "center", at: "center" },
                $scope.beforeCloseEditView);
        };
        $scope.showUpdateCaseDialog = function (titleId) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "InformHeadline/UpdateInform?" +
                    $.param({
                        titleId: titleId
                    }),
                viewResources.UpdateInformHeadlines + ' - ' + viewResources.EditNumberOfAccounts,
                POPUP_WIDTH,
                POPUP_HEIGHT,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.getInforms,
                { my: "center", at: "center" },
                $scope.beforeCloseEditView);
        };
        $scope.showContactDialog = function (titleId, isClosed) {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "InformHeadline/ContactInform?" +
                    $.param({
                        titleId: titleId,
                        isClosed: isClosed
                    }),
                viewResources.UpdateInformHeadlines,
                POPUP_WIDTH,
                POPUP_HEIGHT,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.getInforms,
                { my: "center", at: "center" },
                $scope.beforeCloseEditView);
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);