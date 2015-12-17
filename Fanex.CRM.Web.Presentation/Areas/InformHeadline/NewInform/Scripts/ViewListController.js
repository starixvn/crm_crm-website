(function () {
    "use strict";
    /* global MessageResult,
    hotKeyManager,
    sharedScriptManager,
    viewResources, Site */

    angular.module('newInformApplication').controller('NewInformController',
    ['$scope', '$uiBlockerService', 'newInformService',
    function ($scope, $uiBlockerService, newInformService) {
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
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "InformHeadline_NewInform"
            });
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
            function () {
                $uiBlockerService.unblockUI();
            });
        }

        function isFilterValid() {
            if (!_searchCriteria.Currency) {
                $scope.messageResultOptions = new MessageResult("Currency is required.", true, false);
                return false;
            } else if (!_searchCriteria.Level) {
                $scope.messageResultOptions = new MessageResult("Position is required.", true, false);
                return false;
            } else if (!_searchCriteria.Site) {
                $scope.messageResultOptions = new MessageResult("Site is required.", true, false);
                return false;
            } else if (_searchCriteria.TypeAccount === 0) {
                $scope.messageResultOptions = new MessageResult("Direct Account is required.", true, false);
                return false;
            }
            return true;
        }

        function refreshSearchCriteria() {
            _searchCriteria = {
                Title: $scope.titleDetail.Title,
                Currency: $scope.selectedCurrencies.toString(),
                Site: $scope.selectedSites.toString(),
                Level: $scope.selectedPositions.toString(),
                TypeAccount: getAccountType(),
                PageIndex: 1,
                ItemsPerPage: 10000
            };
        }

        function getAccountType() {
            var accountType = 0;
            // 1: headline, 2: direct, 3: Both
            if ($scope.headlineChecked) {
                accountType = 1;
            }

            if ($scope.directChecked) {
                accountType += 2;
            }
            return accountType;
        }

        $scope.exportInformHeadlines = function () {
            if (isFilterValid() === false) {
                return;
            } else {
                $scope.messageResultOptions = new MessageResult("", false, false);
            }

            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'InformHeadline/ExportNewInforms',
                Params: _searchCriteria
            };
        };
        $scope.getInforms = function () {
            refreshSearchCriteria();
            if (isFilterValid() === false) {
                $scope.showSaveButton = false;
                return;
            } else {
                $scope.messageResultOptions = new MessageResult("", false, false);
            }

            $uiBlockerService.blockUI();

            newInformService.getInforms(_searchCriteria)
                .then(function (data) {
                    $scope.listData = data.Result.Items;
                    $scope.showSaveButton = data.Result.Items.length > 0;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $scope.listData = null;
                    $scope.showSaveButton = false;
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.insertInforms = function () {
            refreshSearchCriteria();
            var title = $scope.titleDetail.Title;
            if (!title || title.trim() === "") {
                $scope.messageResultOptions = new MessageResult("Title is required.", true, false);
                $uiBlockerService.unblockUI();
                return;
            }

            if (isFilterValid() === false) {
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

            newInformService.insertInforms(_searchCriteria)
                .then(function () {
                    $scope.showSaveButton = false;
                    $scope.messageResultOptions = new MessageResult("Add new case successfully!", true, true);
                    setTimeout(function () {
                        parent.ClosePopup();
                    }, sharedScriptManager.cachedData.alertSuccessTime);
                    $uiBlockerService.unblockUI();
                },
                function () {
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
})();