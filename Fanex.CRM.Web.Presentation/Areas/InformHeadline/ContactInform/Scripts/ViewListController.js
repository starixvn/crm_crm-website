"use strict";
/* global contactInformApplication,
MessageResult,
sharedScriptManager,
viewResources, Site */
/* jshint unused: vars,
camelcase: false */

contactInformApplication.controller('ContactInformController',
    ['$scope', '$uiBlockerService', '$resultMessageService', '$timeout', 'updateInformService',
    function ($scope, $uiBlockerService, $resultMessageService, $timeout, updateInformService) {

        function handleUpdateFailed() {
            Site.showMessageBox(viewResources.UpdateFailed);
            $uiBlockerService.unblockUI();
        }

        $scope.showSaveButton = false;
        $scope.hasHostPermission = false;
        $scope.titleId = 0;
        $scope.titleDetail = null;
        $scope.listData = null;
        $scope.updatedItems = {};
        $scope.informStatuses = [];
        $scope.productStatuses = [];
        $scope.stations = [];
        $scope.username = '';

        var _searchCriteria = {};

        function initController() {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "InformHeadline_ContactInform"
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
                    getSettings();
                },
                function (data) {
                    $scope.titleDetail = null;
                    $uiBlockerService.unblockUI();
                });
        }

        function createStatusDropdownSettings(sourceData) {
            $scope.statusDropdownSettings = {
                idMember: "SettingId",
                nameMember: "SettingValue",
                selectedMember: "SettingId",
                sourceData: sourceData,
                width: 200,
                checkedAll: true,
                templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
            };
        }

        function getSettings() {
            $uiBlockerService.blockUI();

            updateInformService.getSettings()
            .then(function (data) {
                var allSetting = data.Result;

                $scope.informStatuses = allSetting.Statuses;
                $scope.productStatuses = allSetting.ProductStatuses;
                $scope.stations = allSetting.Stations;

                createStatusDropdownSettings($scope.informStatuses);

                $timeout($scope.getInforms, 1000);
            },
            function (data) {
                $uiBlockerService.unblockUI();
            });
        }
        $scope.exportInformHeadlines = function () {

            _searchCriteria = {
                UserName: $scope.username,
                TitleId: $scope.titleId,
                StatusId: $scope.selectedStatuses.toString(),
                PageIndex: 1,
                ItemsPerPage: 10000
            };

            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'InformHeadline/ExportContactInforms',
                Params: _searchCriteria
            };
        };
        $scope.getInforms = function () {

            $uiBlockerService.blockUI();

            _searchCriteria = {
                UserName: $scope.username,
                TitleId: $scope.titleId,
                StatusId: $scope.selectedStatuses.toString(),
                PageIndex: 1,
                ItemsPerPage: 10000
            };

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
            if ($scope.hasHostPermission !== true) {
                return;
            }

            $uiBlockerService.blockUI();
            var items = [];
            for (var key in $scope.updatedItems) {
                if ($scope.updatedItems.hasOwnProperty(key)) {
                    items.push($scope.updatedItems[key]);
                }
            }

            function saveSuccessCallback() {
                $scope.showSaveButton = false;
                $scope.messageResultOptions = new MessageResult("Save inform headlines successfully!", true, true);
                setTimeout(function () {
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
                $uiBlockerService.unblockUI();
            }

            if (items.length == 0) {
                saveSuccessCallback();
            } else {
                updateInformService.updateInforms({
                    TitleId: $scope.titleId,
                    Informs: items
                })
                .then(saveSuccessCallback,
                function (data) {
                    $scope.messageResultOptions = new MessageResult("Save inform headlines failed!", true, false);
                    $uiBlockerService.unblockUI();
                });
            }
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