"use strict";
/* global updateInformApplication,
sharedScriptManager,
viewResources, Site */
/* jshint unused: vars,
camelcase: false */

updateInformApplication.controller('UpdateInformController',
    ['$scope', '$uiBlockerService', '$resultMessageService', 'updateInformService',
    function ($scope, $uiBlockerService, $resultMessageService, updateInformService) {
        function handleUpdateFailed() {
            Site.showMessageBox(viewResources.UpdateFailed);
            $uiBlockerService.unblockUI();
        }
        $scope.titleId = 0;
        $scope.titleDetail = null;
        $scope.showSaveButton = false;
        $scope.listData = null;
        $scope.updatedItems = {};
        $scope.informStatuses = [];
        $scope.productStatuses = [];
        $scope.stations = [];

        function initController() {
            sharedScriptManager.safeApply($scope);
            getTitleDetail();
            getAllSettings();
            console.log($scope.titleDetail);
        }

        function getTitleDetail() {
            $uiBlockerService.blockUI();

            var params = {
                titleId: $scope.titleId
            };
            updateInformService.getTitleDetail(params)
                .then(function (data) {
                    $scope.titleDetail = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.titleDetail = null;
                    $uiBlockerService.unblockUI();
                });
        }

        function getAllSettings() {
            $uiBlockerService.blockUI();
            $scope.positionDropdownSettings.sourceData = updateInformService.getPositions();
            updateInformService.getAllSettings()
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

            var params = {
                TitleId: $scope.titleId,
                Currency: $scope.selectedCurrencies.toString(),
                Site: $scope.selectedSites.toString(),
                Level: $scope.selectedPositions.toString(),
                TypeAccount: accountType,
                PageIndex: 1,
                ItemsPerPage: 1000
            };
            updateInformService.getInforms(params)
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
        $scope.updateInforms = function () {
            var items = [];
            for (var key in $scope.updatedItems) {
                if ($scope.updatedItems.hasOwnProperty(key)) {
                    items.push($scope.updatedItems[key]);
                }
            }
            _searchCriteria.Title = $scope.titleDetail.Title;
            var params = {
                InsertCriteria: _searchCriteria,
                Informs: items
            };
            newInformService.updateInforms(params)
                .then(function (data) {
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.updateInformStatus = function (titleId, status) {
            $uiBlockerService.blockUI();

            updateInformService.updateInformStatus({
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

            updateInformService.updateInformStatus({
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