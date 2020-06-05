(function () {
    "use strict";

    angular.module('pic-management-application').controller('picChangeStateController', ['$picManagementFactory', '$uibModalInstance', 'picItem', function ($picManagementFactory, $uibModalInstance, picItem) {
        let vm = this;
        vm.isDiabled = false;
        vm.messageResultOptions = new MessageResult('', false, false);
        vm.SuccessfullMessage = picItem.IsActive ? "Deactive PIC successfully!" : "Active PIC successfully!";
        vm.ComfirmContent = picItem.IsActive ? "Are you sure you want to deactivate PIC " + picItem.Username + "? The PIC assignment shall be removed." : "Are you sure you want to active PIC " + picItem.Username + "?";

        vm.changeState = function () {
            vm.isDiabled = true;
            $picManagementFactory.changePicState({ PicId: picItem.PicId, IsActive: !picItem.IsActive })
                .then(function (data) {
                    vm.showChangeStateResult(data.Result);
                }, function () {
                    vm.isDiabled = false;
                });
        };

        vm.showChangeStateResult = function (result) {
            if (result.Code !== 0) {
                vm.isDiabled = false;
                vm.messageResultOptions = new MessageResult(result.Message, true, false);
                return;
            }

            vm.messageResultOptions = new MessageResult(vm.SuccessfullMessage, true, true);
            setTimeout(function () {
                vm.close()
            }, 1000);
        };

        vm.close = function () {
            if (vm.messageResultOptions.IsSuccess) {
                vm.messageResultOptions = new MessageResult('', false, false);
                $uibModalInstance.close();
            }
            else {
                $uibModalInstance.close();
            }
        };
    }]);
})();