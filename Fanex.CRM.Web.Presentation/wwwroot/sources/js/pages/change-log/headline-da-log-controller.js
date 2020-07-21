(function () {
    "use strict";
    angular.module('change-log-application').controller('headlineDALogController', ['$scope', '$uiBlockerService', function ($scope, $uiBlockerService) {
        let vm = this;
        vm.textSettings = {
            Filter: viewResources.Search,
            NoData: viewResources.NoData,
            NoItemChecked: viewResources.Selected,
            AllItemsSelected: viewResources.All,
            CheckAll: viewResources.All
        };

        vm.searchCriteria = {
            HeadlineDA: '',
            EditBy: '',
            PageIndex: 1,
            PageSize: 100
        };

        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        vm.selectedActions = [];
        vm.actionDropdownSettings = {
            idMember: 'Id',
            nameMember: 'Value',
            sourceData: [],
            checkedAll: false
        };

        vm.actionChangedHandler = function () {
            vm.isSelectAllAction = vm.actionDropdownSettings.sourceData.length > 0
                && vm.selectedActions.length === vm.actionDropdownSettings.sourceData.length;
        };

        vm.lastSelectedParams = angular.copy(vm.searchCriteria);

        vm.initController = function () {
            vm.initPicker();
            //vm.getAllSetting();
            //$scope.$apply();
        };

        vm.initPicker = function () {
            currentDate = new Date(currentDate);
            initFromDate = new Date(initFromDate);
            let beginYearDropdown = new Date(initFromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, initFromDate.getMonth(), initFromDate.getDate());
            $scope.fromMonthOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeEvent: vm.updateFromMonth,
                SelectedYear: initFromDate.getFullYear(),
                SelectedMonth: initFromDate.getMonth() + 1,
                IsOutScopePicker: true
            };

            $scope.fromMonthOptions.IsRerenderPicker = true;
            $scope.$apply();
            $scope.toMonthOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeEvent: vm.updateToMonth,
                SelectedYear: currentDate.getFullYear(),
                SelectedMonth: currentDate.getMonth() + 1
            };

            $scope.toMonthOptions.IsRerenderPicker = true;
            $scope.$apply();
            $scope.fromDateOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MinDate: minReportDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: vm.updateFromDatePicker,
                IsSetDate: false,
            };

            $scope.toDateOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MinDate: minReportDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: vm.updateToDatePicker,
                IsSetDate: false
            };

            vm.updateFromDatePicker();
            vm.updateToDatePicker();
        };

        vm.updateFromDatePicker = function (isOnApply) {
            if (!isOnApply) {
                $scope.$apply(function () {
                    $scope.fromDateOptions.IsRerenderPicker = true;
                });
            }
        };

        vm.updateToDatePicker = function (isOnApply) {
            if (!isOnApply) {
                $scope.$apply(function () {
                    $scope.toDateOptions.IsRerenderPicker = true;
                });
            }
        };

        //vm.getAllSetting = function () {
        //    $uiBlockerService.blockUI();
        //    vm.isLoadedAllSite = false;
        //    $picManagementFactory.getAllSetting()
        //        .then(function (data) {
        //            var allSetting = data.Result;
        //            let roles = angular.copy(allSetting.RoleListWithInActive);
        //            vm.selectedRoles = roles.filter(role => role.IsActive);
        //            let isCheckAllRole = roles.length === vm.selectedRoles.length;
        //            for (let index = 0; index < roles.length; index++) {
        //                let selectedIndex = vm.selectedRoles.findIndex(lang => lang.Id === roles[index].Id);
        //                roles[index].checked = isCheckAllRole || selectedIndex >= 0;
        //            }

        //            vm.roleDropdownSettings = {
        //                idMember: 'Id',
        //                nameMember: 'Value',
        //                sourceData: roles,
        //                checkedAll: isCheckAllRole
        //            };

        //            vm.communicationTools = allSetting.CommunicationTools;
        //            vm.completedLoadFilterInfo();
        //        }, function () {
        //            vm.completedLoadFilterInfo();
        //        });
        //};

        //vm.completedLoadFilterInfo = function () {
        //    vm.searchCriteria.role = vm.roleOptions.DataList[0];
        //    $uiBlockerService.unblockUI();
        //};

        vm.getReport = function () {
            $uiBlockerService.blockUI();
            //$picManagementFactory.getAllSetting()
            //    .then(function (data) {
            //        vm.communicationTools = data.Result.CommunicationTools;
            //        vm.reportOptions.IsUpdateGrid = true;
            //    }, function () {
            //        vm.completedLoadFilterInfo();
            //    });

            //if (vm.selectedActions.length <= 0) {
            //    vm.errorMessage = 'Please choose a action';
            //    $uiBlockerService.unblockUI();
            //    return;
            //}

            var getReportCriteria = {
                HeadlineDA: vm.searchCriteria.HeadlineDA,
                FromDate: vm.fromDateOptions.CurrentDate,
                ToDate: vm.toDateOptions.CurrentDate,
                Action: vm.selectedActions.map(action => action.Id).join(','),
                EditBy: vm.searchCriteria.EditBy,
                PageIndex: vm.searchCriteria.PageIndex,
                PageSize: vm.searchCriteria.PageSize
            };

            vm.lastSelectedParams = getReportCriteria;
            $changeLogFactory.getReport(getReportCriteria)
                .then(function (data) {
                    vm.errorMessage = '';
                    vm.reportOptions.DataList = data.Result;
                    vm.reportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                }, function () {
                    $uiBlockerService.unblockUI();
                });
            $uiBlockerService.unblockUI();
        };

        //vm.pagingCustomerReport = function (pageIndex, pageSize) {
        //    $uiBlockerService.blockUI();
        //    vm.lastSelectedParams.PageIndex = pageIndex;
        //    vm.lastSelectedParams.PageSize = pageSize;
        //    $picManagementFactory.getReport(vm.lastSelectedParams)
        //        .then(function (data) {
        //            vm.reportOptions.DataList = data.Result;
        //            vm.reportOptions.IsUpdateGrid = true;
        //            $uiBlockerService.unblockUI();
        //        }, function () {
        //            $uiBlockerService.unblockUI();
        //        });
        //};

        angular.element(document).ready(function () {
            vm.initController();
            $("#alertboxerr").addClass("alertboxerr");
        });

        $(document).off('keydown').on('keydown', function (eventObject) {
            if (eventObject.keyCode === 13) // enter
            {
                vm.getReport();
                eventObject.preventDefault();
                return false;
            }

            if (eventObject.ctrlKey) {
                switch (eventObject.keyCode) {
                    case 83: // s
                        vm.getReport();
                        eventObject.preventDefault();
                        return false;

                    case 73: // i
                        vm.insertPic();
                        eventObject.preventDefault();
                        return false;
                }
            }
        });
    }]);
})();