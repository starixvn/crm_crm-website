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

            $scope.insertParams = {
                Username: '',
                RoleId: null,
                Remark: '',
                CommunicationTools: []
            };

            $scope.communicationToolItems = [
                {type: "", value: ""}                
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
            };

            $scope.updateSelectedRole = function (item) {
                $scope.insertParams.RoleId = item.Id;
            }

            $scope.updateSelectedCommunicationTool = function (x, y) {
                y.type = x.Id;
                console.log(x);
                
                console.log(y);
            }

            $scope.getAllSetting = function () {
                $scope.BlockUI();
                $scope.isLoadedAllSite = false;
                $insertPicService.getAllSetting()
                    .then(function (data) {
                        var allSetting = data.Result;
                        $scope.roleOptions.DataList = allSetting.RoleList;
                        console.log(allSetting);
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
                console.log($scope.insertParams);
                console.log($scope.communicationToolItems);
                $scope.BlockUI();
                
                $scope.insertParams = {
                    Username: '',
                    RoleId: null,
                    Remark: '',
                    CommunicationTools: []
                };

                var criteria = {
                    Username: '',
                    RoleId: null,
                    Remark: '',
                    CommunicationTools: []
                };

                $insertPicService.savePic(criteria)
                    .then(function (data) {
                        console.log(data);
                        $scope.reportOptions.DataList = data.Result;
                        $scope.unblockUI();
                        parent.ClosePopup();
                    }, function () {
                        $scope.unblockUI();
                    });
            };

            $scope.cancelPic = function () {
                parent.ClosePopup();
            }        

            $scope.addCommunicationTool = function () {
                console.log("adding");
                //var itemNum = $scope.communicationToolItems.length + 1;
                ///var key = "item" + itemNum;
                var item = {type: "", value: "" };
                $scope.communicationToolItems.push(item);
                console.log($scope.communicationToolItems);
            }

            $scope.removeCommunicationTool = function (item) {
                console.log(item.x);
                console.log(item.x.$$hashKey);

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