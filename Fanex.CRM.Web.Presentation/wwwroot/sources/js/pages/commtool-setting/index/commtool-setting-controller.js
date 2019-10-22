(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').controller('CommToolSettingController', commToolSettingController);
    commToolSettingController.$inject = ['$uibModal', '$uiBlockerService', 'commToolSettingFactory'];

    function commToolSettingController($uibModal, $uiBlockerService, commToolSettingFactory) {
        let vm = this;
        vm.filter = {
            customer: '',
            isDisableCategorySubCategory: true
        };
        vm.pagerOption = {
            pageIndex: 0,
            itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        vm.textSettings = {
            Filter: 'Search',
            NoData: 'No data',
            NoItemChecked: 'Please choose',
            AllItemsSelected: 'Any',
            CheckAll: 'Any'
        };
        vm.allSettings = {};
        vm.communicationSettings = [
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Email' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk' },
            { Customer: 'usera', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Letstalk, Email' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Line, Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' },
            { Customer: 'userb', Category: 'Any', SubCategory: 'Any', CommunicationTool: 'Skype' }
        ];
        vm.totalItems = 115;
        vm.selectedSubCategoryIds = [];

        function getCommToolSettings(pageIndex, itemsPerPage) {
            $uiBlockerService.blockUI();

            const params = {
                titleId: titleId,
                pageIndex: pageIndex,
                itemsPerPage: itemsPerPage
            };
            commToolSettingFactory.getCommToolSettings(params)
                .then(function (data) {
                    vm.communicationSettings = data.Result.Items;
                    vm.totalItems = data.Result.TotalItems;
                    $uiBlockerService.unblockUI();
                }, function () {
                    vm.communicationSettings = [];
                    vm.totalItems = 0;
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
                    $uiBlockerService.unblockUI();
                });
        }

        function initialize() {
            getAllSettings();
        }

        vm.categoryChangedHandler = function () {
            vm.subCategoryDropdownSettings = {
                idMember: 'Id',
                nameMember: 'Name',
                sourceData: filterScripts.getSubCategories(
                    vm.selectedCategories,
                    vm.allSettings.SubCategories,
                    vm.selectedSubCategoryIds)
            };
        };

        vm.subCategoryChangedHandler = function () {
            vm.selectedSubCategoryIds = [];
            angular.forEach(vm.selectedSubCategories, function (subCategory) {
                vm.selectedSubCategoryIds.push(subCategory.Id);
            });
        };

        vm.getCommToolSettings = function (pageIndex, itemsPerPage) {
            vm.pagerOption.pageIndex = pageIndex;
            vm.pagerOption.itemsPerPage = itemsPerPage;

            getCommToolSettings(pageIndex + 1, itemsPerPage);
        };

        vm.openCreatePopup = function () {
            $uibModal.open({
                template: angular.element('#template-create-commtool-setting').html(),
                controller: 'CreateCommToolSettingController as createCommToolSettingCtrl',
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