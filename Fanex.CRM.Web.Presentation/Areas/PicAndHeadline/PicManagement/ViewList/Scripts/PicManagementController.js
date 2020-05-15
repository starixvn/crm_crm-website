(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('picmanagement-application').controller('picManagementController', ['$scope', '$picManagementService',
        function ($scope, $picManagementService) { 
            $scope.errorMessage = '';
            $scope.roleOptions = null;
            $scope.communicationTools = [];
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
                $picManagementService.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.roleOptions.DataList = allSetting.RoleListWithAll;
                        $scope.communicationTools = allSetting.CommunicationTools;
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
           
            $scope.getReport = function () {      
                $scope.BlockUI();
                var getReportCriteria = {
                    Username: $scope.searchCriteria.Username,
                    Role: $scope.searchCriteria.role.Id,
                    PageIndex: $scope.searchCriteria.PageIndex,
                    PageSize: $scope.searchCriteria.PageSize                    
                };

                $scope.lastSelectedParams = getReportCriteria;
                $picManagementService.getReport(getReportCriteria)
                    .then(function (data) {
                        $scope.reportOptions.DataList = data.Result;
                        $scope.reportOptions.IsUpdateGrid = true;
                        $scope.unblockUI();
                    }, function () {
                        $scope.unblockUI();
                    });
            };

            $scope.pagingCustomerReport = function (pageIndex, pageSize) {
                $scope.BlockUI();
                $scope.lastSelectedParams.PageIndex = pageIndex;
                $scope.lastSelectedParams.PageSize = pageSize;
                $picManagementService.getReport($scope.lastSelectedParams)
                    .then(function (data) {
                        $scope.reportOptions.DataList = data.Result;
                        $scope.reportOptions.IsUpdateGrid = true;
                        $scope.unblockUI();
                    }, function () {
                        $scope.unblockUI();
                    });
            };

            $scope.insertPic = function () {
                parent.homeManager.CachedData.shouldReloadView = false;
                parent.OpenPopup(
                    window.rootUrl + "PicManagement/OpenInsertPic",
                    viewResources.InsertPIC,
                    480,
                    300,
                    true,
                    false,
                    parent.window,
                    3000,
                    null,
                    $scope.closeInsertView,
                    { my: "center top+200", at: "center top" },
                    $scope.beforeCloseInsertView);
            };

            $scope.beforeCloseInsertView = function () {
                if (parent.homeManager.CachedData.isChangingData && 
                    parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                    var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                    parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                    parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges, popUpController.setFocus);

                    return false;
                }

                return true;
            }

            $scope.closeInsertView = function () {
                $scope.isSetFocus = true;
                //$scope.safeApply();
                if (!parent.homeManager.CachedData.shouldReloadView) {
                    return;
                }

                $scope.$apply();
                $scope.getReport();
            };

            $scope.changePicState = function (item) {
                parent.homeManager.CachedData.shouldReloadView = false;
                var state = !item.IsActive;
                var title = viewResources.Confirmation;
                
                parent.OpenPopup(
                    window.rootUrl + "PicManagement/OpenChangePicState?" + $.param({ PicId: item.PicId, IsActive: state, Username: item.Username }),
                    title,
                    450,
                    160,
                    true,
                    false,
                    parent.window,
                    3000,
                    null,
                    $scope.closeInsertView,
                    { my: "center top+200", at: "center top" },
                    null);
            };

            $scope.editPic = function (item) {
                
                parent.homeManager.CachedData.shouldReloadView = false;
                parent.OpenPopup(
                    window.rootUrl + "PicManagement/OpenEditPic?" + $.param(item),
                    viewResources.EditPIC,
                    480,
                    300,
                    true,
                    false,
                    parent.window,
                    3000,
                    null,
                    $scope.closeInsertView,
                    { my: "center top+200", at: "center top" },
                    $scope.beforeCloseInsertView);
            };

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