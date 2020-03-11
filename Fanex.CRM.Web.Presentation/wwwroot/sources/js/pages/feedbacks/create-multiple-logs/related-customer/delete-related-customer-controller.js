(function (angular) {
    'use strict';

    angular.module('createMultipleLogsModule').controller('DeleteRelatedCustomerController', deleteRelatedCustomerController);
    deleteRelatedCustomerController.$inject = ['$uibModalInstance'];

    function deleteRelatedCustomerController($uibModalInstance) {
        let vm = this;
        vm.deleted = false;

        vm.close = function () {
            $uibModalInstance.close(vm.deleted);
        };

        vm.delete = function () {
            vm.deleted = true;
            vm.close();
        };
    }
})(angular);