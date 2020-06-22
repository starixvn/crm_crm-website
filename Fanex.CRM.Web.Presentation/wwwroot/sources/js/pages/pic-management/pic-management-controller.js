(function () {
    "use strict";
    /* global
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('pic-management-application').controller('picManagementController', ['$scope', '$picManagementFactory', '$window', '$uibModal', '$uiBlockerService', function ($scope, $picManagementFactory, $window, $uibModal, $uiBlockerService) {
        $scope.errorMessage = '';
        $scope.roleDropdownSettings = {
            idMember: 'Id',
            nameMember: 'Value',
            sourceData: [],
            checkedAll: false
        };

        $scope.selectedRoles = [];
        $scope.textSettings = {
            Filter: viewResources.Search,
            NoData: viewResources.NoData,
            NoItemChecked: viewResources.Selected,
            AllItemsSelected: viewResources.All,
            CheckAll: viewResources.All
        };

        $scope.roleChangedHandler = function () {
            $scope.isSelectAllRole = $scope.roleDropdownSettings.sourceData.length > 0
                && $scope.selectedRoles.length === $scope.roleDropdownSettings.sourceData.length;
        };

        $scope.communicationTools = [];

        $scope.reportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };

        $scope.searchCriteria = {
            Username: '',
            Role: '',
            HeadlineName: '',
            PageIndex: 1,
            PageSize: 100
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
            $uiBlockerService.blockUI();
            $scope.isLoadedAllSite = false;
            $picManagementFactory.getAllSetting()
                .then(function (data) {
                    var allSetting = data.Result;
                    let roles = angular.copy(allSetting.RoleListWithInActive);
                    $scope.selectedRoles = roles.filter(role => role.IsActive);
                    let isCheckAllRole = roles.length === $scope.selectedRoles.length;
                    for (let index = 0; index < roles.length; index++) {
                        let selectedIndex = $scope.selectedRoles.findIndex(lang => lang.Id === roles[index].Id);
                        roles[index].checked = isCheckAllRole || selectedIndex >= 0;
                    }

                    $scope.roleDropdownSettings = {
                        idMember: 'Id',
                        nameMember: 'Value',
                        sourceData: roles,
                        checkedAll: isCheckAllRole
                    };

                    $scope.communicationTools = allSetting.CommunicationTools;
                    $scope.completedLoadFilterInfo();
                }, function () {
                    $scope.completedLoadFilterInfo();
                });
        };

        $scope.completedLoadFilterInfo = function () {
            $scope.searchCriteria.role = $scope.roleOptions.DataList[0];
            $uiBlockerService.unblockUI();
        };
           
        $scope.getReport = function () {      
            $uiBlockerService.blockUI();
            $picManagementFactory.getAllSetting()
                .then(function (data) {
                    $scope.communicationTools = data.Result.CommunicationTools;
                    $scope.reportOptions.IsUpdateGrid = true;
                }, function () {
                    $scope.completedLoadFilterInfo();
                });

            if ($scope.selectedRoles.length <= 0) {
                $scope.errorMessage = 'Please choose a role';
                $uiBlockerService.unblockUI();
                return;
            }

            var getReportCriteria = {
                Username: $scope.searchCriteria.Username,
                HeadlineName: $scope.searchCriteria.HeadlineName,
                Role: $scope.selectedRoles.map(role => role.Id).join(','),
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize                    
            };

            $scope.lastSelectedParams = getReportCriteria;
            $picManagementFactory.getReport(getReportCriteria)
                .then(function (data) {
                    $scope.errorMessage = '';
                    $scope.reportOptions.DataList = data.Result;
                    $scope.reportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                }, function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.pagingCustomerReport = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();
            $scope.lastSelectedParams.PageIndex = pageIndex;
            $scope.lastSelectedParams.PageSize = pageSize;
            $picManagementFactory.getReport($scope.lastSelectedParams)
                .then(function (data) {
                    $scope.reportOptions.DataList = data.Result;
                    $scope.reportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                }, function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.insertPic = function () {
            const modalInstance = $uibModal.open({
                template: angular.element('#template-pic-insert').html(),
                controller: 'picInsertController',
                controllerAs: 'picInsertCtrl',
                backdrop: 'static',
                backdropClass: 'pic-insert-modal-backdrop',
                keyboard: false,
                size: 'lg',
                windowClass: 'pic-insert-modal',
                appendTo: $window.parent.$('body')
            });

            modalInstance.result.then(function (isSuccess) {
                if (isSuccess) {
                    $scope.getReport();
                }
            });
        };
        
        $scope.editPic = function (picItem) {
            const modalInstance = $uibModal.open({
                template: angular.element('#template-pic-edit').html(),
                controller: 'picEditController',
                controllerAs: 'picEditCtrl',
                backdrop: 'static',
                backdropClass: 'pic-edit-modal-backdrop',
                keyboard: false,
                size: 'lg',
                windowClass: 'pic-edit-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    picItem
                }
            });

            modalInstance.result.then(function (isSuccess) {
                if (isSuccess) {
                    $scope.getReport();
                }
            });
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
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.$apply();
            $scope.getReport();
        };

        $scope.changePicState = function (picItem, objectState) {
            const modalInstance = $uibModal.open({
                template: angular.element('#template-pic-change-state').html(),
                controller: 'picChangeStateController',
                controllerAs: 'picChangeStateCtrl',
                backdrop: 'static',
                backdropClass: 'pic-change-state-modal-backdrop',
                keyboard: false,
                size: 'lg',
                windowClass: 'pic-change-state-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    picItem
                }
            });

            modalInstance.result.then(function (isSuccess) {
                if (isSuccess) {
                    $scope.getReport();
                }
                else {
                    if (picItem.IsActive) {
                        $(objectState).prop('checked', true);
                    }
                    else {
                        $(objectState).prop('checked', false);
                    }
                }
            });
        };

        $scope.completeSetFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = false;
        };

        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "PicManagement/ExportExcel",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply();
        };

        angular.element(document).ready(function () {
            $scope.initController();

            const style = $('head link[href*="pic.management.css"]').clone();
            $window.parent.$('head').append(style);
        });

        $(document).off('keydown').on('keydown', function (eventObject) {
            if (eventObject.keyCode === 13) // enter
            {
                $scope.getReport();
                eventObject.preventDefault();
                return false;
            }

            if (eventObject.ctrlKey) {
                switch (eventObject.keyCode) {

                    case 83: // s
                        $scope.getReport();
                        eventObject.preventDefault();
                        return false;

                    case 73: // i
                        $scope.insertPic();
                        eventObject.preventDefault();
                        return false;
                }
            }
        });
    }]);
})();