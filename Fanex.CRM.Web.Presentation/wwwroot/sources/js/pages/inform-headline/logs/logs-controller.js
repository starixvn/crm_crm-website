(function (angular) {
    'use strict';

    angular.module('informHeadlineModule').controller('LogsController', logsController);
    logsController.$inject = ['$modal', '$modalInstance', '$uiBlockerService', 'informHeadlineFactory', 'titleId'];

    function logsController($modal, $modalInstance, $uiBlockerService, informHeadlineFactory, titleId) {
        let vm = this;

        vm.pagerOption = {
            pageIndex: 0,
            itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        vm.data = [];
        vm.totalItems = 0;

        vm.close = function () {
            $modalInstance.close();
        };

        vm.viewLogDetails = function (logid) {
            $modal.open({
                template: angular.element('#template-log-details').html(),
                controller: 'LogDetailsController as logDetailsCtrl',
                backdrop: 'static',
                size: 'lg',
                windowClass: 'log-details-modal',
                resolve: {
                    logid: function () {
                        return logid;
                    }
                }
            });
        };

        vm.getRemovedAccountLogs = function (pageIndex, itemsPerPage) {
            vm.pagerOption.pageIndex = pageIndex;
            vm.pagerOption.itemsPerPage = itemsPerPage;

            getRemovedAccountLogs(pageIndex + 1, itemsPerPage);
        };

        function getRemovedAccountLogs(pageIndex, itemsPerPage) {
            $uiBlockerService.blockUI();

            const params = {
                titleId: titleId,
                pageIndex: pageIndex,
                itemsPerPage: itemsPerPage
            };
            informHeadlineFactory.getRemovedAccountLogs(params)
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
            getRemovedAccountLogs(vm.pagerOption.pageIndex + 1, vm.pagerOption.itemsPerPage);
        }

        initialize();
    }
})(angular);