"use strict";
/* global viewListApplication,
sharedScriptManager,
registerOptions,
registerUpdateFunctions,
registerHotkeyFunctions */
/* jshint unused: vars,
camelcase: false */

viewListApplication.controller('viewListController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'viewListService',
    function ($scope, $uiBlockerService, $resultMessageService, viewListService) {
        $scope.listData = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getInforms();
        };
        $scope.getInforms = function () {
            $uiBlockerService.blockUI();

            viewListService.getInforms()
                .then(function (data) {
                    $scope.listData = data.Result.Items;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
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
        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);
            $scope.initController();
            $scope.registerHotKey();
        });
    } ]);