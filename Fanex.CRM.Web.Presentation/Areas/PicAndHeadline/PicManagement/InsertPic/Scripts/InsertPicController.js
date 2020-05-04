(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('insertpic-application').controller('insertPicController', ['$scope', '$insertPicService', '$compile',
        function ($scope, $insertPicService, $compile) { 
            $scope.errorMessage = '';
            $scope.roleOptions = null;
            $scope.reportOptions = {
                DataList: null,
                IsResize: false,
                IsUpdateGrid: false
            };
            $scope.searchCriteria = {
                Username: '',
                RoleId: null,
                PageIndex: 1,
                PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
            };
            $scope.lastSelectedParams = angular.copy($scope.searchCriteria);

            $scope.records = [
                "Alfreds Futterkiste"
            ]

            $scope.initController = function () {
                $scope.initPicker();
               
                $scope.getAllSetting();
                $scope.defaultFocusOptions = {
                    IsSetFocus: false,
                    OnChangeEvent: $scope.completeSetFocus
                };

                $scope.defaultFocusOptions.IsSetFocus = true;
                $scope.$apply();
            };

            $scope.initPicker = function () {
                $scope.roleOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedRole,
                    AllText: viewResources.All
                };
            };

            $scope.getAllSetting = function () {
                $scope.BlockUI();
                $scope.isLoadedAllSite = false;
                $insertPicService.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.roleOptions.DataList = allSetting.RoleList;
                        $scope.completedLoadFilterInfo();
                    }, function () {
                        $scope.completedLoadFilterInfo();
                    });
            };

            $scope.completedLoadFilterInfo = function () {
                $scope.searchCriteria.role = $scope.roleOptions.DataList[0];
                $scope.unblockUI();
            };
            
            $scope.updateSelectedRole = function (role) {
                $scope.searchCriteria.role = role;
            };
           
            $scope.savePic = function () {      
                $scope.BlockUI();
                var getReportCriteria = {
                    Username: $scope.searchCriteria.Username,
                    RoleId: $scope.searchCriteria.role.Id,
                    PageIndex: $scope.searchCriteria.PageIndex,
                    PageSize: $scope.searchCriteria.PageSize                    
                };

                $scope.lastSelectedParams = getReportCriteria;
                $insertPicService.savePic(getReportCriteria)
                    .then(function (data) {
                        console.log(data);
                        $scope.reportOptions.DataList = data.Result;
                        $scope.reportOptions.IsUpdateGrid = true;
                        $scope.unblockUI();
                    }, function () {
                        $scope.unblockUI();
                    });
            };

            $scope.addCommunicationTool = function () {
                console.log("adding");
                $scope.records.push($scope.records.length + 1);

                
            }

            $scope.completeSetFocus = function () {
                $scope.defaultFocusOptions.IsSetFocus = false;
            };

            $scope.unblockUI = function () {
                try {
                    if (parent && parent.$.unblockUI) {
                        parent.$.unblockUI();
                    }
                } catch (e) {
                }

                hotKeyManager.CachedData.isBlockingUI = false;
            };

            $scope.BlockUI = function () {
                hotKeyManager.CachedData.isBlockingUI = true;
                try {
                    if (parent && parent.BlockUI) {
                        parent.BlockUI();
                    }
                } catch (e) {
                }
            };

            angular.element(document).ready(function () {
                $scope.initController();
            });
        }]);
})();