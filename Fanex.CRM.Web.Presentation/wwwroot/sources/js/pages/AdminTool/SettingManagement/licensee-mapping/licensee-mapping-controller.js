(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').controller('LicenseeMappingController', licenseeMappingController);
    licenseeMappingController.$inject = ['$scope', '$window', '$uibModal', '$uibModalInstance', '$uiBlockerService', 'licenseeMappingFactory'];

    function licenseeMappingController($scope, $window, $uibModal, $uibModalInstance, $uiBlockerService, licenseeMappingFactory) {
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
            $($window.parent.document).off('keydown');
            $uibModalInstance.close();
        };

        vm.search = function () {
            getLicenseeMapping();
        };

        vm.saveMapping = function (param) {
            licenseeMappingFactory.updateLicenseeMapping(param)
                .success(function (data) {
                    vm.isSuccessful = data.IsSuccessful;
                    vm.resultMessage = data.IsSuccessful ? 'Update successfully!' : data.Message;
                    vm.isShowResultMessage = true;

                    if (vm.isSuccessful) {
                        setTimeout(function () {
                            vm.isShowResultMessage = false;
                            getLicenseeMapping();
                            $scope.apply();
                        }, sharedScriptManager.cachedData.alertSuccessTime);
                    }
                });
        };

        vm.clearMapping = function (settingId) {
            const modalInstance = $uibModal.open({
                template: angular.element('#delete-licensee-mapping-template').html(),
                controller: 'DeleteLicenseeMappingController',
                controllerAs: 'deleteLicenseeMappingCtrl',
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                appendTo: $window.parent.$('body'),
                resolve: {
                    settingId: settingId
                }
            });
            modalInstance.result.then(function (isSuccessful) {
                if (isSuccessful) {
                    getLicenseeMapping();
                }
            });
        };

        function initialize() {
            getLicensee();
            getLicenseeMapping();
        }

        initialize();

        $($window.parent.document).off('keydown').on('keydown', function (eventObject) {
            if (eventObject.ctrlKey) {
                switch (eventObject.keyCode) {
                    case 83: // s
                        vm.search();
                        $scope.$apply();
                        eventObject.preventDefault();
                        return false;
                }
            }
        });
    }
})(angular);