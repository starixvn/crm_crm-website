(function () {
    "use strict";
    angular.module('changes-log-application').controller('headlineDAController', ['$scope', '$changesLogFactory', '$uiBlockerService',
        function ($scope, $changesLogFactory, $uiBlockerService) {
            let vm = this;
            $scope.headlineDAReportOptions = {
                DataList: [],
                IsUpdateGrid: false
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
                EditBy: '',
                PageIndex: 1,
                PageSize: 100
            };

            vm.selectedActions = [];
            vm.actionDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: [],
                checkedAll: false
            };

            vm.getActionTypes = function () {
                $changesLogFactory.getActionTypes()
                    .then(function (data) {
                        vm.actionDropdownSettings = {
                            idMember: 'Id',
                            nameMember: 'Name',
                            sourceData: angular.copy(data.Result),
                            checkedAll: true
                        };
                    }, function () {
                        vm.actionDropdownSettings.sourceData = [];
                    });
            };

            vm.getReport = function (pageIndex, pageSize) {
                $uiBlockerService.blockUI();
                $scope.$parent.errorMessage = '';
                $scope.$parent.isError = false;
                if (vm.selectedActions.length <= 0) {
                    $scope.$parent.isError = true;
                    $scope.$parent.errorMessage = viewResources.PleaseChooseAction;
                    $uiBlockerService.unblockUI();
                    return;
                }

                var getReportCriteria = {
                    UserName: vm.searchCriteria.HeadlineDA,
                    FromDate: moment(vm.selectedFromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    ToDate: moment(vm.selectedToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    ActionTypeIds: vm.selectedActions.map(action => action.Id).join(','),
                    EditBy: vm.searchCriteria.EditBy,
                    PageIndex: pageIndex ? pageIndex : vm.searchCriteria.PageIndex,
                    PageSize: pageSize ? pageSize : vm.searchCriteria.PageSize
                };

                let toDate = new Date(getReportCriteria.ToDate);
                let fromDate = new Date(getReportCriteria.FromDate);
                if (toDate.getTime() - fromDate.getTime() < 0) {
                    $scope.$parent.isError = true;
                    $scope.$parent.errorMessage = viewResources.ToDateCanNotBeforeFromDate;
                    $uiBlockerService.unblockUI();
                    return;
                }

                $scope.lastHeadlineDASelectedParams = getReportCriteria;
                $changesLogFactory.getHeadlineDAReport(getReportCriteria)
                    .then(function (data) {
                        $scope.headlineDAReportOptions.DataList = data.Result;
                        $scope.headlineDAReportOptions.IsUpdateGrid = true;
                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
            };

            $scope.pagingHeadlineDAReport = function (pageIndex, pageSize) {
                $scope.$parent.focused = true;
                vm.getReport(pageIndex, pageSize);
            };

            vm.initPicker = function () {
                let localCurrentDate = new Date(currentDate);
                vm.selectedFromDate = localCurrentDate;
                $scope.fromDateOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: localCurrentDate,
                    MinDate: minReportDate,
                    MaxDate: localCurrentDate,
                    IsRerenderPicker: false,
                    OnChangeDateEvent: vm.updateFromDatePicker,
                    IsSetDate: false,
                };

                vm.selectedToDate = localCurrentDate;
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
                else {
                    vm.selectedFromDate = isOnApply;
                }
            };

            vm.updateToDatePicker = function (isOnApply) {
                if (!isOnApply) {
                    $scope.$apply(function () {
                        $scope.toDateOptions.IsRerenderPicker = true;
                    });
                }
                else {
                    vm.selectedToDate = isOnApply;
                }
            };

            angular.element(document).ready(function () {
                $uiBlockerService.blockUI();
                vm.getActionTypes();
                vm.initPicker();
                $uiBlockerService.unblockUI();
            });
        }]);
})();