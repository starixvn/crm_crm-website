(function () {
    "use strict";
    angular.module('changes-log-application').controller('headlineDAController', ['$scope', '$window', '$changesLogFactory', '$uiBlockerService',
        function ($scope, $window, $changesLogFactory, $uiBlockerService) {
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

            vm.getReport = function () {
                $uiBlockerService.blockUI();
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
                $changesLogFactory.getHeadlineDAReport(getReportCriteria)
                    .then(function (data) {
                        vm.errorMessage = '';
                        $scope.headlineDAReportOptions.DataList = data.Result;
                        $scope.headlineDAReportOptions.IsUpdateGrid = true;
                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
            };

            $scope.pagingHeadlineDAReport = function (pageIndex, pageSize) {
                $uiBlockerService.blockUI();
                $scope.lastHeadlineDASelectedParams.PageIndex = pageIndex;
                $scope.lastHeadlineDASelectedParams.PageSize = pageSize;
                $changesLogFactory.getHeadlineDAReport($scope.lastHeadlineDASelectedParams)
                    .then(function (data) {
                        $scope.headlineDAReportOptions.DataList = data.Result;
                        $scope.headlineDAReportOptions.IsUpdateGrid = true;
                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
            };

            vm.initPicker = function () {
                let localCurrentDate = new Date(currentDate);
                $scope.fromDateOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: new Date(initFromDate),
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

            angular.element(document).ready(function () {
                $uiBlockerService.blockUI();
                vm.getActionTypes();
                vm.initPicker();
                $uiBlockerService.unblockUI();
            });
        }]);
})();