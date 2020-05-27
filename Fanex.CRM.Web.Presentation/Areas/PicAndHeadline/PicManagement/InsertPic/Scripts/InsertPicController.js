(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('insertpic-application').controller('insertPicController', ['$scope', '$insertPicService', '$compile',
        function ($scope, $insertPicService, $compile) {
            $scope.errorMessage = '';
            $scope.roleOptions = null;
            $scope.communicationToolOptions = null;
            $scope.languageOptions = null;
            $scope.countryOptions = null;

            $scope.insertParams = {
                Username: '',
                Role: '',
                Remark: '',
                CommunicationTools: [],
                Language: '',
                Country: ''
            };

            $scope.SelectedLanguage = '';

            $scope.communicationToolItems = [
                {Type: -1, Value: ""}                
            ]

            $scope.initController = function () {
                $scope.initPicker();

                $scope.getAllSetting();
                $scope.defaultFocusOptions = {
                    IsSetFocus: false,
                    OnChangeEvent: $scope.completeSetFocus
                };

                $scope.defaultFocusOptions.IsSetFocus = true;
                $scope.$apply();
            };

            $scope.initPicker = function () {
                $scope.roleOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedRole,
                    AllText: viewResources.All
                };

                $scope.communicationToolOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedCommunicationTool,
                    AllText: viewResources.All
                }

                $scope.languageOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedLanguage
                };

                $scope.countryOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedCountry,
                    AllText: viewResources.All
                };
            };

            $scope.updateSelectedRole = function (selectedItem) {
                $scope.insertParams.Role = selectedItem.Value;
            }

            $scope.updateSelectedCommunicationTool = function (target, selectedItem) {
                target.Type = selectedItem.Id;               
            }

            $scope.updateSelectedLanguage = function (selectedItem) {
                $scope.insertParams.Language = selectedItem;
            }

            $scope.updateSelectedCountry = function (selectedItem) {
                $scope.insertParams.Country = selectedItem.Name;
            }

            $scope.getAllSetting = function () {
                $scope.BlockUI();
                $scope.isLoadedAllSite = false;
                $insertPicService.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.roleOptions.DataList = allSetting.RoleList;
                        $scope.communicationToolOptions.DataList = allSetting.CommunicationTools;
                        $scope.languageOptions.DataList = allSetting.Languages;
                        $scope.countryOptions.DataList = allSetting.Countries;
                        $scope.completedLoadFilterInfo();
                    }, function () {
                        $scope.completedLoadFilterInfo();
                    });
            };

            $scope.completedLoadFilterInfo = function () {
                $scope.unblockUI();
            };

            $scope.savePic = function () {
                $scope.BlockUI();

                var communicationToolItems = [];
                for (let index = 0; index < $scope.communicationToolItems.length; index++) {
                    if (communicationToolItems.some(element => element.Type === $scope.communicationToolItems[index].Type)) {
                        let tempIndex = communicationToolItems.findIndex(element1 => element1.Type === $scope.communicationToolItems[index].Type);
                        communicationToolItems[tempIndex].Value = communicationToolItems[tempIndex].Value + ',' + $scope.communicationToolItems[index].Value;
                    }
                    else {
                        communicationToolItems.push($scope.communicationToolItems[index]);
                    }
                }
                
                var criteria = {
                    Username: $scope.insertParams.Username,
                    Role: $scope.insertParams.Role,
                    Remark: $scope.insertParams.Remark,
                    CommunicationTools: communicationToolItems,
                    Language: $scope.insertParams.Language,
                    Country: $scope.insertParams.Country
                };

                if (!$scope.validateParams(criteria)) {
                    $scope.unblockUI();
                    return;
                }

                $insertPicService.savePic(criteria)
                    .then(function (data) {
                        $scope.showInsertResult(data.Result);                       
                    }, function () {
                        $scope.unblockUI();
                    });
            };

            $scope.validateParams = function (params) {
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

                $scope.messageResultOptions = new MessageResult(message, !result, result);
                return result;
            }

            $scope.showInsertResult = function (result) {
                $scope.unblockUI();

                if (result.Code !== 0) {
                    $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                    return;
                }

                $scope.messageResultOptions = new MessageResult(result.Message, true, true);
                parent.homeManager.CachedData.shouldReloadView = true;
                setTimeout(function () {
                    $scope.messageResultOptions = new MessageResult('', false, false);                    
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            };

            $scope.cancelPic = function () {
                $scope.updateChangeFlag();
                parent.ClosePopup();
            };        

            $scope.addCommunicationTool = function () {
                var item = { Type: -1, Value: "" };
                $scope.communicationToolItems.push(item);
            };

            $scope.removeCommunicationTool = function (item) {
                var index = $scope.communicationToolItems.indexOf(item.x);
                if (index > -1) {
                    $scope.communicationToolItems.splice(index, 1);
                }
            };

            $scope.completeSetFocus = function () {
                $scope.defaultFocusOptions.IsSetFocus = false;
            };

            $scope.updateChangeFlag = function () {
                
                if ($scope.isParamsChange()) {
                    parent.homeManager.CachedData.isChangingData = true;
                    parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.ConfirmCancel;
                } else {
                    parent.homeManager.CachedData.isChangingData = false;
                }

                if (!$scope.$$phase) {
                    $scope.$apply();
                }
            };

            $scope.isParamsChange = function () {
                var result = false;
                var params = {
                    Username: $scope.insertParams.Username,
                    Role: $scope.insertParams.Role,
                    Language: $scope.insertParams.Language,
                    Country: $scope.insertParams.Country,
                    Remark: $scope.insertParams.Remark,
                    CommunicationTools: $scope.communicationToolItems
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

            $scope.ignoreChanges = function () {
                parent.homeManager.CachedData.isChangingData = false;
                $scope.$apply();
                parent.ClosePopup();
            };
            
            $scope.setFocus = function () {
                parent.homeManager.CachedData.isChangingData = false;
                $scope.isSetFocus = true;
                $scope.$apply();
            };

            $scope.unblockUI = function () {
                try {
                    if (parent && parent.$.unblockUI) {
                        parent.$.unblockUI();
                    }
                } catch (e) {
                }

                hotKeyManager.CachedData.isBlockingUI = false;
            };

            $scope.BlockUI = function () {
                hotKeyManager.CachedData.isBlockingUI = true;
                try {
                    if (parent && parent.BlockUI) {
                        parent.BlockUI();
                    }
                } catch (e) {
                }
            };

            $scope.invokeHotKey_CtrlS = function () {
                $scope.savePic();
                $scope.$apply();
            };

            angular.element(document).ready(function () {              
                $scope.initController();

                hotKeyManager.CachedData.ActiveDocumentManager.push({
                    Value: $scope,
                    Id: 'PicInsert'
                });
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            });
        }]);
})();