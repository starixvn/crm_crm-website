"use strict";
/* global newInformApplication,
sharedScriptManager,
viewResources, Site */
/* jshint unused: vars,
camelcase: false */

newInformApplication.controller('NewInformController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'newInformService',
    function ($scope, $uiBlockerService, $resultMessageService, newInformService) {
        function handleUpdateFailed() {
            Site.showMessageBox(viewResources.UpdateFailed);
            $uiBlockerService.unblockUI();
        }
        $scope.titleDetail = {};
        $scope.showSaveButton = false;
        $scope.listData = null;
        $scope.updatedItems = {};
        $scope.informStatuses = [];
        $scope.productStatuses = [];
        $scope.stations = [];

        var _searchCriteria = {};

        function initController() {
            sharedScriptManager.safeApply($scope);
            getAllSettings();
        }

        function getAllSettings() {
            $uiBlockerService.blockUI();
            $scope.positionDropdownSettings.sourceData = newInformService.getPositions();
            newInformService.getAllSettings()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.siteDropdownSettings.sourceData = allSetting.Sites[0].ChildSites;
                $scope.currencyDropdownSettings.sourceData = allSetting.Currencies;
                $scope.informStatuses = allSetting.Statuses;
                $scope.productStatuses = allSetting.ProductStatuses;
                $scope.stations = allSetting.Stations;
                $uiBlockerService.unblockUI();
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
        }

        $scope.getInforms = function () {

            $uiBlockerService.blockUI();
            var accountType = 0;
            // 1: headline, 2: direct, 3: Both
            if ($scope.headlineChecked) {
                accountType = 1;
            }

            if ($scope.directChecked) {
                accountType += 2;
            }

            _searchCriteria = {
                Currency: $scope.selectedCurrencies.toString(),
                Site: $scope.selectedSites.toString(),
                Level: $scope.selectedPositions.toString(),
                TypeAccount: accountType,
                PageIndex: 1,
                ItemsPerPage: 1000
            };
            newInformService.getInforms(_searchCriteria)
                .then(function (data) {
                    $scope.listData = data.Result.Items;
                    $scope.showSaveButton = data.Result.Items.length > 0;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.listData = null;
                    $scope.showSaveButton = false;
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.insertInforms = function () {
            var items = [];
            for (var key in $scope.updatedItems) {
                if ($scope.updatedItems.hasOwnProperty(key)) {
                    items.push($scope.updatedItems[key]);
                }
            }
            _searchCriteria.Title = $scope.titleDetail.Title;
            _searchCriteria.Informs = items;

            newInformService.insertInforms(_searchCriteria)
                .then(function (data) {
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.updateInformStatus = function (titleId, status) {
            $uiBlockerService.blockUI();

            newInformService.updateInformStatus({
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

        // New inform cases popup        
        $scope.updateInformHeadlines = function (titleId, status) {
            $uiBlockerService.blockUI();

            newInformService.updateInformStatus({
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
            checkedAll: true,
            hasFilter: false,
            idMember: "Id",
            nameMember: "Name",
            selectedMember: "Id",
            sourceData: null,
            width: 150,
            templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
        };
        $scope.positionDropdownSettings = {
            checkedAll: true,
            hasFilter: false,
            idMember: "Id",
            nameMember: "Name",
            selectedMember: "Id",
            sourceData: null,
            width: 150,
            templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
        };
        $scope.siteDropdownSettings = {
            checkedAll: true,
            hasFilter: false,
            idMember: "Id",
            nameMember: "Name",
            selectedMember: "Name",
            sourceData: null,
            width: 220,
            templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
        };

        angular.element(document).ready(function () {
            initController();
        });
    } ]);