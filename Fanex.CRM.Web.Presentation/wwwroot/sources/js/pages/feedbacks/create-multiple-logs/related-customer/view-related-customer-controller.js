(function (angular) {
    'use strict';

    angular.module('createMultipleLogsModule').controller('ViewRelatedCustomerController', viewRelatedCustomerController);
    viewRelatedCustomerController.$inject = ['$uibModalInstance', 'relatedCustomers'];

    function viewRelatedCustomerController($uibModalInstance, relatedCustomers) {
        let vm = this;
        vm.relatedCustomers = relatedCustomers;

        vm.close = function () {
            $uibModalInstance.close();
        };
    }
})(angular);