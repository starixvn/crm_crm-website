(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').controller('LogsController', logsController);
    logsController.$inject = ['$modal', '$modalInstance', 'titleId'];

    function logsController($modal, $modalInstance, titleId) {
        var vm = this;

        vm.data = [
            { Date: new Date(), EditedBy: 'CCSABC', Totals: 5 },
            { Date: new Date(), EditedBy: 'CCSABC', Totals: 3 },
            { Date: new Date(), EditedBy: 'CCSABC', Totals: 4 },
            { Date: new Date(), EditedBy: 'CCSABC', Totals: 4 }
        ];

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
    }
})(angular);