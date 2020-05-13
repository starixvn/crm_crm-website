(function () {
    "use strict";
    /* global hotKeyManager,
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

            
            $scope.insertParams = {
                PicId: item.PicId,
                Username: item.Username,
                RoleId: item.RoleId,
                Remark: item.Remark,
                CommunicationTools: []
            };

            $scope.communicationToolItems = item.CommunicationTools;

            
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
            };

            $scope.updateSelectedRole = function (selectedItem) {
                $scope.insertParams.RoleId = selectedItem.Id;
            }

            $scope.updateSelectedCommunicationTool = function (target, selectedItem) {
                target.Type = selectedItem.Id;
            }

            $scope.getAllSetting = function () {
                $scope.BlockUI();
                $scope.isLoadedAllSite = false;
                $editPicService.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.roleOptions.DataList = allSetting.RoleList;
                        $scope.communicationToolOptions.DataList = allSetting.CommunicationTools;
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

                var criteria = {
                    PicId: $scope.insertParams.PicId,
                    Username: $scope.insertParams.Username,
                    RoleId: $scope.insertParams.RoleId,
                    Remark: $scope.insertParams.Remark,
                    CommunicationTools: $scope.communicationToolItems
                };
                
                $editPicService.savePic(criteria)
                    .then(function (data) {
                        $scope.showInsertResult(data.Result);
                        $scope.unblockUI();
                        //parent.ClosePopup();
                    }, function () {
                        $scope.unblockUI();
                    });
            };

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
                parent.ClosePopup();
            }        

            $scope.addCommunicationTool = function () {
                //var itemNum = $scope.communicationToolItems.length + 1;
                ///var key = "item" + itemNum;
                var item = {Type: "", Value: "" };
                $scope.communicationToolItems.push(item);
            }

            $scope.removeCommunicationTool = function (item) {

                var index = $scope.communicationToolItems.indexOf(item.x);
                if (index > -1) {
                    $scope.communicationToolItems.splice(index, 1);
                }
            }

            $scope.completeSetFocus = function () {
                $scope.defaultFocusOptions.IsSetFocus = false;
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

            angular.element(document).ready(function () {
                $scope.initController();
            });
        }]);
})();