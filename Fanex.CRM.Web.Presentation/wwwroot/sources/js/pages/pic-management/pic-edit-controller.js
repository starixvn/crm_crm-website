(function () {
    "use strict";
    angular.module('pic-management-application').controller('picEditController', ['$scope', '$window', '$picManagementFactory', '$uibModalInstance', '$uibModal', 'picItem', function ($scope, $window, $picManagementFactory, $uibModalInstance, $uibModal, picItem) {
        let vm = this;
        vm.isDiabled = false;
        vm.username = picItem.Username;
        vm.roles = [];
        vm.selectedRole = { Value: viewResources.Selected };
        vm.languageDropdownSettings = {
            idMember: 'Value',
            nameMember: 'Value',
            sourceData: [],
            checkedAll: false
        };

        vm.selectedCountry = { Name: viewResources.Selected};
        vm.remark = picItem.Remark;
        vm.communicationTools = [];
        vm.initialSelectedComm = { Id: -1, Name: viewResources.Selected };
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
                    vm.roles = allSetting.RoleList;
                    vm.roles.unshift(vm.selectedRole);
                    vm.selectedRole = vm.roles.filter(role => role.Value === picItem.Role)[0];
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
                    let selectedLanguagesListString = picItem.Language.split(',').map(langStr => langStr.trim());
                    let isCheckedAll = languages.length === selectedLanguagesListString.length;
                    for (let index = 0; index < languages.length; index++) {
                        let selectedIndex = selectedLanguagesListString.findIndex(lang => lang === languages[index].Value.trim());
                        languages[index].checked = isCheckedAll || selectedIndex >= 0;
                    }

                    vm.languageDropdownSettings = {
                        idMember: 'Value',
                        nameMember: 'Value',
                        sourceData: languages,
                        checkedAll: isCheckedAll
                    };

                    vm.selectedLanguages = languages.filter(lang => lang.checked);
                    vm.countries = allSetting.Countries;
                    vm.countries.unshift(vm.selectedCountry);
                    let selectedCountries = vm.countries.filter(country => country.Name === picItem.Country);
                    if (selectedCountries.length > 0) {
                        vm.selectedCountry = selectedCountries[0];
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
                Username: vm.username,
                Role: vm.selectedRole.Value === viewResources.Selected ? '' : vm.selectedRole.Value,
                Remark: vm.remark,
                CommunicationTools: communicationToolItems.map(commItem => { return { Type: commItem.selectedComm.Id, Value: commItem.Value} }),
                Language: vm.selectedLanguages.map(lang => lang.Value).join(', '),
                Country: vm.selectedCountry.Name === viewResources.Selected ? '' : vm.selectedCountry.Name
            };

            let message = vm.validateParams(criteria);
            if (message === '') {
                vm.messageResultOptions = new MessageResult('', false, false);

                $picManagementFactory.editPic(criteria)
                    .then(function (data) {
                        vm.showInsertResult(data.Result);
                    }, function () {
                        vm.isDiabled = false;
                    });
            } else {
                vm.isDiabled = false;
                vm.messageResultOptions = new MessageResult(message, true, false)
            }
        };

        vm.validateParams = function (params) {
            let message = '';
            if (params.Username === '') {
                message = viewResources.Username + ' ' + viewResources.IsRequired;
            }
            else if (params.Role === '') {
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
            let selectedRole = vm.selectedRole.Value === viewResources.Selected ? '' : vm.selectedRole.Value;
            let selectedCountry = vm.selectedCountry.Name === viewResources.Selected ? '' : vm.selectedCountry.Name;
            if (vm.username !== picItem.Username ||
                selectedRole !== picItem.Role ||
                vm.remark !== picItem.Remark ||
                vm.selectedLanguages.map(lang => lang.Value).join(',') !== picItem.Language.split(',').map(lang => lang.trim()).join(',') ||
                selectedCountry !== picItem.Country ||
                vm.isCommChanged(vm.communicationToolItems)) {
                result = true;
            }

            return result;
        };

        vm.isCommChanged = function (commTools) {
            let result = commTools.length !== vm.oldCommunicationToolItems.length;
            if (!result) {
                for (let index = 0; index < commTools.length; index++) {
                    if (commTools[index].selectedComm.Id !== vm.oldCommunicationToolItems[index].selectedComm.Id || commTools[index].Value !== vm.oldCommunicationToolItems[index].Value) {
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