(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('CreateCommToolSettingController', createCommToolSettingController);
    createCommToolSettingController.$inject = ['$window', '$scope', '$uibModalInstance', '$uiBlockerService', 'commToolSettingFactory', 'allSettings'];

    function createCommToolSettingController($window, $scope, $uibModalInstance, $uiBlockerService, commToolSettingFactory, allSettings) {
        let vm = this;

        vm.isCreateSuccessfully = false;
        vm.allSettings = allSettings;
        vm.textSettings = {
            Filter: 'Search',
            NoData: 'No data',
            NoItemChecked: 'Please choose',
            AllItemsSelected: 'Any',
            CheckAll: 'Any'
        };
        vm.communicationTextSettings = {
            Filter: 'Search',
            NoData: 'No data',
            NoItemChecked: 'Please choose',
            CheckAll: 'All'
        };

        function getCustomers() {
            let customers = [];
            angular.forEach(vm.relatedCustomers, function (customer) {
                if (customer.Id !== 0) {
                    customers.push({ Id: customer.Id, Name: customer.Name });
                }
            });

            return customers;
        }

        function getCategoryIds() {
            let categoryIds = '';
            if (vm.isSelectAllCategory) {
                categoryIds = '0';
            } else {
                categoryIds = vm.selectedCategories.map(cat => cat.Id).sort().join(',');
            }

            return categoryIds;
        }

        function getSubCategoryIds() {
            let subCategoryIds = '';
            if (vm.isSelectAllSubCategory) {
                subCategoryIds = '0';
            } else {
                subCategoryIds = vm.selectedSubCategories.map(subcat => subcat.Id).sort().join(',');
            }

            return subCategoryIds;
        }

        function getCommunicationIds() {
            return vm.selectedCommunications.map(comm => comm.Id).sort().join(',');
        }

        function validateForm() {
            let isValid = true;

            if (vm.customersInput.length > 0 && !vm.isVerifiedCustomers) {
                isValid = false;
                vm.resultMessage = communicationSettingResources.VerifyListOfUsernameBeforeInsertSetting;
            }

            const customers = getCustomers();
            if (isValid && customers.length === 0) {
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
                vm.isSuccessful = false;
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
            vm.isDiableCreate = false;
            vm.relatedCustomers = [];
            vm.customersInput = '';
            vm.selectedCategories = [];
            vm.selectedSubCategories = [];
            vm.selectedCommunications = [];
            vm.selectedSubCategoryIds = [];
            vm.isSelectAllCategory = false;
            vm.isSelectAllSubCategory = false;
            vm.isVerifiedCustomers = false;
        }

        vm.close = function () {
            $uibModalInstance.close(vm.isCreateSuccessfully);
        };

        vm.categoryChangedHandler = function () {
            vm.categorySelectAllHandler();
            vm.subCategoryDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: filterScripts.getSubCategories(
                    vm.selectedCategories,
                    allSettings.SubCategories,
                    vm.selectedSubCategoryIds)
            };
        };

        vm.categorySelectAllHandler = function () {
            vm.isSelectAllCategory = vm.categoryDropdownSettings.sourceData.length > 0
                && vm.selectedCategories.length === vm.categoryDropdownSettings.sourceData.length;
        };

        vm.subCategoryChangedHandler = function () {
            vm.subCategorySelectAllHandler();
            vm.selectedSubCategoryIds = [];
            angular.forEach(vm.selectedSubCategories, function (subCategory) {
                vm.selectedSubCategoryIds.push(subCategory.Id);
            });
        };

        vm.subCategorySelectAllHandler = function () {
            vm.isSelectAllSubCategory = vm.subCategoryDropdownSettings.sourceData.length
                && vm.selectedSubCategories.length === vm.subCategoryDropdownSettings.sourceData.length;
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

                commToolSettingFactory.verifyUsername(customersInput)
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
            vm.isVerifiedCustomers = vm.customersInput.length > 0 && vm.relatedCustomers.length === 0;
            $scope.$apply();
        };

        vm.pasteCustomerHandler = function (event) {
            var customer = vm.customersInput.trim();
            var items = event.originalEvent.clipboardData.getData('text/plain')
                .replace(/ /g, ',')
                .replace(/\r\n/g, ',')
                .replace(/\n/g, ',')
                .replace(/\t/g, ',')
                .split(',').filter(item => item !== '');

            setTimeout(() => {
                vm.customersInput = (customer.length > 0 ? customer + ',' : '') + items.join(',');
                $scope.$apply();
            }, 100);
        };

        vm.saveAndContinue = function () {
            if (validateForm()) {
                const params = {
                    customers: getCustomers(),
                    categoryIds: getCategoryIds(),
                    subCategoryIds: getSubCategoryIds(),
                    communicationIds: getCommunicationIds()
                };

                commToolSettingFactory.createCommToolSetting(params)
                    .then(function (data) {
                        $uiBlockerService.unblockUI();

                        vm.isSuccessful = data.Result.IsSuccessful;
                        vm.resultMessage = data.Result.Message;
                        vm.isShowResultMessage = true;

                        if (vm.isSuccessful) {
                            vm.isCreateSuccessfully = true;
                            initialize();

                            setTimeout(function () { vm.isShowResultMessage = false; $scope.$apply(); }, sharedScriptManager.cachedData.alertSuccessTime);
                        }
                    });
            }
        };

        vm.save = function () {
            if (validateForm()) {
                const params = {
                    customers: getCustomers(),
                    categoryIds: getCategoryIds(),
                    subCategoryIds: getSubCategoryIds(),
                    communicationIds: getCommunicationIds()
                };

                commToolSettingFactory.createCommToolSetting(params)
                    .then(function (data) {
                        $uiBlockerService.unblockUI();

                        vm.isSuccessful = data.Result.IsSuccessful;
                        vm.resultMessage = data.Result.Message;
                        vm.isShowResultMessage = true;

                        if (vm.isSuccessful) {
                            vm.isCreateSuccessfully = true;
                            vm.isDiableCreate = true;
                            setTimeout(function () { vm.close(); }, sharedScriptManager.cachedData.alertSuccessTime);
                        }
                    });
            }
        };

        initialize();

        $($window.parent.document).off('keydown').on('keydown', function (eventObject) {
            if (eventObject.ctrlKey) {
                switch (eventObject.keyCode) {
                    case 65: // a
                        vm.saveAndContinue();
                        $scope.$apply();
                        eventObject.preventDefault();
                        return false;
                    case 69: // e
                        if (vm.customersInput.trim() !== '') {
                            vm.verifyCustomers();
                        }
                        eventObject.preventDefault();
                        return false;
                    case 83: // s
                        vm.save();
                        $scope.$apply();
                        eventObject.preventDefault();
                        return false;
                }
            }
        });
    }
})(angular);