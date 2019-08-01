(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').controller('LogDetailsController', logDetailsController);
    logDetailsController.$inject = ['$modalInstance', '$uiBlockerService', 'logid'];

    function logDetailsController($modalInstance, $uiBlockerService, logid) {
        var vm = this;

        vm.pagerOption = {
            pageIndex: 0,
            itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        vm.data = [];

        vm.close = function () {
            $modalInstance.close();
        };

        vm.getRemovedAccountLogDetails = function (pageIndex, itemsPerPage) {
            vm.pagerOption.pageIndex = pageIndex;
            vm.pagerOption.itemsPerPage = itemsPerPage;

            getRemovedAccountLogDetails(pageIndex + 1, itemsPerPage);
        };

        function getRemovedAccountLogDetails(pageIndex, itemsPerPage) {
            $uiBlockerService.blockUI();

            var params = {
                logid: logid,
                pageIndex: pageIndex,
                itemsPerPage: itemsPerPage
            };
            logsFactory.getRemovedAccountLogDetails(params)
                .then(function (data) {
                    vm.data = data.Result;
                    $uiBlockerService.unblockUI();
                }, function () {
                    vm.data = [];
                    $uiBlockerService.unblockUI();
                });
        }

        function initialize() {
            getRemovedAccountLogDetails(vm.pagerOption.pageIndex, vm.pagerOption.itemsPerPage);
        }

        initialize();
    }
})(angular);