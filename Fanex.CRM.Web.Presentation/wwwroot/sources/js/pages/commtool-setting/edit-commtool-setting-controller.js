(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('EditCommToolSettingController', editCommToolSettingController);
    editCommToolSettingController.$inject = ['$window', '$scope', '$uibModalInstance', '$uiBlockerService', 'commToolSettingFactory', 'allSettings', 'communicationSettingId'];

    function editCommToolSettingController($window, $scope, $uibModalInstance, $uiBlockerService, commToolSettingFactory, allSettings, communicationSettingId) {
        let vm = this;

        vm.isEditSuccessfully = false;
        vm.isDisableEdit = false;
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

        function getCommToolSettingById() {
            $uiBlockerService.blockUI();
            commToolSettingFactory.getCommToolSettingById(communicationSettingId)
                .then(function (data) {
                    $uiBlockerService.unblockUI();
                    const setting = data.Result;
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
                    vm.selectedCategories = [];
                    vm.selectedSubCategories = [];
                    vm.selectedCommunications = [];
                    vm.selectedSubCategoryIds = [];
                    vm.isSelectAllCategory = false;
                    vm.isSelectAllSubCategory = false;

                    vm.username = setting.Username;

                    if (setting.Category === '0') {
                        vm.isSelectAllCategory = true;
                    }

                    const categoryIds = setting.Category.split(',');
                    let totalItems = vm.categoryDropdownSettings.sourceData.length;
                    for (let i = 0; i < totalItems; i++) {
                        const index = categoryIds.findIndex(x => x === vm.categoryDropdownSettings.sourceData[i].Id.toString());
                        if (setting.Category === '0' || index >= 0) {
                            vm.categoryDropdownSettings.sourceData[i].checked = true;
                            vm.selectedCategories.push(vm.categoryDropdownSettings.sourceData[i]);
                        }
                    }

                    if (setting.SubCategory === '0') {
                        vm.isSelectAllSubCategory = true;
                    }

                    const subCategoryIds = setting.SubCategory.split(',');
                    vm.subCategoryDropdownSettings.sourceData = filterScripts.getSubCategories(
                        vm.selectedCategories,
                        allSettings.SubCategories,
                        vm.selectedSubCategoryIds);
                    totalItems = vm.subCategoryDropdownSettings.sourceData.length;
                    for (let i = 0; i < totalItems; i++) {
                        const index = subCategoryIds.findIndex(x => x === vm.subCategoryDropdownSettings.sourceData[i].Id.toString());
                        if (setting.SubCategory === '0' || index >= 0) {
                            vm.subCategoryDropdownSettings.sourceData[i].checked = true;
                            vm.selectedSubCategories.push(vm.subCategoryDropdownSettings.sourceData[i]);
                            vm.selectedSubCategoryIds.push(vm.subCategoryDropdownSettings.sourceData[i].Id);
                        }
                    }

                    const communicationIds = setting.CommTool.split(',');
                    totalItems = vm.communicationDropdownSettings.sourceData.length;
                    for (let i = 0; i < totalItems; i++) {
                        const index = communicationIds.findIndex(x => x === vm.communicationDropdownSettings.sourceData[i].Id.toString());
                        if (index >= 0) {
                            vm.communicationDropdownSettings.sourceData[i].checked = true;
                        }
                    }
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
                vm.isSuccessful = false;
            }

            return isValid;
        }

        function initialize() {
            const index = $window.parent.hotKeyManager.CachedData.ActiveDocumentManager.findIndex(x => x.Id === 'EditDefaultCommunicationTool');
            const activeDocument = {
                Value: vm,
                Id: 'EditDefaultCommunicationTool'
            };
            if (index === -1) {
                $window.parent.hotKeyManager.CachedData.ActiveDocumentManager.push(activeDocument);
            } else {
                $window.parent.hotKeyManager.CachedData.ActiveDocumentManager[index] = activeDocument;
            }

            getCommToolSettingById();
        }

        vm.close = function () {
            $uibModalInstance.close(vm.isEditSuccessfully);
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

        vm.save = function () {
            if (validateForm()) {
                const params = {
                    defaultCommId: communicationSettingId,
                    categoryIds: getCategoryIds(),
                    subCategoryIds: getSubCategoryIds(),
                    communicationIds: getCommunicationIds()
                };

                commToolSettingFactory.editCommToolSetting(params)
                    .then(function (data) {
                        $uiBlockerService.unblockUI();

                        vm.isSuccessful = data.Result.IsSuccessful;
                        vm.resultMessage = data.Result.Message;
                        vm.isShowResultMessage = true;

                        if (vm.isSuccessful) {
                            vm.isEditSuccessfully = true;
                            vm.isDisableEdit = true;
                            setTimeout(function () { vm.close(); }, sharedScriptManager.cachedData.alertSuccessTime);
                        }
                    });
            }
        };

        vm.invokeHotKey_CtrlU = function () {
            vm.save();
            $scope.$apply();
        };

        vm.invokeHotKey_CtrlC = function () {
            vm.close();
        };

        initialize();
    }
})(angular);