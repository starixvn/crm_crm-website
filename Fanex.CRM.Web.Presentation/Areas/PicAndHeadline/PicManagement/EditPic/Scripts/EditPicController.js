(function () {
    "use strict";
    /* global
    viewResources,
    crmConstants,
    currentDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('editpic-application').controller('editPicController', ['$scope', '$editPicService', '$compile',
        function ($scope, $editPicService, $compile) {
            $scope.errorMessage = '';
            $scope.roleOptions = null;
            $scope.communicationToolOptions = null;
            $scope.languageOptions = null;
            $scope.countryOptions = null;

            $scope.insertParams = {
                PicId: item.PicId,
                Username: item.Username,
                Role: item.Role,
                Remark: item.Remark,
                CommunicationTools: item.CommunicationTools,
                Language: item.Language,
                Country: item.Country
            };

            $scope.SelectedLanguage = item.Language;

            $scope.communicationToolItems = item.CommunicationTools;

            $scope.oldParams = {
                PicId: item.PicId,
                Username: item.Username,
                Role: item.Role,
                Language: item.Language,
                Country: item.Country,
                Remark: item.Remark,
                CommunicationTools: item.CommunicationTools.map(a => Object.assign({}, a))
            }            
            
            $scope.initController = function () {
                $scope.initPicker();

                $scope.getAllSetting();
                $scope.defaultFocusOptions = {
                    IsSetFocus: false,
                    OnChangeEvent: $scope.completeSetFocus
                };

                $scope.defaultFocusOptions.IsSetFocus = true;
                var iframe = parent.document.getElementById('innerFrame');
                parent.document.getElementById('dialog-modal').style.maxHeight = '450px';
                parent.document.getElementById('dialog-modal').style.overflowY =  'auto';
                parent.document.getElementById('dialog-modal').style.marginTop =  '10px';
                iframe.style.height = `${iframe.contentWindow.document.getElementsByClassName('main')[0].clientHeight}px`;
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
                $editPicService.getAllSetting()
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
                    PicId: $scope.insertParams.PicId,
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
                
                $editPicService.savePic(criteria)
                    .then(function (data) {
                        $scope.showInsertResult(data.Result);
                        $scope.unblockUI();
                        //parent.ClosePopup();
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
                if (result.Code !== 0) {
                    $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                    return;
                }

                $scope.messageResultOptions = new MessageResult(result.Message, true, true);
                parent.homeManager.CachedData.shouldReloadView = true;
                //$scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
                setTimeout(function () {
                    $scope.messageResultOptions = new MessageResult('', false, false);
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            };

            $scope.cancelPic = function () {
                $scope.updateChangeFlag();
                parent.ClosePopup();
            }        

            $scope.addCommunicationTool = function () {
                var item = {Type: "", Value: "" };
                $scope.communicationToolItems.push(item);
                var iframe = parent.document.getElementById('innerFrame');
                iframe.style.height = `${iframe.contentWindow.document.getElementsByClassName('main')[0].clientHeight + 36}px`;
            }

            $scope.removeCommunicationTool = function (item) {
                var index = $scope.communicationToolItems.indexOf(item.x);
                if (index > -1) {
                    $scope.communicationToolItems.splice(index, 1);
                }
                var iframe = parent.document.getElementById('innerFrame');
                iframe.style.height = `${iframe.contentWindow.document.getElementsByClassName('main')[0].clientHeight - 36}px`;
            }

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

                if (params.Username != $scope.oldParams.Username ||
                    params.Role != $scope.oldParams.Role ||
                    params.Language != $scope.oldParams.Language ||
                    params.Country != $scope.oldParams.Country ||
                    params.Remark != $scope.oldParams.Remark ||
                    $scope.isCommunicationToolChanged($scope.oldParams.CommunicationTools, params.CommunicationTools))
                {
                    result = true;
                }               

                return result;
            };

            $scope.isCommunicationToolChanged = function (oldTools, newTools) {
                if (oldTools.length != newTools.length) {
                    return true;
                }

                for (var i = 0; i < oldTools.length; i++) {
                    if (oldTools[i].Type != newTools[i].Type || oldTools[i].Value != newTools[i].Value) {
                        return true;
                    }
                }

                return false;
            } 

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
            };

            $scope.BlockUI = function () {
                try {
                    if (parent && parent.BlockUI) {
                        parent.BlockUI();
                    }
                } catch (e) {
                }
            };

            angular.element(document).ready(function () {
                $scope.initController();
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
            });

            $(document).keydown(function (eventObject) {
                if (eventObject.ctrlKey) {
                    switch (eventObject.keyCode) {
                        case 83: // s
                            $scope.savePic();
                            $scope.$apply();
                            eventObject.preventDefault();
                            return false;
                    }
                }
            });
        }]);
})();