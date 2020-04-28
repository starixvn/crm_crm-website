(function () {
    "use strict";
    /* global sharedScriptManager,
    viewResources,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions,
    MessageResult,
    crmConstants,
    settingInfo */
    /* jshint camelcase: false */

    angular.module('addmintool-settingmanagement-application').controller('insertSubSettingController',
        ['$scope', '$uiBlockerService', 'insertSubSettingService', '$uibModalInstance', 'settingType',
        function ($scope, $uiBlockerService, insertSubSettingService, $uibModalInstance, settingType) {
            $scope.sharedScriptManager = sharedScriptManager;
            $scope.sourceSettingMapping = [];
            $scope.selectedMappings = [];
            $scope.insertSettingParams = {
                NewSetting: {
                    ParentId: 0,
                    SettingType: settingType,
                    Value: '',
                    DisplayOrder: '',
                    SettingSource : '',
                }
            };

            $scope.dropdownData = {
                idMember: 'SettingSource',
                nameMember: 'SettingSource',
                sourceData: $scope.sourceSettingMapping,
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
            };

            $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
            $scope.initController = function () {
                sharedScriptManager.safeApply($scope);
                $scope.initDefaultOption();
                $scope.getParentSetting();
                if ($scope.isShowSourceMapping) {
                    $scope.getSourceSettingMapping();
                }
            };
            $scope.getParentSetting = function () {
                $uiBlockerService.blockUI();
                var params = { };
                insertSubSettingService.getParentSetting(params)
                    .then(function (data) {
                        $scope.parentSettingOptions.DataList = data.Result;
                        $scope.completedLoadFilterInfo();
                    },
                    function () {
                        $scope.completedLoadFilterInfo();
                    });
            };
            $scope.getSourceSettingMapping = function () {
                $uiBlockerService.blockUI();
                var params = { Settings: settingType };
                insertSubSettingService.getSourceSettingMapping(params)
                    .then(function (data) {
                        $scope.sourceSettingMapping = data.Result;

                        $scope.dropdownData = {
                            idMember: 'SettingSource',
                            nameMember: 'SettingSource',
                            sourceData: data.Result,
                            checkedAll: false
                        };

                        $scope.completedLoadFilterInfo();
                    },
                        function () {
                            $scope.completedLoadFilterInfo();
                        });
            }
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

                insertSubSettingService.insertSubSetting($scope.insertSettingParams)
                .then(function (data) {
                    $scope.showInsertResult(data.Result);
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
            $scope.updateChangeFlag = function () {
                if ($scope.oldInsertSettingParams.NewSetting &&
                    ($scope.oldInsertSettingParams.NewSetting.SettingType !== $scope.insertSettingParams.NewSetting.SettingType ||
                    $scope.oldInsertSettingParams.NewSetting.Value !== $scope.insertSettingParams.NewSetting.Value ||
                    $scope.oldInsertSettingParams.NewSetting.DisplayOrder !== $scope.insertSettingParams.NewSetting.DisplayOrder ||
                    $scope.oldInsertSettingParams.NewSetting.ParentId !== $scope.insertSettingParams.NewSetting.ParentId)) {
                    parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_InsertSetting_Changes;
                    parent.homeManager.CachedData.isChangingDataPopUp = true;
                    return;
                }

                parent.homeManager.CachedData.isChangingDataPopUp = false;
            };
            $scope.ignoreChanges = function () {
                $scope.oldInsertSettingParams = angular.copy($scope.insertSettingParams);
                $scope.$apply();
                $scope.updateChangeFlag();
                parent.ClosePopup();
                    };

            $scope.isShowSourceMapping = function () {
                return settingType === 'BetType' || settingType === 'Product' || settingType === 'SubProduct';
            }

            angular.element(document).ready(function () {
                registerOptions($scope);
                registerUpdateFunctions($scope);
                //registerHotkeyFunctions($scope);
                $scope.initController();
                //$scope.registerHotKey();
                $scope.setFocus();
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
                $scope.$watch(function ($scope) {
                    $scope.updateChangeFlag();
                });
            });
    } ]);
})();