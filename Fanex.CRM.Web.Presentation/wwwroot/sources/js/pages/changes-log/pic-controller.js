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
                PageSize: 100
            };

            $scope.lastPicSelectedParams = angular.copy(vm.searchCriteria);
            vm.getReport = function () {
                $uiBlockerService.blockUI();
                $scope.$parent.isError = false;
                $scope.$parent.errorMessage = '';
                let getReportCriteria = {
                    PicName: vm.searchCriteria.Pic,
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
                let yesterday = new Date(currentDate);
                yesterday = yesterday.setDate(yesterday.getDate() - 1);
                let minDate = new Date(currentDate);
                minDate = minDate.setMonth(minDate.getMonth() - 1);
                vm.selectedFromDate = localCurrentDate;
                $scope.picFromDateOptions = {
                    CurrentDate: yesterday,
                    InitDate: yesterday,
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
                    MinDate: yesterday,
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
                    $scope.picToDateOptions.MinDate = isOnApply;
                    let localCurrentDate = new Date(currentDate);
                    let tempMaxToDate = new Date(isOnApply);
                    let toDate = new Date(vm.selectedToDate);
                    tempMaxToDate.setMonth(tempMaxToDate.getMonth() + 1);

                    if (tempMaxToDate - localCurrentDate > 0) {
                        tempMaxToDate = localCurrentDate;
                    }

                    if (toDate - tempMaxToDate > 0) {
                        vm.selectedToDate = tempMaxToDate;
                    }

                    $scope.picToDateOptions.MaxDate = tempMaxToDate;                    
                    $scope.picToDateOptions.IsRerenderPicker = true;
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