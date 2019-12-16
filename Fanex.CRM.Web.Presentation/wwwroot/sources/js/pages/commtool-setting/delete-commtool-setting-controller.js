(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('DeleteCommToolSettingController', deleteCommToolSettingController);
    deleteCommToolSettingController.$inject = ['$uibModalInstance', '$uiBlockerService', 'commToolSettingFactory', 'communicationSettingId', 'rownum'];

    function deleteCommToolSettingController($uibModalInstance, $uiBlockerService, commToolSettingFactory, communicationSettingId, rownum) {
        let vm = this;
        vm.rownum = rownum;
        vm.isDeleteSuccessfully = false;
        vm.isDisableDelete = false;

        vm.close = function () {
            $uibModalInstance.close(vm.isDeleteSuccessfully);
        };

        vm.delete = function () {
            $uiBlockerService.blockUI();
            commToolSettingFactory.deleteCommToolSetting(communicationSettingId)
                .then(function (data) {
                    $uiBlockerService.unblockUI();
                    vm.isDisableDelete = true;
                    vm.isSuccessful = data.Result.IsSuccessful;
                    vm.resultMessage = data.Result.Message;
                    vm.isShowResultMessage = true;

                    if (vm.isSuccessful) {
                        vm.isDeleteSuccessfully = true;
                        vm.isDisableDelete = true;
                        setTimeout(function () { vm.close(); }, sharedScriptManager.cachedData.alertSuccessTime);
                    }
                });
        };
    }
})(angular);