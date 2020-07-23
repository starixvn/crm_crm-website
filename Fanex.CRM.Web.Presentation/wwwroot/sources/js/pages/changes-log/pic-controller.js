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
                let getReportCriteria = {
                    Pic: vm.searchCriteria.Pic,
                    FromDate: moment($scope.picFromDateOptions.CurrentDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    ToDate: moment($scope.picToDateOptions.CurrentDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                    EditBy: vm.searchCriteria.EditBy,
                    PageIndex: vm.searchCriteria.PageIndex,
                    PageSize: vm.searchCriteria.PageSize
                };

                $scope.lastPicSelectedParams = getReportCriteria;
                $changesLogFactory.getPicReport(getReportCriteria)
                    .then(function (data) {
                        vm.errorMessage = '';
                        $scope.picReportOptions.DataList = data.Result;
                        $scope.picReportOptions.IsUpdateGrid = true;
                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
            };

            $scope.pagingPicReport = function (pageIndex, pageSize) {
                $uiBlockerService.blockUI();
                $scope.lastPicSelectedParams.PageIndex = pageIndex;
                $scope.lastPicSelectedParams.PageSize = pageSize;
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
                $scope.picFromDateOptions = {
                    CurrentDate: localCurrentDate,
                    InitDate: new Date(initFromDate),
                    MinDate: minReportDate,
                    MaxDate: localCurrentDate,
                    IsRerenderPicker: false,
                    OnChangeDateEvent: vm.updateFromDatePicker,
                    IsSetDate: false,
                };

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
            };

            vm.updateToDatePicker = function (isOnApply) {
                if (!isOnApply) {
                    $scope.$apply(function () {
                        $scope.picToDateOptions.IsRerenderPicker = true;
                    });
                }
            };

            angular.element(document).ready(function () {
                $uiBlockerService.blockUI();
                vm.initPicker();
                $uiBlockerService.unblockUI();
            });
        }]);
})();