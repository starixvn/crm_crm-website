(function () {
    "use strict";
    angular.module('change-log-application').controller('changeLogController', ['$scope', '$window', '$changeLogFactory', '$uiBlockerService',
        function ($scope, $window, $changeLogFactory, $uiBlockerService) {
            let vm = this;
            $scope.isShowFilter = true;
            vm.errorMessage = '';
            vm.isPicReport = false;
            vm.isShowActionFilter = true;
            $scope.headlineDAReportOptions = {
                DataList: [],
                IsHeadlineDAUpdateGrid: false
            };

            $scope.picReportOptions = {
                DataList: [],
                IsPICUpdateGrid: false
            };

            $scope.fromDateOptions = null;
            $scope.toDateOptions = null;
            vm.textSettings = {
                Filter: viewResources.Search,
                NoData: viewResources.NoData,
                NoItemChecked: viewResources.Selected,
                AllItemsSelected: viewResources.All,
                CheckAll: viewResources.All
            };

            vm.searchCriteria = {
                HeadlineDA: '',
                PIC: '',
                EditBy: '',
                PageIndex: 1,
                PageSize: 100
            };

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

            $scope.lastHeadlineDASelectedParams = angular.copy(vm.searchCriteria);
            $scope.lastPicSelectedParams = angular.copy(vm.searchCriteria);

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
                if (vm.isPicReport) {
                    vm.getPicReport();
                }
                else {
                    vm.getHeadlineDAReport();
                }
            };

            vm.getHeadlineDAReport = function () {
                $uiBlockerService.blockUI();
                //$picManagementFactory.getAllSetting()
                //    .then(function (data) {
                //        vm.communicationTools = data.Result.CommunicationTools;
                //        vm.headlineDAReportOptions.IsUpdateGrid = true;
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
                    FromDate: moment($scope.fromDateOptions.CurrentDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    ToDate: moment($scope.toDateOptions.CurrentDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    Action: vm.selectedActions.map(action => action.Id).join(','),
                    EditBy: vm.searchCriteria.EditBy,
                    PageIndex: vm.searchCriteria.PageIndex,
                    PageSize: vm.searchCriteria.PageSize
                };

                $scope.lastHeadlineDASelectedParams = getReportCriteria;
                $changeLogFactory.getHeadlineDAReport(getReportCriteria)
                    .then(function (data) {
                        vm.errorMessage = '';
                        $scope.headlineDAReportOptions.DataList = data.Result;
                        $scope.headlineDAReportOptions.IsHeadlineDAUpdateGrid = true;
                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
                $uiBlockerService.unblockUI();
            };

            vm.getPicReport = function () {
                $uiBlockerService.blockUI();
                let getReportCriteria = {
                    Pic: vm.searchCriteria.PIC,
                    FromDate: moment($scope.fromDateOptions.CurrentDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    ToDate: moment($scope.toDateOptions.CurrentDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    EditBy: vm.searchCriteria.EditBy,
                    PageIndex: vm.searchCriteria.PageIndex,
                    PageSize: vm.searchCriteria.PageSize
                };

                $scope.lastPicSelectedParams = getReportCriteria;
                $changeLogFactory.getPicReport(getReportCriteria)
                    .then(function (data) {
                        vm.errorMessage = '';
                        $scope.picReportOptions.DataList = data.Result;
                        $scope.picReportOptions.IsPICUpdateGrid = true;
                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
                $uiBlockerService.unblockUI();
            };

        //vm.pagingCustomerReport = function (pageIndex, pageSize) {
        //    $uiBlockerService.blockUI();
        //    $scope.lastSelectedParams.PageIndex = pageIndex;
        //    $scope.lastSelectedParams.PageSize = pageSize;
        //    $picManagementFactory.getReport($scope.lastSelectedParams)
        //        .then(function (data) {
        //            vm.headlineDAReportOptions.DataList = data.Result;
        //            vm.headlineDAReportOptions.IsUpdateGrid = true;
        //            $uiBlockerService.unblockUI();
        //        }, function () {
        //            $uiBlockerService.unblockUI();
        //        });
        //};
            vm.initPicker = function () {
                let localCurrentDate = new Date(currentDate);
                let localInitFromDate = new Date(initFromDate);
                let beginYearDropdown = new Date(localInitFromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, localInitFromDate.getMonth(), localInitFromDate.getDate());
                $scope.fromMonthOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: localInitFromDate,
                    MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                    MaxDate: localCurrentDate,
                    IsRerenderPicker: false,
                    OnChangeEvent: vm.updateFromMonth,
                    SelectedYear: localInitFromDate.getFullYear(),
                    SelectedMonth: localInitFromDate.getMonth() + 1,
                    IsOutScopePicker: true
                };

                $scope.fromMonthOptions.IsRerenderPicker = true;
                $scope.$apply();
                $scope.toMonthOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: localCurrentDate,
                    MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                    MaxDate: localCurrentDate,
                    IsRerenderPicker: false,
                    OnChangeEvent: vm.updateToMonth,
                    SelectedYear: localCurrentDate.getFullYear(),
                    SelectedMonth: localCurrentDate.getMonth() + 1
                };

                $scope.toMonthOptions.IsRerenderPicker = true;
                $scope.$apply();
                $scope.fromDateOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: localInitFromDate,
                    MinDate: minReportDate,
                    MaxDate: localCurrentDate,
                    IsRerenderPicker: false,
                    OnChangeDateEvent: vm.updateFromDatePicker,
                    IsSetDate: false,
                };

                $scope.toDateOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: localCurrentDate,
                    MinDate: minReportDate,
                    MaxDate: localCurrentDate,
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

            vm.changeHeadlineDATab = function () {
                vm.isPicReport = false;
                vm.isShowActionFilter = true;
            };

            vm.changePICTab = function () {
                vm.isPicReport = true;
                vm.isShowActionFilter = false;
            };

            angular.element(document).ready(function () {
                vm.initPicker();
                $("#alertboxerr").addClass("alertboxerr");
            });

            //$(document).off('keydown').on('keydown', function (eventObject) {
            //    if (eventObject.keyCode === 13) // enter
            //    {
            //        vm.getReport();
            //        eventObject.preventDefault();
            //        return false;
            //    }

            //    if (eventObject.ctrlKey) {
            //        switch (eventObject.keyCode) {
            //            case 83: // s
            //                vm.getReport();
            //                eventObject.preventDefault();
            //                return false;

            //            case 73: // i
            //                vm.insertPic();
            //                eventObject.preventDefault();
            //                return false;
            //        }
            //    }
            //});
        }]);
})();