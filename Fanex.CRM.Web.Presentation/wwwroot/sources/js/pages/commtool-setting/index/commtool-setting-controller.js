(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('CommToolSettingController', commToolSettingController);
    commToolSettingController.$inject = ['$uibModal', '$uiBlockerService', 'commToolSettingFactory'];

    function commToolSettingController($uibModal, $uiBlockerService, commToolSettingFactory) {
        let vm = this;
        vm.filter = {
            customer: '',
            isSelectAllCategory: false,
            isSelectAllSubCategory: false,
            isDisableCategorySubCategory: true,
            pageIndex: 0,
            pageSize: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        vm.textSettings = {
            Filter: 'Search',
            NoData: 'No data',
            NoItemChecked: 'Please choose',
            AllItemsSelected: 'Any',
            CheckAll: 'Any'
        };
        vm.allSettings = {};
        vm.communicationSettings = [];
        vm.totalItems = 0;
        vm.selectedSubCategoryIds = [];
        vm.isDisableSearch = false;

        function getCommToolSettings() {
            $uiBlockerService.blockUI();

            const getCategoryIds = function () {
                let categoryIds = '';
                if (!vm.filter.isDisableCategorySubCategory) {
                    if (vm.filter.isSelectAllCategory) {
                        categoryIds = '0';
                    } else {
                        categoryIds = vm.selectedCategories.map(cat => cat.Id).sort().join(',');
                    }
                }

                return categoryIds;
            };
            const getSubCategoryIds = function () {
                let subCategoryIds = '';
                if (!vm.filter.isDisableCategorySubCategory) {
                    if (vm.filter.isSelectAllSubCategory) {
                        subCategoryIds = '0';
                    } else {
                        subCategoryIds = vm.selectedSubCategories.map(subcat => subcat.Id).sort().join(',');
                    }
                }

                return subCategoryIds;
            };

            const params = {
                username: vm.filter.customer.trim(),
                categoryIds: getCategoryIds(),
                subcategoryIds: getSubCategoryIds(),
                isDisableCategorySubCategory: vm.filter.isDisableCategorySubCategory,
                pageIndex: vm.filter.pageIndex + 1,
                pageSize: vm.filter.pageSize
            };
            commToolSettingFactory.getCommToolSettings(params)
                .then(function (data) {
                    vm.communicationSettings = data.Result.Items;
                    vm.totalItems = data.Result.TotalItems;
                    $uiBlockerService.unblockUI();
                });
        }

        function getAllSettings() {
            $uiBlockerService.blockUI();
            commToolSettingFactory.getAllSettings()
                .then(function (data) {
                    vm.allSettings = data.Result;
                    vm.categoryDropdownSettings = {
                        idMember: 'Id',
                        nameMember: 'Name',
                        sourceData: vm.allSettings.MainCategories,
                        checkedAll: true
                    };
                    vm.subCategoryDropdownSettings = {
                        idMember: 'Id',
                        nameMember: 'Name',
                        sourceData: vm.allSettings.SubCategories,
                        checkedAll: true
                    };

                    vm.selectedCategories = vm.allSettings.MainCategories;
                    vm.selectedSubCategories = vm.allSettings.SubCategories;
                    vm.filter.isDisableCategorySubCategory = true;
                    vm.filter.isSelectAllCategory = true;
                    vm.filter.isSelectAllSubCategory = true;

                    $uiBlockerService.unblockUI();
                });
        }

        function initialize() {
            getAllSettings();
            getCommToolSettings();
        }

        vm.checkDisableSearch = function () {
            vm.isDisableSearch = !vm.filter.isDisableCategorySubCategory
                && (vm.selectedCategories.length === 0 || vm.selectedSubCategories.length === 0);
        };

        vm.categoryChangedHandler = function () {
            vm.categorySelectAllHandler();
            vm.checkDisableSearch();
            vm.subCategoryDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: filterScripts.getSubCategories(
                    vm.selectedCategories,
                    vm.allSettings.SubCategories,
                    vm.selectedSubCategoryIds)
            };
        };

        vm.categorySelectAllHandler = function () {
            vm.filter.isSelectAllCategory = vm.categoryDropdownSettings.sourceData.length > 0
                && vm.selectedCategories.length === vm.categoryDropdownSettings.sourceData.length;
        };

        vm.subCategoryChangedHandler = function () {
            vm.checkDisableSearch();
            vm.subCategorySelectAllHandler();
            vm.selectedSubCategoryIds = [];
            angular.forEach(vm.selectedSubCategories, function (subCategory) {
                vm.selectedSubCategoryIds.push(subCategory.Id);
            });
        };

        vm.subCategorySelectAllHandler = function () {
            vm.filter.isSelectAllSubCategory = vm.subCategoryDropdownSettings.sourceData.length
                && vm.selectedSubCategories.length === vm.subCategoryDropdownSettings.sourceData.length;
        };

        vm.getCommToolSettings = function (pageIndex, pageSize) {
            vm.filter.pageIndex = pageIndex;
            vm.filter.pageSize = pageSize;

            getCommToolSettings();
        };

        vm.search = function () {
            vm.filter.pageIndex = 0;
            getCommToolSettings();
        };

        vm.openCreatePopup = function () {
            const modalInstance = $uibModal.open({
                template: angular.element('#template-create-commtool-setting').html(),
                controller: 'CreateCommToolSettingController as createCommToolSettingCtrl',
                backdrop: 'static',
                size: 'lg',
                windowClass: 'create-commtool-setting-modal',
                //appendTo: window.parent.$('body'),
                resolve: {
                    allSettings: function () {
                        return angular.copy(vm.allSettings);
                    }
                }
            });

            modalInstance.result.then(function (isSuccessful) {
                console.log(isSuccessful);
            });
        };

        vm.openEditPopup = function () {
            $uibModal.open({
                template: angular.element('#template-edit-commtool-setting').html(),
                controller: 'EditCommToolSettingController as editCommToolSettingCtrl',
                backdrop: 'static',
                size: 'lg',
                windowClass: 'create-commtool-setting-modal',
                //appendTo: window.parent.$('body'),
                resolve: {
                    allSettings: function () {
                        return angular.copy(vm.allSettings);
                    },
                    textSettings: function () {
                        return angular.copy(vm.textSettings);
                    }
                }
            });
        };

        initialize();
    }
})(angular);