(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('pic-management-application').controller('picManagementController', ['$scope', '$picManagementFactory', '$window', '$uibModal', function ($scope, $picManagementFactory, $window, $uibModal) {
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
            Role: '',
            HeadlineName: '',
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
            $picManagementFactory.getAllSetting()
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
                HeadlineName: $scope.searchCriteria.HeadlineName,
                Role: $scope.searchCriteria.role.Id === 0 ? '' : $scope.searchCriteria.role.Value,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize                    
            };

            $scope.lastSelectedParams = getReportCriteria;
            $picManagementFactory.getReport(getReportCriteria)
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
            $picManagementFactory.getReport($scope.lastSelectedParams)
                .then(function (data) {
                    $scope.reportOptions.DataList = data.Result;
                    $scope.reportOptions.IsUpdateGrid = true;
                    $scope.unblockUI();
                }, function () {
                    $scope.unblockUI();
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

        $scope.changePicState = function (picItem) {
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

            modalInstance.result.then(function () {
                $scope.getReport();
            });
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

        $($window.parent.document).off('keydown').on('keydown', function (eventObject) {
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