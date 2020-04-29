(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').controller('LicenseeMappingController', licenseeMappingController);
    licenseeMappingController.$inject = ['$uibModal', '$uibModalInstance', '$uiBlockerService', 'licenseeMappingFactory'];

    function licenseeMappingController($uibModal, $uibModalInstance, $uiBlockerService, licenseeMappingFactory) {
        let vm = this;
        vm.keyword = '';
        vm.licensees = [];
        vm.licenseeMappings = [];

        function getLicensee() {
            licenseeMappingFactory.getLicensee()
                .success(function (data) {
                    vm.licensees = data.map(item => { return { id: item, text: item }; });
                });
        }

        function getLicenseeMapping() {
            $uiBlockerService.blockUI();

            const params = {
                keyword: vm.keyword.trim()
            };
            licenseeMappingFactory.getLicenseeMapping(params)
                .success(function (data) {
                    vm.licenseeMappings = data;
                    $uiBlockerService.unblockUI();
                });
        }

        vm.close = function () {
            $uibModalInstance.close();
        };

        vm.search = function () {
            getLicenseeMapping();
        };

        vm.save = function (param) {
            licenseeMappingFactory.updateLicenseeMapping(param)
                .success(function (data) {
                    console.log(data);
                });
        };

        function initialize() {
            getLicensee();
            getLicenseeMapping();
        }

        initialize();
    }
})(angular);