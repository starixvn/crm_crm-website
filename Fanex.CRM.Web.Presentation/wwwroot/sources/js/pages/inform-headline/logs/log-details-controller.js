(function (angular) {
    'use strict';

    angular.module('informHeadlineModule').controller('LogDetailsController', logDetailsController);
    logDetailsController.$inject = ['$uibModalInstance', '$uiBlockerService', 'informHeadlineFactory', 'logid'];

    function logDetailsController($uibModalInstance, $uiBlockerService, informHeadlineFactory, logid) {
        let vm = this;

        vm.pagerOption = {
            pageIndex: 0,
            itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        vm.data = [];
        vm.totalItems = 0;

        vm.close = function () {
            $uibModalInstance.close();
        };

        vm.getRemovedAccountLogDetails = function (pageIndex, itemsPerPage) {
            vm.pagerOption.pageIndex = pageIndex;
            vm.pagerOption.itemsPerPage = itemsPerPage;

            getRemovedAccountLogDetails(pageIndex + 1, itemsPerPage);
        };

        function getRemovedAccountLogDetails(pageIndex, itemsPerPage) {
            $uiBlockerService.blockUI();

            const params = {
                logid: logid,
                pageIndex: pageIndex,
                itemsPerPage: itemsPerPage
            };
            informHeadlineFactory.getRemovedAccountLogDetails(params)
                .then(function (data) {
                    vm.data = data.Result.Items;
                    vm.totalItems = data.Result.TotalItems;
                    $uiBlockerService.unblockUI();
                }, function () {
                    vm.data = [];
                    vm.totalItems = 0;
                    $uiBlockerService.unblockUI();
                });
        }

        function initialize() {
            getRemovedAccountLogDetails(vm.pagerOption.pageIndex + 1, vm.pagerOption.itemsPerPage);
        }

        initialize();
    }
})(angular);