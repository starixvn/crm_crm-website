(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('pic-management-application').controller('picInsertController', ['$scope', '$picManagementFactory', '$uibModalInstance',
        function ($scope, $picManagementFactory, $uibModalInstance) {
            let vm = this;

            vm.errorMessage = '';
            vm.roleInsertOptions = null;
            vm.communicationToolsInsertOptions = null;
            vm.countryOptions = null;
            vm.isShowResultMessage = false;

            // start muilti select
            vm.textSettings = {
                Filter: 'Search',
                NoData: 'No data',
                NoItemChecked: 'Please choose',
                AllItemsSelected: 'Any',
                CheckAll: 'Any'
            };

            vm.selectedLanguages = [];

            vm.languageChangedHandler = function () {
                vm.isSelectAllLanguage = vm.languageDropdownSettings.sourceData.length > 0
                    && vm.selectedLanguages.length === vm.languageDropdownSettings.sourceData.length;
            };
            // start muilti select

            vm.insertParams = {
                Username: '',
                Role: '',
                Remark: '',
                CommunicationTools: [],
                Country: ''
            };

            vm.communicationToolItems = [
                {Type: -1, Value: ""}                
            ]

            vm.initController = function () {
                vm.initPicker();
                vm.getAllSetting();
            };

            vm.getAllSetting = function () {
                vm.BlockUI();
                $picManagementFactory.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        vm.roleInsertOptions.DataList = allSetting.RoleList;
                        vm.communicationToolsInsertOptions.DataList = allSetting.CommunicationTools;

                        vm.languageDropdownSettings = {
                            idMember: 'Name',
                            nameMember: 'Name',
                            sourceData: angular.copy(allSetting.Languages),
                            checkedAll: false
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
            }

            vm.updateSelectedCommunicationTool = function (target, selectedItem) {
                target.Type = selectedItem.Id;               
            }

            vm.updateSelectedCountry = function (selectedItem) {
                vm.insertParams.Country = selectedItem.Name;
            }

            vm.completedLoadFilterInfo = function () {
                vm.unblockUI();
            };

            vm.savePic = function () {
                vm.BlockUI();

                var communicationToolItems = [];
                for (let index = 0; index < vm.communicationToolItems.length; index++) {
                    if (communicationToolItems.some(element => element.Type === vm.communicationToolItems[index].Type)) {
                        let tempIndex = communicationToolItems.findIndex(element1 => element1.Type === vm.communicationToolItems[index].Type);
                        communicationToolItems[tempIndex].Value = communicationToolItems[tempIndex].Value + ',' + vm.communicationToolItems[index].Value;
                    }
                    else {
                        communicationToolItems.push(vm.communicationToolItems[index]);
                    }
                }
                
                var criteria = {
                    Username: vm.insertParams.Username,
                    Role: vm.insertParams.Role,
                    Remark: vm.insertParams.Remark,
                    CommunicationTools: communicationToolItems,
                    Language: vm.selectedLanguages.map(lang => lang.Name).join(','),
                    Country: vm.insertParams.Country
                };

                if (vm.validateParams(criteria)) {
                    vm.isShowResultMessage = false;
                } else {
                    vm.isShowResultMessage = true;
                    vm.isSuccessful = false;
                }

                $picManagementFactory.insertPic(criteria)
                    .then(function (data) {
                        vm.showInsertResult(data.Result);                       
                    }, function () {
                        vm.unblockUI();
                    });
            };

            vm.validateParams = function (params) {
                var result = true;
                var message = '';

                if (params.Username == '') {
                    message = viewResources.Username + " " + viewResources.IsRequired;
                    result = false;
                }
                else if (params.Role == '') {
                    message = viewResources.Role + " " + viewResources.IsRequired;
                    result = false;
                }
                else if (params.CommunicationTools.length == 0 || params.CommunicationTools[0].Type <= 0 ||
                    params.CommunicationTools[0].Value == '') {
                    message = viewResources.CommunicationTool + " " + viewResources.IsRequired;
                    result = false;
                }

                vm.messageResultOptions = new MessageResult(message, !result, result);
                return result;
            }

            vm.showInsertResult = function (result) {
                vm.unblockUI();

                if (result.Code !== 0) {
                    vm.messageResultOptions = new MessageResult(result.Message, true, false);
                    return;
                }

                vm.messageResultOptions = new MessageResult(result.Message, true, true);
                parent.homeManager.CachedData.shouldReloadView = true;
                setTimeout(function () {
                    vm.messageResultOptions = new MessageResult('', false, false);                    
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            };

            vm.close = function () {
                vm.updateChangeFlag();
                $uibModalInstance.close();
            };        

            vm.addCommunicationTool = function () {
                var item = { Type: -1, Value: "" };
                vm.communicationToolItems.push(item);
            };

            vm.removeCommunicationTool = function (item) {
                var index = vm.communicationToolItems.indexOf(item.x);
                if (index > -1) {
                    vm.communicationToolItems.splice(index, 1);
                }
            };

            vm.updateChangeFlag = function () {
                if (vm.isParamsChange()) {
                    parent.homeManager.CachedData.isChangingData = true;
                    parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.ConfirmCancel;
                } else {
                    parent.homeManager.CachedData.isChangingData = false;
                }

                //if (!vm.$$phase) {
                //    $scope.$apply();
                //}
            };

            vm.isParamsChange = function () {
                var result = false;
                var params = {
                    Username: vm.insertParams.Username,
                    Role: vm.insertParams.Role,
                    Remark: vm.insertParams.Remark,
                    CommunicationTools: vm.communicationToolItems
                };

                if (params.Username != '' ||
                    params.Role != '' ||
                    params.Remark != '' ||
                    params.Language != '' ||
                    params.Country != '' ||
                    params.CommunicationTools[0].Type > 0 ||
                    params.CommunicationTools[0].Value != '')
                {
                    result = true;
                }   
                
                return result;
            };

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

            vm.invokeHotKey_CtrlS = function () {
                vm.savePic();
                $scope.$apply();
            };

            angular.element(document).ready(function () {
                vm.initController();

                hotKeyManager.CachedData.ActiveDocumentManager.push({
                    Value: $scope,
                    Id: 'PicInsert'
                });
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            });
        }]);
})();