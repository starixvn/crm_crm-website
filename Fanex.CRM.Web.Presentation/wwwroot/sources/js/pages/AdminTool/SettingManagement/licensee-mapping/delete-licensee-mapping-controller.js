(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').controller('DeleteLicenseeMappingController', deleteLicenseeMappingController);
    deleteLicenseeMappingController.$inject = ['$uibModalInstance', 'licenseeMappingFactory', 'settingId'];

    function deleteLicenseeMappingController($uibModalInstance, licenseeMappingFactory, settingId) {
        let vm = this;
        vm.isDeleteSuccessfully = false;

        vm.close = function () {
            $uibModalInstance.close(vm.isDeleteSuccessfully);
        };

        vm.delete = function () {
            const param = {
                settingId: settingId,
                sourceMapping: '',
                licenseeManager: ''
            };

            licenseeMappingFactory.updateLicenseeMapping(param)
                .success(function (data) {
                    vm.isSuccessful = data.IsSuccessful;
                    vm.resultMessage = data.IsSuccessful ? 'Update successfully!' : data.Message;
                    vm.isShowResultMessage = true;

                    if (vm.isSuccessful) {
                        vm.isDeleteSuccessfully = true;
                        setTimeout(function () { vm.close(); }, sharedScriptManager.cachedData.alertSuccessTime);
                    }
                });
        };
    }
})(angular);