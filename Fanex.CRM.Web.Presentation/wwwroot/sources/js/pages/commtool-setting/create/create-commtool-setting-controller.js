(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('CreateCommToolSettingController', createCommToolSettingController);
    createCommToolSettingController.$inject = ['$scope', '$uibModalInstance', '$uiBlockerService', 'commToolSettingFactory', 'allSettings', 'textSettings'];

    function createCommToolSettingController($scope, $uibModalInstance, $uiBlockerService, commToolSettingFactory, allSettings, textSettings) {
        let vm = this;

        vm.allSettings = allSettings;
        vm.textSettings = textSettings;
        vm.selectedSubCategoryIds = [];
        vm.relatedCustomers = [];
        vm.customersInput = '';
        vm.isVerifiedCustomers = false;

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

        function getRelatedCustomerName() {
            let relatedCustomerNames = [];
            angular.forEach(vm.relatedCustomers, function (customer) {
                if (customer.Id !== 0) {
                    relatedCustomerNames.push(customer.Name);
                }
            });

            return relatedCustomerNames.join(',');
        }

        function validateForm() {
            let isValid = true;
            const customers = getRelatedCustomerName();
            if (customers.length === 0) {
                isValid = false;
                vm.resultMessage = communicationSettingResources.ListOfCustomersAreMandatory;
            }

            if (isValid && vm.selectedCategories.length === 0) {
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

        vm.verifyCustomers = function () {
            const isValidCustomer = function (customers) {
                return customers.split(',').filter(function (value) {
                    return value.length > 50;
                }).length === 0;
            };
            const isDuplicatedCustomer = function (source, target) {
                return source.Id === target.Id && source.Name === target.Name;
            };
            const combineCustomers = function (currentCustomers, newCustomers) {
                let customers = (currentCustomers || []).concat(newCustomers);
                for (let i = 0; i < currentCustomers.length; ++i) {
                    for (let j = i + 1; j < customers.length; ++j) {
                        if (isDuplicatedCustomer(customers[i], customers[j])) {
                            customers.splice(j--, 1);
                        }
                    }
                }

                return customers;
            };

            const customersInput = vm.customersInput.trim().replace(/,\s+/g, ',').replace(/^,+|,+$/g, '').replace(/,{2,}/g, ',');
            if (isValidCustomer(customersInput)) {
                vm.isShowResultMessage = false;
                vm.isVerifiedCustomers = true;
                $uiBlockerService.blockUI();

                commToolSettingFactory.verifyCustomers(customersInput)
                    .then(function (data) {
                        let customers = [];
                        angular.forEach(data.Result, function (customer) {
                            customers.push({ Id: customer.Id, Name: customer.Name });
                        });

                        vm.relatedCustomers = combineCustomers(vm.relatedCustomers, customers);
                        vm.customersInput = '';

                        $uiBlockerService.unblockUI();
                    }, function () {
                        $uiBlockerService.unblockUI();
                    });
            } else {
                vm.isShowResultMessage = true;
                vm.resultMessage = communicationSettingResources.ErrorRelatedCustomerNameLength;
            }
        };

        vm.deleteCustomer = function (customerId) {
            vm.relatedCustomers = vm.relatedCustomers.filter(function (value) {
                return value.Id !== customerId;
            });
            $scope.$apply();
        };

        vm.saveAndContinue = function () {
            validateForm();
        };

        vm.save = function () {
            validateForm();
        };

        initialize();
    }
})(angular);