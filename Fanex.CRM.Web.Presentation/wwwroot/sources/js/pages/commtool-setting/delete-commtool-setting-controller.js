(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('DeleteCommToolSettingController', deleteCommToolSettingController);
    deleteCommToolSettingController.$inject = ['$uibModalInstance', '$uiBlockerService', 'commToolSettingFactory', 'communicationSetting'];

    function deleteCommToolSettingController($uibModalInstance, $uiBlockerService, commToolSettingFactory, communicationSetting) {
        let vm = this;
        vm.communicationSetting = communicationSetting;
        vm.isDeleteSuccessfully = false;
        vm.isDisableDelete = false;

        vm.close = function () {
            $uibModalInstance.close(vm.isDeleteSuccessfully);
        };

        vm.delete = function () {
            $uiBlockerService.blockUI();
            commToolSettingFactory.deleteCommToolSetting(vm.communicationSetting.DefaultCommId)
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