(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').controller('LogDetailsController', logDetailsController);
    logDetailsController.$inject = ['$modalInstance'];

    function logDetailsController($modalInstance) {
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