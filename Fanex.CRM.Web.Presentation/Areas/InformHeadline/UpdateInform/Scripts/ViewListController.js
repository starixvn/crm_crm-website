"use strict";
/* global updateInformApplication,
MessageResult,
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
        var _searchCriteria = {};

        function initController() {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "InformHeadline_UpdateInform"
            });
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
                    $scope.titleDetail.Title = decodeHtml($scope.titleDetail.Title);
                    getAllSettings();
                },
                function (data) {
                    $scope.titleDetail = null;
                    $uiBlockerService.unblockUI();
                });
        }
        function isFilterChanged() {
            var titleDetail = $scope.titleDetail;

            return _searchCriteria.Currency !== titleDetail.Currencies ||
                    _searchCriteria.Site !== titleDetail.Sites ||
                    _searchCriteria.TypeAccount !== titleDetail.TypeAccount ||
                    _searchCriteria.Level !== titleDetail.Positions;
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
                    return sites.indexOf(item.Id.toString()) > -1;
                }
            };
        }

        function isFilterValid() {
            return $scope.selectedCurrencies.length > 0 &&
                $scope.selectedSites.length > 0 &&
                $scope.selectedPositions.length > 0;
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

                $scope.headlineChecked = $scope.titleDetail.TypeAccount === HEADLINE_ACCOUNT_TYPE ||
                                        $scope.titleDetail.TypeAccount === BOTH_ACCOUNT_TYPE;
                $scope.directChecked = $scope.titleDetail.TypeAccount === DIRECT_ACCOUNT_TYPE ||
                                        $scope.titleDetail.TypeAccount === BOTH_ACCOUNT_TYPE;

                $timeout($scope.getInforms, 2000);
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
        }

        function refreshSearchCriteria() {
            var accountType = 0;
            // 1: headline, 2: direct, 3: Both
            if ($scope.headlineChecked) {
                accountType = 1;
            }

            if ($scope.directChecked) {
                accountType += 2;
            }

            _searchCriteria = {
                TitleId: $scope.titleId,
                Currency: $scope.selectedCurrencies.toString(),
                Site: $scope.selectedSites.toString(),
                Level: $scope.selectedPositions.toString(),
                TypeAccount: accountType,
                PageIndex: 1,
                ItemsPerPage: 10000
            };

            _searchCriteria.ChangeFilter = isFilterChanged();
        }

        function decodeHtml(text) {
            return angular.element('<div/>').html(text).text();
        }

        $scope.exportInformHeadlines = function () {
            refreshSearchCriteria();

            if (_searchCriteria.TypeAccount === 0 || isFilterValid() === false) {
                $scope.messageResultOptions = new MessageResult("Please fill out all fields.", true, false);
                return;
            } else {
                $scope.messageResultOptions = new MessageResult("", false, false);
            }

            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'InformHeadline/ExportUpdateInforms',
                Params: _searchCriteria
            };
        };
        $scope.getInforms = function () {
            refreshSearchCriteria();

            if (_searchCriteria.TypeAccount === 0 || isFilterValid() === false) {
                $scope.messageResultOptions = new MessageResult("Please fill out all fields.", true, false);
                $uiBlockerService.unblockUI();
                $scope.showSaveButton = false;
                return;
            } else {
                $scope.messageResultOptions = new MessageResult("", false, false);
            }

            $uiBlockerService.blockUI();

            updateInformService.getInforms(_searchCriteria)
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
            var title = $scope.titleDetail.Title;
            if (!title || title.trim() === "") {
                $scope.messageResultOptions = new MessageResult("Title is required.", true, false);
                $uiBlockerService.unblockUI();
                return;
            }

            $uiBlockerService.blockUI();
            var items = [];
            for (var key in $scope.updatedItems) {
                if ($scope.updatedItems.hasOwnProperty(key)) {
                    items.push($scope.updatedItems[key]);
                }
            }

            _searchCriteria.Title = title.trim();
            _searchCriteria.Informs = items;
            _searchCriteria.ChangeFilter = isFilterChanged();

            updateInformService.updateInforms(_searchCriteria)
                .then(function (data) {
                    $scope.showSaveButton = false;
                    $scope.messageResultOptions = new MessageResult("Save inform headlines successfully!", true, true);
                    setTimeout(function () {
                        parent.ClosePopup();
                    }, sharedScriptManager.cachedData.alertSuccessTime);
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $scope.messageResultOptions = new MessageResult("Save inform headlines failed!", true, false);
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