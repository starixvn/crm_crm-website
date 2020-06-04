(function () {
    "use strict";
    angular.module('pic-management-application').controller('picChangeConfirmController', ['$uibModalInstance', 
        function ($uibModalInstance) {
            let vm = this;

            vm.changeConfirm = function () {
                $uibModalInstance.close(true);
            };

            vm.close = function () {
                $uibModalInstance.close(false);
            };
        }]);
})();