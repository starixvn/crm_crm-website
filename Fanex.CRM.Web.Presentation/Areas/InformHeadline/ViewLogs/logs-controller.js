(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').controller('LogsController', logsController);
    logsController.$inject = ['$modal', '$modalInstance', '$uiBlockerService', 'logsFactory', 'titleId'];

    function logsController($modal, $modalInstance, $uiBlockerService, logsFactory, titleId) {
        var vm = this;

        vm.data = [];

        vm.close = function () {
            $modalInstance.close();
        };

        vm.viewLogDetails = function () {
            $modal.open({
                template: angular.element('#template-log-details').html(),
                controller: 'LogDetailsController as logDetailsCtrl',
                backdrop: 'static',
                size: 'lg',
                windowClass: 'log-details-modal'
            });
        };

        function getRemovedAccountLogs() {
            $uiBlockerService.blockUI();

            var params = { titleId: titleId };
            logsFactory.getRemovedAccountLogs(params)
                .then(function (data) {
                    vm.data = data.Result;
                    $uiBlockerService.unblockUI();
                }, function () {
                    vm.data = [];
                    $uiBlockerService.unblockUI();
                });
        }

        function initialize() {
            getRemovedAccountLogs();
        }

        initialize();
    }
})(angular);