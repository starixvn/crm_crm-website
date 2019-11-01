(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('CommToolSettingController', commToolSettingController);
    commToolSettingController.$inject = ['$window', '$uibModal', '$uiBlockerService', 'commToolSettingFactory'];

    function commToolSettingController($window, $uibModal, $uiBlockerService, commToolSettingFactory) {
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
                        sourceData: angular.copy(vm.allSettings.MainCategories),
                        checkedAll: true
                    };
                    vm.subCategoryDropdownSettings = {
                        idMember: 'Id',
                        nameMember: 'Name',
                        sourceData: angular.copy(vm.allSettings.SubCategories),
                        checkedAll: true
                    };

                    vm.selectedCategories = angular.copy(vm.allSettings.MainCategories);
                    vm.selectedSubCategories = angular.copy(vm.allSettings.SubCategories);
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
                    angular.copy(vm.allSettings.SubCategories),
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
                controller: 'CreateCommToolSettingController',
                controllerAs: 'createCommToolSettingCtrl',
                backdrop: 'static',
                backdropClass: 'create-commtool-setting-modal-backdrop',
                keyboard: false,
                size: 'lg',
                windowClass: 'create-commtool-setting-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    allSettings: angular.copy(vm.allSettings)
                }
            });

            modalInstance.result.then(function (isSuccessful) {
                if (isSuccessful) {
                    getCommToolSettings();
                }
            });
        };

        vm.openEditPopup = function (communicationSettingId) {
            const modalInstance = $uibModal.open({
                template: angular.element('#template-edit-commtool-setting').html(),
                controller: 'EditCommToolSettingController',
                controllerAs: 'editCommToolSettingCtrl',
                backdrop: 'static',
                backdropClass: 'create-commtool-setting-modal-backdrop',
                keyboard: false,
                size: 'lg',
                windowClass: 'create-commtool-setting-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    allSettings: angular.copy(vm.allSettings),
                    communicationSettingId: communicationSettingId
                }
            });

            modalInstance.result.then(function (isSuccessful) {
                if (isSuccessful) {
                    getCommToolSettings();
                }
            });
        };

        vm.openDeletePopup = function (communicationSettingId, rownum) {
            const modalInstance = $uibModal.open({
                template: angular.element('#template-delete-commtool-setting').html(),
                controller: 'DeleteCommToolSettingController',
                controllerAs: 'deleteCommToolSettingCtrl',
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                appendTo: $window.parent.$('body'),
                resolve: {
                    communicationSettingId: communicationSettingId,
                    rownum: rownum
                }
            });
            modalInstance.result.then(function (isSuccessful) {
                if (isSuccessful) {
                    getCommToolSettings();
                }
            });
        };

        angular.element(document).ready(function () {
            initialize();
            const style = $('head link[href*="commtool.setting.css"]').clone();
            $window.parent.$('head').append(style);
        });
    }
})(angular);