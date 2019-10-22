(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('EditCommToolSettingController', editCommToolSettingController);
    editCommToolSettingController.$inject = ['$uibModalInstance', '$uiBlockerService', 'commToolSettingFactory', 'allSettings', 'textSettings'];

    function editCommToolSettingController($uibModalInstance, $uiBlockerService, commToolSettingFactory, allSettings, textSettings) {
        let vm = this;

        vm.allSettings = allSettings;
        vm.textSettings = textSettings;
        vm.selectedSubCategoryIds = [];

        function getRemovedAccountLogs(pageIndex, itemsPerPage) {
            $uiBlockerService.blockUI();

            const params = {
                titleId: titleId,
                pageIndex: pageIndex,
                itemsPerPage: itemsPerPage
            };
            commToolSettingFactory.getRemovedAccountLogs(params)
                .then(function (data) {
                    vm.data = data.Result.Items;
                    vm.totalItems = data.Result.TotalItems;
                    $uiBlockerService.unblockUI();
                }, function () {
                    vm.data = [];
                    vm.totalItems = 0;
                    $uiBlockerService.unblockUI();
                });
        }

        function validateForm() {
            let isValid = true;

            if (vm.selectedCategories.length === 0) {
                isValid = false;
                vm.resultMessage = communicationSettingResources.CategoryIsMandatory;
            }

            if (isValid && vm.selectedSubCategories.length === 0) {
                isValid = false;
                vm.resultMessage = communicationSettingResources.SubCategoryIsMandatory;
            }

            if (isValid && vm.selectedCommunications.length === 0) {
                isValid = false;
                vm.resultMessage = communicationSettingResources.CommunicationToolIsMandatory;
            }

            if (isValid) {
                vm.isShowResultMessage = false;
            } else {
                vm.isShowResultMessage = true;
                vm.isSuccess = false;
            }

            return isValid;
        }

        function initialize() {
            vm.communicationDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: allSettings.CommunicationTools
            };
            vm.categoryDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: allSettings.MainCategories
            };
            vm.subCategoryDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: allSettings.SubCategories
            };
        }

        vm.close = function () {
            $uibModalInstance.close();
        };

        vm.categoryChangedHandler = function () {
            vm.subCategoryDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: filterScripts.getSubCategories(
                    vm.selectedCategories,
                    allSettings.SubCategories,
                    vm.selectedSubCategoryIds)
            };
        };

        vm.subCategoryChangedHandler = function () {
            vm.selectedSubCategoryIds = [];
            angular.forEach(vm.selectedSubCategories, function (subCategory) {
                vm.selectedSubCategoryIds.push(subCategory.Id);
            });
        };

        vm.communicationChangeHandler = function () {
        };

        vm.save = function () {
            validateForm();
        };

        initialize();
    }
})(angular);