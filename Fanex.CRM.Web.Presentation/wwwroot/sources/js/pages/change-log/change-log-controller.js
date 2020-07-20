(function () {
    "use strict";
    angular.module('change-log-application').controller('changeLogController', ['$scope', '$window',
        function ($scope, $window) {
            let vm = this;
            vm.errorMessage = '';
            $scope.isShowFilter = true;
            vm.textSettings = {
                Filter: viewResources.Search,
                NoData: viewResources.NoData,
                NoItemChecked: viewResources.Selected,
                AllItemsSelected: viewResources.All,
                CheckAll: viewResources.All
            };

            vm.searchCriteria = {
                Username: '',
                PIC: '',
                FromDate: new Date(initFromDate),
                ToDate: new Date(currentDate),
                Action: '',
                EditBy: '',
                PageIndex: 1,
                PageSize: 100
            };

            vm.fromDateOptions = {
                CurrentDate: new Date(currentDate),
                InitDate: new Date(initFromDate),
                MinDate: new Date(minReportDate),
                MaxDate: new Date(currentDate),
                IsRerenderPicker: false,
                OnChangeDateEvent: vm.updateFromDate,
                IsSetDate: false
            };

            vm.updateFromDatePicker = function (isOnApply) {
                if (!isOnApply) {
                    $scope.$apply(function () {
                        vm.fromDateOptions.IsRerenderPicker = true;
                    });
                }
            };

            vm.lastSelectedParams = angular.copy(vm.searchCriteria);

            vm.initController = function () {
                //vm.initPicker();
                //vm.getAllSetting();
                //$scope.$apply();
            };

            vm.initPicker = function () {
                $scope.roleOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedRole,
                    AllText: viewResources.All
                };
            };

            //$scope.getAllSetting = function () {
            //    $uiBlockerService.blockUI();
            //    $scope.isLoadedAllSite = false;
            //    $picManagementFactory.getAllSetting()
            //        .then(function (data) {
            //            var allSetting = data.Result;
            //            let roles = angular.copy(allSetting.RoleListWithInActive);
            //            $scope.selectedRoles = roles.filter(role => role.IsActive);
            //            let isCheckAllRole = roles.length === $scope.selectedRoles.length;
            //            for (let index = 0; index < roles.length; index++) {
            //                let selectedIndex = $scope.selectedRoles.findIndex(lang => lang.Id === roles[index].Id);
            //                roles[index].checked = isCheckAllRole || selectedIndex >= 0;
            //            }

            //            $scope.roleDropdownSettings = {
            //                idMember: 'Id',
            //                nameMember: 'Value',
            //                sourceData: roles,
            //                checkedAll: isCheckAllRole
            //            };

            //            $scope.communicationTools = allSetting.CommunicationTools;
            //            $scope.completedLoadFilterInfo();
            //        }, function () {
            //            $scope.completedLoadFilterInfo();
            //        });
            //};

            $scope.completedLoadFilterInfo = function () {
                $scope.searchCriteria.role = $scope.roleOptions.DataList[0];
                $uiBlockerService.unblockUI();
            };

            //$scope.getReport = function () {
            //    $uiBlockerService.blockUI();
            //    $picManagementFactory.getAllSetting()
            //        .then(function (data) {
            //            $scope.communicationTools = data.Result.CommunicationTools;
            //            $scope.reportOptions.IsUpdateGrid = true;
            //        }, function () {
            //            $scope.completedLoadFilterInfo();
            //        });

            //    if ($scope.selectedRoles.length <= 0) {
            //        $scope.errorMessage = 'Please choose a role';
            //        $uiBlockerService.unblockUI();
            //        return;
            //    }

            //    var getReportCriteria = {
            //        Username: $scope.searchCriteria.Username,
            //        HeadlineName: $scope.searchCriteria.HeadlineName,
            //        Role: $scope.selectedRoles.map(role => role.Id).join(','),
            //        PageIndex: $scope.searchCriteria.PageIndex,
            //        PageSize: $scope.searchCriteria.PageSize
            //    };

            //    $scope.lastSelectedParams = getReportCriteria;
            //    $picManagementFactory.getReport(getReportCriteria)
            //        .then(function (data) {
            //            $scope.errorMessage = '';
            //            $scope.reportOptions.DataList = data.Result;
            //            $scope.reportOptions.IsUpdateGrid = true;
            //            $uiBlockerService.unblockUI();
            //        }, function () {
            //            $uiBlockerService.unblockUI();
            //        });
            //};

            //$scope.pagingCustomerReport = function (pageIndex, pageSize) {
            //    $uiBlockerService.blockUI();
            //    $scope.lastSelectedParams.PageIndex = pageIndex;
            //    $scope.lastSelectedParams.PageSize = pageSize;
            //    $picManagementFactory.getReport($scope.lastSelectedParams)
            //        .then(function (data) {
            //            $scope.reportOptions.DataList = data.Result;
            //            $scope.reportOptions.IsUpdateGrid = true;
            //            $uiBlockerService.unblockUI();
            //        }, function () {
            //            $uiBlockerService.unblockUI();
            //        });
            //};

            angular.element(document).ready(function () {
                vm.initController();
                $("#alertboxerr").addClass("alertboxerr");
                const style = $('head link[href*="change.log.css"]').clone();
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