(function () {
    "use strict";
    angular.module('pic-management-application').controller('picEditController', ['$rootScope', '$scope', '$window', '$picManagementFactory', '$uibModalInstance', '$uibModal', 'picItem', function ($rootScope, $scope, $window, $picManagementFactory, $uibModalInstance, $uibModal, picItem) {
        let vm = this;
        vm.isDiabled = false;
        $rootScope.validateMessage = '';
        vm.username = picItem.Username;
        vm.initSelectedRole = { Id: 0, Value: viewResources.Selected };
        vm.picTyped = '';
        vm.roles = [];
        vm.languageDropdownSettings = {
            idMember: 'Value',
            nameMember: 'Value',
            sourceData: [],
            checkedAll: false
        };

        vm.selectedCountry = { Id: 0, Name: viewResources.Selected};
        vm.remark = picItem.Remark;
        vm.communicationTools = [];
        vm.initialSelectedComm = { Id: -1, Value: viewResources.Selected };
        vm.messageResultOptions = new MessageResult('', false, false);

        // start muilti select
        vm.textSettings = {
            Filter: viewResources.Search,
            NoData: viewResources.NoData,
            NoItemChecked: viewResources.Selected,
            AllItemsSelected: viewResources.All,
            CheckAll: viewResources.All
        };

        vm.languageChangedHandler = function () {
            vm.isSelectAllLanguage = vm.languageDropdownSettings.sourceData.length > 0
                && vm.selectedLanguages.length === vm.languageDropdownSettings.sourceData.length;
        };

        vm.communicationToolItems = [
            { selectedComm : vm.initialSelectedComm, Value: '' }
        ];
        
        vm.oldCommunicationToolItems = [
            { selectedComm : vm.initialSelectedComm, Value: '' }
        ];

        vm.getAllSetting = function () {
            vm.isDiabled = true;
            $picManagementFactory.getAllSetting()
                .then(function (data) {
                    let allSetting = data.Result;
                    vm.picTypes = allSetting.PicTypeList;
                    vm.picTyped = picItem.PicType;
                    vm.selectedPicType = vm.picTypes.filter(type => type.Id === picItem.PicType)[0];
                    vm.GetRoleByPic(vm.selectedPicType.Id);

                    vm.communicationTools = allSetting.CommunicationTools;
                    vm.communicationTools.unshift(vm.initialSelectedComm);
                    if (picItem.CommunicationTools && picItem.CommunicationTools.length > 0) {
                        let communicationToolItems = picItem.CommunicationTools.map(tool => {
                            let selectedComms = vm.communicationTools.filter(t => t.Id === tool.SettingId);
                            if (selectedComms.length > 0) {
                                return { selectedComm: selectedComms[0], Value: tool.Value };
                            }

                            return { selectedComm: vm.initialSelectedComm, Value: tool.Value }
                        });

                        vm.oldCommunicationToolItems = angular.copy(communicationToolItems);
                        vm.communicationToolItems = communicationToolItems;
                    }

                    let languages = angular.copy(allSetting.Languages);
                    for (let index = 0; index < picItem.Languages.length; index++) {
                        let selectedIndex = languages.findIndex(lang => lang.Id === picItem.Languages[index].Id);
                        if (selectedIndex < 0) {
                            let inactiveLanguage = picItem.Languages[index];
                            inactiveLanguage.checked = true;
                            languages.push(inactiveLanguage);
                        }
                        else {
                            languages[selectedIndex].checked = true;;
                        }
                    }

                    vm.selectedLanguages = picItem.Languages;
                    vm.languageDropdownSettings = {
                        idMember: 'Value',
                        nameMember: 'Value',
                        sourceData: languages,
                        checkedAll: vm.selectedLanguages.length === languages.length
                    };

                    vm.countries = allSetting.Countries;
                    vm.countries.unshift(vm.selectedCountry);
                    let selectedCountry = { Id: picItem.CountryId, Name: picItem.CountryName }
                    if (vm.countries.filter(country => country.Id === picItem.CountryId).length === 0) {
                        vm.countries.push(selectedCountry);
                    }

                    vm.selectedCountry = vm.countries.filter(country => country.Id === picItem.CountryId)[0];
                    vm.isDiabled = false;
                }, function () {
                    vm.isDiabled = false;
                });
        };

        vm.GetRoleByPic = function (selectedPicType) {
            vm.isDiabled = true;
            const param = { PicType: selectedPicType };
            $picManagementFactory.getRolesByPic(param)
                .then(function (data) {
                    vm.roles = data.Result;
                    if (vm.roles.filter(role => role.Id === picItem.RoleId).length === 0) {
                        vm.selectedRole = vm.initSelectedRole
                        vm.roles.unshift(vm.selectedRole);
                    }
                    else {
                        vm.selectedRole = vm.roles.filter(role => role.Id === picItem.RoleId)[0];
                    }
                    vm.isDiabled = false;
                }, function () {
                    vm.isDiabled = false;
                });
        };

        vm.savePic = function () {
            vm.isDiabled = true;

            let communicationToolItems = [];
            for (let index = 0; index < vm.communicationToolItems.length; index++) {
                if (vm.communicationToolItems[index].selectedComm.Id > 0 && vm.communicationToolItems[index].Value.trim() !== '') {
                    if (communicationToolItems.some(element => element.selectedComm.Id === vm.communicationToolItems[index].selectedComm.Id)) {
                        let tempIndex = communicationToolItems.findIndex(element1 => element1.selectedComm.Id === vm.communicationToolItems[index].selectedComm.Id);
                        communicationToolItems[tempIndex].Value = communicationToolItems[tempIndex].Value + ',' + vm.communicationToolItems[index].Value;
                    }
                    else {
                        communicationToolItems.push(vm.communicationToolItems[index]);
                    }
                }
            }

            let criteria = {
                PicId: picItem.PicId,
                PicType: vm.selectedPicType.Id,
                Username: vm.username,
                Role: vm.selectedRole.Id,
                Remark: vm.remark,
                CommunicationTools: communicationToolItems.map(commItem => {
                    const values = commItem.Value.split(',').map(value => value.trim()).filter(value => value !== '').toString();
                    return { Type: commItem.selectedComm.Id, Value: values }
                }),
                Language: vm.selectedLanguages.map(lang => lang.Id).join(','),
                Country: vm.selectedCountry.Id
            };

            let message = vm.validateParams(criteria);
            if (message === '') {
                vm.messageResultOptions = new MessageResult('', false, false);
                $picManagementFactory.validateEditPic({
                    PicId: criteria.PicId,
                    CommunicationTools: criteria.CommunicationTools
                }).then(function (data) {
                    $rootScope.validateMessage = data.Result;
                    if (data.Result) {
                        const modalValidateInstance = $uibModal.open({
                            template: angular.element('#template-validate-edit-pic-confirm').html(),
                            controller: 'validateEditPicConfirmController',
                            controllerAs: 'validateEditPicConfirmCtrl',
                            backdrop: 'static',
                            backdropClass: 'validate-edit-pic-confirm-modal-backdrop',
                            keyboard: false,
                            size: 'lg',
                            windowClass: 'validate-edit-pic-confirm-modal',
                            appendTo: $window.parent.$('body'),
                        });
    
                        modalValidateInstance.result.then(function (isConfirmed) {
                            if (isConfirmed) {
                                vm.editPic(criteria);
                            }
                            else {
                                vm.isDiabled = false;
                            }
                        });
                    }
                    else {
                        vm.editPic(criteria);
                    }
                }, function () {
                    vm.isDiabled = false;
                });
            } else {
                vm.isDiabled = false;
                vm.messageResultOptions = new MessageResult(message, true, false)
            }
        };

        vm.editPic = function (criteria) {
            $picManagementFactory.editPic(criteria)
                .then(function (data) {
                    vm.showInsertResult(data.Result);
                }, function () {
                    vm.isDiabled = false;
                });
        };

        vm.validateParams = function (params) {
            let message = '';
            if (params.Username === '') {
                message = viewResources.Username + ' ' + viewResources.IsRequired;
            }
            else if (params.PicType <= 0) {
                message = viewResources.PicType + ' ' + viewResources.IsRequired;
            }
            else if (params.Role <= 0) {
                message = viewResources.Role + ' ' + viewResources.IsRequired;
            }
            else if (vm.communicationToolItems[0].selectedComm.Id < 0 || vm.communicationToolItems[0].Value.trim() === '') {
                message = viewResources.CommunicationTool + ' ' + viewResources.IsRequired;
            }

            return message;
        }

        vm.showInsertResult = function (result) {
            if (result.Code !== 0) {
                vm.isDiabled = false;
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
            let item = { selectedComm: vm.initialSelectedComm, Value: '' };
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
            if (vm.username !== picItem.Username ||
                vm.selectedPicType.Id !== picItem.PicType ||
                vm.selectedRole.Id !== picItem.RoleId ||
                vm.remark !== picItem.Remark ||
                vm.isLanguageChanged() ||
                vm.selectedCountry.Id !== picItem.CountryId ||
                vm.isCommChanged()) {
                result = true;
            }

            return result;
        };

        vm.isLanguageChanged = function () {
            let result = vm.selectedLanguages.length !== picItem.Languages.length;
            if (!result) {
                for (let index = 0; index < vm.selectedLanguages.length; index++) {
                    let selectedIndex = picItem.Languages.findIndex(lang => lang.Id === vm.selectedLanguages[index].Id);
                    if (selectedIndex < 0) {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        vm.isCommChanged = function () {
            let result = vm.communicationToolItems.length !== vm.oldCommunicationToolItems.length;
            if (!result) {
                for (let index = 0; index < vm.communicationToolItems.length; index++) {
                    if (vm.communicationToolItems[index].selectedComm.Id !== vm.oldCommunicationToolItems[index].selectedComm.Id || vm.communicationToolItems[index].Value !== vm.oldCommunicationToolItems[index].Value) {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        angular.element(document).ready(function () {
            vm.getAllSetting();
        });

        $($window.parent.document).off('keydown').on('keydown', function (eventObject) {
            if (!vm.isDiabled && eventObject.ctrlKey) {
                switch (eventObject.keyCode) {
                    case 83: // s
                        vm.savePic();
                        $scope.$apply();
                        eventObject.preventDefault();
                        return false;
                }
            }
        });
    }]);
})();