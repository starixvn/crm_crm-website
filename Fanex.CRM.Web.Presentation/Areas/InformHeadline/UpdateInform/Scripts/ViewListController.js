"use strict";
/* global updateInformApplication,
sharedScriptManager,
viewResources, Site */
/* jshint unused: vars,
camelcase: false */

updateInformApplication.controller('UpdateInformController',
    ['$scope', '$uiBlockerService', '$resultMessageService', '$timeout', 'updateInformService',
    function ($scope, $uiBlockerService, $resultMessageService, $timeout, updateInformService) {

        var HEADLINE_ACCOUNT_TYPE = 1;
        var DIRECT_ACCOUNT_TYPE = 2;
        var BOTH_ACCOUNT_TYPE = 3;

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

        $scope.currencyDropdownSettings = {};
        $scope.positionDropdownSettings = {};
        $scope.siteDropdownSettings = {};


        function initController() {
            sharedScriptManager.safeApply($scope);
            getTitleDetail();
        }

        function getTitleDetail() {
            $uiBlockerService.blockUI();

            var params = {
                titleId: $scope.titleId
            };
            updateInformService.getTitleDetail(params)
                .then(function (data) {
                    $scope.titleDetail = data.Result;
                    getAllSettings();
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.titleDetail = null;
                    $uiBlockerService.unblockUI();
                });
        }

        function createCurrencyDropdownSettings(sourceData, currencyIds) {
            $scope.currencyDropdownSettings = {
                sourceData: sourceData,
                checkedFilter: function (item) {
                    return currencyIds.indexOf(item.Id.toString()) > -1;
                }
            };
        }

        function createPositionDropdownSettings(sourceData, positions) {
            $scope.positionDropdownSettings = {
                sourceData: sourceData,
                checkedFilter: function (item) {
                    return positions.indexOf(item.Id.toString()) > -1;
                }
            };
        }

        function createSiteDropdownSettings(sourceData, sites) {
            $scope.siteDropdownSettings = {
                width: 220,
                sourceData: sourceData,
                checkedFilter: function (item) {
                    return sites.indexOf(item.Name.toString()) > -1;
                }
            };
        }

        function getAllSettings() {
            $uiBlockerService.blockUI();
            createPositionDropdownSettings(updateInformService.getPositions(), $scope.titleDetail.Positions);

            updateInformService.getAllSettings()
            .then(function (data) {
                var allSetting = data.Result;

                $scope.informStatuses = allSetting.Statuses;
                $scope.productStatuses = allSetting.ProductStatuses;
                $scope.stations = allSetting.Stations;

                createCurrencyDropdownSettings(allSetting.Currencies, $scope.titleDetail.Currencies.split(','));
                createSiteDropdownSettings(allSetting.Sites[0].ChildSites, $scope.titleDetail.Sites);

                $scope.headlineChecked = $scope.titleDetail.TypeAccount == HEADLINE_ACCOUNT_TYPE ||
                                        $scope.titleDetail.TypeAccount == BOTH_ACCOUNT_TYPE;
                $scope.directChecked = $scope.titleDetail.TypeAccount == DIRECT_ACCOUNT_TYPE ||
                                        $scope.titleDetail.TypeAccount == BOTH_ACCOUNT_TYPE;
                $timeout($scope.getInforms, 1000);
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
                accountType = HEADLINE_ACCOUNT_TYPE;
            }

            if ($scope.directChecked) {
                accountType += DIRECT_ACCOUNT_TYPE;
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

        angular.element(document).ready(function () {
            initController();
        });
    } ]);