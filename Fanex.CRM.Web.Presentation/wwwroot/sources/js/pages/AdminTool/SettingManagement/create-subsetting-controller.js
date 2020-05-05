(function () {
    "use strict";

    angular.module('addmintool-settingmanagement-application').controller('insertSubSettingController',
        ['$scope', '$window', '$uiBlockerService', 'admintoolSettingFactory', '$uibModalInstance', 'settingType', 'sourceSettingMapping',
            function ($scope, $window, $uiBlockerService, admintoolSettingFactory, $uibModalInstance, settingType, sourceSettingMapping) {
                $scope.sharedScriptManager = sharedScriptManager;
                $scope.sourceSettingMapping = [];
                $scope.selectedMappings = [];
                $scope.insertSettingParams = {
                    NewSetting: {
                        ParentId: 0,
                        SettingType: settingType,
                        Value: '',
                        DisplayOrder: '',
                        SettingSource: '',
                    }
                };

                $scope.dropdownData = {
                    idMember: 'SettingSource',
                    nameMember: 'SettingSource',
                    sourceData: sourceSettingMapping,
                    checkedAll: false
                };

                $scope.textSettings = {
                    Filter: 'Search',
                    NoData: 'No data',
                    NoItemChecked: '',
                    AllItemsSelected: 'All',
                    CheckAll: 'All'
                };

                $scope.closeModal = function () {
                    $uibModalInstance.close();
                    $($window.parent.document).off('keydown');
                };

                $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
                $scope.initController = function () {
                    sharedScriptManager.safeApply($scope);
                    $scope.initDefaultOption();
                    $scope.getParentSetting();
                };
                $scope.getParentSetting = function () {
                    $uiBlockerService.blockUI();
                    var params = {};
                    admintoolSettingFactory.getParentSetting(params)
                        .then(function (data) {
                            $scope.parentSettingOptions.DataList = data.Result;
                            $scope.completedLoadFilterInfo();
                        },
                            function () {
                                $scope.completedLoadFilterInfo();
                            });
                };

                $scope.completedLoadFilterInfo = function () {
                    $scope.updateSelectedParentSetting({
                        Id: crmConstants.Enums.Setting.NotApply,
                        SettingType: viewResources.Symbol_NotApply,
                        Value: viewResources.Symbol_NotApply
                    });
                    $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
                    $uiBlockerService.unblockUI();
                };
                $scope.setFocus = function () {
                    $scope.isSetFocus = true;
                    $scope.safeApply();
                };
                $scope.completeSetFocus = function () {
                    $scope.isSetFocus = false;
                };

                $scope.insertSubSetting = function () {
                    $uiBlockerService.blockUI();
                    if (!$scope.checkInsertParam($scope.insertSettingParams)) {
                        $uiBlockerService.unblockUI();
                        return;
                    }
                    if ($scope.selectedMappings.length > 0) {
                        $scope.insertSettingParams.NewSetting.SettingSource =
                            $scope.selectedMappings.map(mapping => mapping.SettingSource).join(',');
                    }

                    admintoolSettingFactory.insertSubSetting($scope.insertSettingParams)
                        .then(function (data) {
                            $scope.showInsertResult(data.Result);

                            if (data.Result.IsSuccessful) {
                                setTimeout(function () {
                                    $uibModalInstance.close(true);
                                    $($window.parent.document).off('keydown');
                                }, sharedScriptManager.cachedData.alertSuccessTime);
                            }

                            $uiBlockerService.unblockUI();
                        },
                            function () {
                                $uiBlockerService.unblockUI();
                            });
                };
                $scope.checkInsertParam = function (insertParam) {
                    if (insertParam.NewSetting.SettingType === "") {
                        $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingName, true, false);
                        return false;
                    }

                    if (insertParam.NewSetting.Value === "") {
                        $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingValue, true, false);
                        return false;
                    }

                    if (insertParam.NewSetting.SettingType.length > 100) {
                        $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedName, true, false);
                        return false;
                    }

                    if (insertParam.NewSetting.Value.length > 100) {
                        $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedValue, true, false);
                        return false;
                    }

                    if (insertParam.NewSetting.DisplayOrder > 2100000000) {
                        $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_DisplayOrder, true, false);
                        return false;
                    }

                    $scope.messageResultOptions = new MessageResult("", false, false);
                    return true;
                };
                $scope.showInsertResult = function (result) {
                    if (result.Code !== 0) {
                        $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                        return;
                    }

                    $scope.messageResultOptions = new MessageResult(result.Message, true, true);
                    parent.homeManager.CachedData.shouldReloadView = true;
                    $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
                    setTimeout(function () {
                        $scope.messageResultOptions = new MessageResult('', false, false);
                        parent.ClosePopup();
                    }, sharedScriptManager.cachedData.alertSuccessTime);
                };

                $scope.isShowSourceMapping = function () {
                    return settingType === 'BetType' || settingType === 'Product' || settingType === 'SubProduct';
                }

                $($window.parent.document).off('keydown').on('keydown', function (eventObject) {
                    if (eventObject.ctrlKey) {
                        if (eventObject.keyCode === 83) { // s
                            $scope.insertSubSetting();
                            $scope.$apply();
                            eventObject.preventDefault();
                            return false;
                        }
                    }
                });
            }]);
})();