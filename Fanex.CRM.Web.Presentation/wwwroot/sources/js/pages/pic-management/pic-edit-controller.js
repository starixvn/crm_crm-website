(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('pic-management-application').controller('picEditController', ['$window', '$picManagementFactory', '$uibModalInstance', '$uibModal', 'picItem', function ($window, $picManagementFactory, $uibModalInstance, $uibModal, picItem) {
        let vm = this;
        vm.roleInsertOptions = null;
        vm.communicationToolsInsertOptions = null;
        vm.countryOptions = null;
        vm.messageResultOptions = new MessageResult('', false, false);

        // start muilti select
        vm.textSettings = {
            Filter: 'Search',
            NoData: 'No data',
            NoItemChecked: 'Select',
            AllItemsSelected: 'All',
            CheckAll: 'All'
        };

        vm.getSelectedLanguages = function (stringLanguages, data) {
            let stringLanguageArray = stringLanguages.split(',').map(lang => lang.trim());
            return data.filter(item => stringLanguageArray.includes(item.Name));
        };

        vm.selectedLanguages = [];
        vm.oldLanguages = [];

        vm.languageChangedHandler = function () {
            vm.isSelectAllLanguage = vm.languageDropdownSettings.sourceData.length > 0
                && vm.selectedLanguages.length === vm.languageDropdownSettings.sourceData.length;
        };

        vm.insertParams = {
            PicId: picItem.PicId,
            Username: picItem.Username,
            Role: picItem.Role,
            Remark: picItem.Remark,
            CommunicationTools: picItem.CommunicationTools,
            Country: picItem.Country
        };

        vm.communicationToolItems = picItem.CommunicationTools && picItem.CommunicationTools.length > 0 ? picItem.CommunicationTools.map(tool => { return { Type: tool.SettingId, Value: tool.Value } }) : [
            { Type: -1, Value: "" }
        ]

        vm.oldCommunicationToolItems = picItem.CommunicationTools && picItem.CommunicationTools.length > 0 ? picItem.CommunicationTools.map(tool => { return { Type: tool.SettingId, Value: tool.Value } }) : [
            { Type: -1, Value: "" }
        ]

        vm.initController = function () {
            vm.initPicker();
            vm.getAllSetting();
        };

        vm.getAllSetting = function () {
            vm.BlockUI();
            $picManagementFactory.getAllSetting()
                .then(function (data) {
                    let allSetting = data.Result;
                    vm.roleInsertOptions.DataList = allSetting.RoleList;
                    vm.communicationToolsInsertOptions.DataList = allSetting.CommunicationTools;
                    let languages = angular.copy(allSetting.Languages);
                    vm.selectedLanguages = vm.oldLanguages = vm.getSelectedLanguages(picItem.Language, languages);
                    let isCheckedAll = languages.length === vm.selectedLanguages.length;
                    for (let index = 0; index < languages.length; index++) {
                        let selectedIndex = vm.selectedLanguages.findIndex(lang => lang.Id === languages[index].Id);
                        languages[index].checked = isCheckedAll || selectedIndex >= 0;
                    }

                    vm.languageDropdownSettings = {
                        idMember: 'Name',
                        nameMember: 'Name',
                        sourceData: languages,
                        checkedAll: isCheckedAll
                    };

                    vm.countryOptions.DataList = allSetting.Countries;
                    vm.unblockUI();
                }, function () {
                    vm.unblockUI();
                });
        };

        vm.initPicker = function () {
            vm.roleInsertOptions = {
                DataList: [],
                OnChangeEvent: vm.updateSelectedRole,
                AllText: viewResources.All
            };

            vm.communicationToolsInsertOptions = {
                DataList: [],
                OnChangeEvent: vm.updateSelectedCommunicationTool,
                AllText: viewResources.All
            }

            vm.countryOptions = {
                DataList: [],
                OnChangeEvent: vm.updateSelectedCountry,
                AllText: viewResources.All
            };

            vm.languageDropdownSettings = {
                idMember: 'Name',
                nameMember: 'Name',
                sourceData: [],
                checkedAll: false
            };
        };

        vm.updateSelectedRole = function (selectedItem) {
            vm.insertParams.Role = selectedItem.Value;
        };

        vm.updateSelectedCommunicationTool = function (target, selectedItem) {
            target.Type = selectedItem.Id;
        };

        vm.updateSelectedCountry = function (selectedItem) {
            vm.insertParams.Country = selectedItem.Name;
        }

        vm.completedLoadFilterInfo = function () {
            vm.unblockUI();
        };

        vm.savePic = function () {
            vm.BlockUI();

            let communicationToolItems = [];
            for (let index = 0; index < vm.communicationToolItems.length; index++) {
                if (communicationToolItems.some(element => element.Type === vm.communicationToolItems[index].Type)) {
                    let tempIndex = communicationToolItems.findIndex(element1 => element1.Type === vm.communicationToolItems[index].Type);
                    communicationToolItems[tempIndex].Value = communicationToolItems[tempIndex].Value + ',' + vm.communicationToolItems[index].Value;
                }
                else {
                    communicationToolItems.push(vm.communicationToolItems[index]);
                }
            }

            let criteria = {
                PicId: picItem.PicId,
                Username: vm.insertParams.Username,
                Role: vm.insertParams.Role,
                Remark: vm.insertParams.Remark,
                CommunicationTools: communicationToolItems,
                Language: vm.selectedLanguages.map(lang => lang.Name).join(', '),
                Country: vm.insertParams.Country
            };

            if (vm.validateParams(criteria) === '') {
                vm.messageResultOptions = new MessageResult('', false, false);

                $picManagementFactory.editPic(criteria)
                    .then(function (data) {
                        vm.showInsertResult(data.Result);
                    }, function () {
                        vm.unblockUI();
                    });
            } else {
                vm.messageResultOptions = new MessageResult('', true, false)
            }
        };

        vm.validateParams = function (params) {
            let message = '';
            if (params.Username == '') {
                message = viewResources.Username + " " + viewResources.IsRequired;
            }
            else if (params.Role == '') {
                message = viewResources.Role + " " + viewResources.IsRequired;
            }
            else if (params.CommunicationTools.length == 0 || params.CommunicationTools[0].Type <= 0 ||
                params.CommunicationTools[0].Value == '') {
                message = viewResources.CommunicationTool + " " + viewResources.IsRequired;
            }

            return message;
        }

        vm.showInsertResult = function (result) {
            vm.unblockUI();

            if (result.Code !== 0) {
                vm.messageResultOptions = new MessageResult(result.Message, true, false);
                return;
            }

            vm.messageResultOptions = new MessageResult(result.Message, true, true);
            setTimeout(function () {
                vm.close()
            }, 1000);
        };

        vm.close = function () {
            if (vm.messageResultOptions.IsSuccess) {
                vm.messageResultOptions = new MessageResult('', false, false);
                $uibModalInstance.close(true);
            }
            else {
                if (vm.isParamsChange()) {
                    const modalConfirmInstance = $uibModal.open({
                        template: angular.element('#template-pic-change-confirm').html(),
                        controller: 'picChangeConfirmController',
                        controllerAs: 'picChangeConfirmCtrl',
                        backdrop: 'static',
                        backdropClass: 'pic-change-confirm-modal-backdrop',
                        keyboard: false,
                        size: 'lg',
                        windowClass: 'pic-change-confirm-modal',
                        appendTo: $window.parent.$('body'),
                    });

                    modalConfirmInstance.result.then(function (isConfirmed) {
                        if (isConfirmed) {
                            $uibModalInstance.close();
                        }
                    });
                }
                else {
                    $uibModalInstance.close();
                }
            }
        };

        vm.addCommunicationTool = function () {
            let item = { Type: -1, Value: "" };
            vm.communicationToolItems.push(item);
        };

        vm.removeCommunicationTool = function (item) {
            let index = vm.communicationToolItems.indexOf(item.x);
            if (index > -1) {
                vm.communicationToolItems.splice(index, 1);
            }
        };

        vm.isParamsChange = function () {
            let result = false;
            if (vm.insertParams.Username !== picItem.Username ||
                vm.insertParams.Role !== picItem.Role ||
                vm.insertParams.Remark !== picItem.Remark ||
                vm.selectedLanguages.map(lang => lang.Name).join(',') !== vm.oldLanguages.map(lang => lang.Name).join(',') ||
                vm.insertParams.Country !== picItem.Country ||
                vm.isCommChanged(vm.communicationToolItems)) {
                result = true;
            }

            return result;
        };

        vm.isCommChanged = function (commTools) {
            let result = commTools.length !== vm.oldCommunicationToolItems.length;
            if (!result) {
                for (let index = 0; index < commTools.length; index++) {
                    if (commTools[index].Type !== vm.oldCommunicationToolItems[index].Type || commTools[index].Value !== vm.oldCommunicationToolItems[index].Value) {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        vm.ignoreChanges = function () {
            parent.homeManager.CachedData.isChangingData = false;
            parent.ClosePopup();
        };

        vm.setfocus = function () {
            parent.homemanager.cacheddata.ischangingdata = false;
            vm.issetfocus = true;
        };

        vm.unblockUI = function () {
            try {
                if (parent && parent.$.unblockUI) {
                    parent.$.unblockUI();
                }
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };

        vm.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                if (parent && parent.BlockUI) {
                    parent.BlockUI();
                }
            } catch (e) {
            }
        };

        angular.element(document).ready(function () {
            vm.initController();
        });
    }]);
})();