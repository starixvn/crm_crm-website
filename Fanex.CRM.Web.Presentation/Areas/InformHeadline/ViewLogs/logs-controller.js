(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').controller('LogsController', logsController);
    logsController.$inject = ['$modalInstance', 'titleId'];

    function logsController($modalInstance, titleId) {
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
    }
})(angular);