(function () {
    "use strict";
    angular.module('changes-log-application').controller('picController', ['$scope', '$changesLogFactory', '$uiBlockerService',
        function ($scope, $changesLogFactory, $uiBlockerService) {
            let vm = this;
            $scope.picFromDateOptions = null;
            $scope.picToDateOptions = null;
            $scope.picReportOptions = {
                DataList: [],
                IsUpdateGrid: false
            };

            vm.searchCriteria = {
                Pic: '',
                EditBy: '',
                PageIndex: 1,
                PageSize: 10
            };

            $scope.lastPicSelectedParams = angular.copy(vm.searchCriteria);
            vm.getReport = function () {
                $uiBlockerService.blockUI();
                $scope.$parent.isError = false;
                $scope.$parent.errorMessage = '';
                let getReportCriteria = {
                    Pic: vm.searchCriteria.Pic,
                    FromDate: moment(vm.selectedFromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    ToDate: moment(vm.selectedToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    EditBy: vm.searchCriteria.EditBy,
                    PageIndex: vm.searchCriteria.PageIndex,
                    PageSize: vm.searchCriteria.PageSize
                };

                $scope.lastPicSelectedParams = getReportCriteria;
                vm.validateAndQuery();
            };

            $scope.pagingPicReport = function (pageIndex, pageSize) {
                $uiBlockerService.blockUI();
                $scope.lastPicSelectedParams.PageIndex = pageIndex;
                $scope.lastPicSelectedParams.PageSize = pageSize;
                vm.validateAndQuery();
            };

            vm.validateAndQuery = function () {
                if ($scope.lastPicSelectedParams.ToDate < $scope.lastPicSelectedParams.FromDate) {
                    $scope.$parent.isError = true;
                    $scope.$parent.errorMessage = viewResources.ToDateCanNotBeforeFromDate;
                    $uiBlockerService.unblockUI();
                    return;
                }

                $changesLogFactory.getPicReport($scope.lastPicSelectedParams)
                    .then(function (data) {
                        $scope.picReportOptions.DataList = data.Result;
                        $scope.picReportOptions.IsUpdateGrid = true;
                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
            };

            vm.initPicker = function () {
                let localCurrentDate = new Date(currentDate);
                vm.selectedFromDate = localCurrentDate;
                $scope.picFromDateOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: new Date(initFromDate),
                    MinDate: minReportDate,
                    MaxDate: localCurrentDate,
                    IsRerenderPicker: false,
                    OnChangeDateEvent: vm.updateFromDatePicker,
                    IsSetDate: false,
                };

                vm.selectedToDate = localCurrentDate;
                $scope.picToDateOptions = {
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
                        $scope.picFromDateOptions.IsRerenderPicker = true;
                    });
                }
                else {
                    vm.selectedFromDate = isOnApply;
                }
            };

            vm.updateToDatePicker = function (isOnApply) {
                if (!isOnApply) {
                    $scope.$apply(function () {
                        $scope.picToDateOptions.IsRerenderPicker = true;
                    });
                }
                else {
                    vm.selectedToDate = isOnApply;
                }
            };

            angular.element(document).ready(function () {
                $uiBlockerService.blockUI();
                vm.initPicker();
                $uiBlockerService.unblockUI();
            });
        }]);
})();