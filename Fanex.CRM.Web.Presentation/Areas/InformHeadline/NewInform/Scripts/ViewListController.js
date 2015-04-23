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
        $scope.currencyDropdownSettings = {};
        $scope.positionDropdownSettings = {};
        $scope.siteDropdownSettings = {
            width: 220
        };
        $scope.headlineChecked = true;
        $scope.directChecked = true;

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

        function isFilterValid() {
            return $scope.selectedCurrencies.length > 0 &&
                $scope.selectedSites.length > 0 &&
                $scope.selectedPositions.length > 0;
        }

        $scope.getInforms = function () {
            var accountType = 0;
            // 1: headline, 2: direct, 3: Both
            if ($scope.headlineChecked) {
                accountType = 1;
            }

            if ($scope.directChecked) {
                accountType += 2;
            }

            if (accountType == 0 || isFilterValid() == false) {
                $scope.messageResultOptions = new MessageResult("Please fill out all fields.", true, false);
                return;
            } else {
                $scope.messageResultOptions = new MessageResult("", false, false);
            }

            $uiBlockerService.blockUI();
            _searchCriteria = {
                Currency: $scope.selectedCurrencies.toString(),
                Site: $scope.selectedSites.toString(),
                Level: $scope.selectedPositions.toString(),
                TypeAccount: accountType,
                PageIndex: 1,
                ItemsPerPage: 10000
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
            $uiBlockerService.blockUI();
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
                    $scope.messageResultOptions = new MessageResult("Add new case successfully!", true, true);
                    setTimeout(function () {
                        parent.ClosePopup();
                    }, sharedScriptManager.cachedData.alertSuccessTime);
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.messageResultOptions = new MessageResult("Add new case failed!", true, false);
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

        angular.element(document).ready(function () {
            initController();
        });
    } ]);