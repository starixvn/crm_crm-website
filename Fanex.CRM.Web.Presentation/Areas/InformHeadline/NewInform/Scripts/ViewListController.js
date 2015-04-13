"use strict";
/* global viewListApplication,
sharedScriptManager,
registerOptions,
registerUpdateFunctions,
registerHotkeyFunctions,
viewResources, Site */
/* jshint unused: vars,
camelcase: false */

viewListApplication.controller('viewListController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'viewListService',
    function ($scope, $uiBlockerService, $resultMessageService, viewListService) {
        function handleUpdateFailed() {
            Site.showMessageBox(viewResources.UpdateFailed);
            $uiBlockerService.unblockUI();
        }

        $scope.listData = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getAllSettings();
        };
        $scope.updateSelectedSite = function (siteIdList) {
            $scope.searchCriteria.SiteIds = siteIdList;
        };
        $scope.getDirectAccounts = function () {
            $uiBlockerService.blockUI();
            var accountType = 0;
            // 1: headline, 2: direct, 3: Both
            if($scope.headlineChecked)
            {
                accountType = 1;
            }

            if($scope.directChecked)
            {
                accountType += 2;
            }

            var params = {
                Currency: $scope.selectedCurrencies.toString(),
                Site: $scope.selectedSites.toString(),
                Level: $scope.selectedPositions.toString(),
                TypeAccount: accountType,
                PageIndex: 1,
                ItemsPerPage: 100
            };
            viewListService.getDirectAccounts(params)
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
                        $scope.getDirectAccounts();
                    }
                },
                handleUpdateFailed);
        };

        $scope.currencyDropdownSettings = {
            hasFilter: false,
            idMember: "Id",
            nameMember: "Name",
            selectedMember: "Id",
            sourceData: null,
            width: 150,
            templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
        };
        $scope.positionDropdownSettings = {
            hasFilter: false,
            idMember: "Id",
            nameMember: "Name",
            selectedMember: "Id",
            sourceData: null,
            width: 150,
            templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
        };
        $scope.siteDropdownSettings = {
            hasFilter: false,
            idMember: "Id",
            nameMember: "Name",
            selectedMember: "Name",
            sourceData: null,
            width: 220,
            templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
        };
        $scope.getAllSettings = function () {
            $uiBlockerService.blockUI();
            $scope.positionDropdownSettings.sourceData = viewListService.getPositions();
            viewListService.getAllSettings()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.siteDropdownSettings.sourceData = allSetting.Sites[0].ChildSites;
                $scope.currencyDropdownSettings.sourceData = allSetting.Currencies;
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
        };
        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);
            $scope.initController();
            $scope.registerHotKey();
        });
    } ]);