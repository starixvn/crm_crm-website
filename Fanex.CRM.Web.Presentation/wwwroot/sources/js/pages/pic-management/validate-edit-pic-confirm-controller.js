(function () {
    "use strict";
    angular.module('pic-management-application').controller('validateEditPicConfirmController', ['$uibModalInstance', '$rootScope',
        function ($uibModalInstance, $rootScope) {
            let vm = this;
            vm.message = $rootScope.validateMessage;

            vm.confirm = function () {
                $uibModalInstance.close(true);
            };

            vm.close = function () {
                $uibModalInstance.close(false);
            };
        }]);
})();