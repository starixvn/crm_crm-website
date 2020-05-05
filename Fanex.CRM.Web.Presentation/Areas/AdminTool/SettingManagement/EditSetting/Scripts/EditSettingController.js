(function () {
    "use strict";
    /* global sharedScriptManager,
    viewResources,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions,
    MessageResult,
    settingInfo */
    /* jshint camelcase: false */

    angular.module('addmintool-settingmanagement-application').controller('editSettingController',
        ['$scope', '$window', '$uiBlockerService', 'editSettingService', '$uibModalInstance', 'settingInfo', 'sourceSettingMapping',
            function ($scope, $window, $uiBlockerService, editSettingService, $uibModalInstance, settingInfo, sourceSettingMapping) {
            $scope.messageResultOptions = null;
            $scope.isSetFocus = false;
            $scope.sourceSettingMapping = sourceSettingMapping;

            $scope.dropdownData = {
                idMember: 'SettingSource',
                nameMember: 'SettingSource',
                sourceData: $scope.sourceSettingMapping,
                checkedAll: true
            };

            $scope.textSettings = {
                Filter: 'Search',
                NoData: 'No data',
                NoItemChecked: '',
                AllItemsSelected: 'All',
                CheckAll: 'All'
            };

            $scope.sharedScriptManager = sharedScriptManager;

            $scope.editSettingParams = {
                OldSetting: angular.copy(settingInfo),
                UpdatedSetting: angular.copy(settingInfo)
            };
            $scope.oldEditSettingParams = angular.copy($scope.editSettingParams);
            $scope.initController = function () {
                sharedScriptManager.safeApply($scope);

                if ($scope.isShowSourceMapping) {
                    $scope.getSourceSettingMapping();
                }
            };

            $scope.setFocus = function () {
                $scope.isSetFocus = true;
                $scope.safeApply();
            };
            $scope.editSetting = function () {
                $uiBlockerService.blockUI();
                if (!$scope.checkEditParam($scope.editSettingParams)) {
                    $uiBlockerService.unblockUI();
                    return;
                }

                if ($scope.isShowSourceMapping) {
                    $scope.editSettingParams.UpdatedSetting.SettingSource =
                        $scope.selectedMappings.map(mapping => mapping.SettingSource).join(',');
                }
                
                editSettingService.editSetting($scope.editSettingParams)
                    .then(function (data) {
                        $scope.showEditResult(data.Result);

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
            $scope.checkEditParam = function (editParam) {
                if (editParam.UpdatedSetting.SettingType === "") {
                    $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingName, true, false);
                    return false;
                }

                if (editParam.UpdatedSetting.Value === "") {
                    $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_EmptySettingValue, true, false);
                    return false;
                }

                if (editParam.UpdatedSetting.SettingType.length > 100) {
                    $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedName, true, false);
                    return false;
                }

                if (editParam.UpdatedSetting.Value.length > 100) {
                    $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_ExceedValue, true, false);
                    return false;
                }

                if (editParam.UpdatedSetting.DisplayOrder > 2100000000) {
                    $scope.messageResultOptions = new MessageResult(viewResources.SettingManagement_Error_DisplayOrder, true, false);
                    return false;
                }

                $scope.messageResultOptions = new MessageResult("", false, false);
                return true;
            };
            $scope.showEditResult = function (result) {
                if (result.Code !== 0) {
                    $scope.messageResultOptions = new MessageResult(result.Message, true, false);
                    return;
                }

                $scope.messageResultOptions = new MessageResult(result.Message, true, true);
                parent.homeManager.CachedData.shouldReloadView = true;
                $scope.oldEditSettingParams = angular.copy($scope.editSettingParams);
                setTimeout(function () {
                    $scope.messageResultOptions = new MessageResult('', false, false);
                    parent.ClosePopup();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            };
            $scope.updateChangeFlag = function () {
                if ($scope.oldEditSettingParams.UpdatedSetting &&
                    ($scope.oldEditSettingParams.UpdatedSetting.SettingType !== $scope.editSettingParams.UpdatedSetting.SettingType ||
                    $scope.oldEditSettingParams.UpdatedSetting.Value !== $scope.editSettingParams.UpdatedSetting.Value ||
                    $scope.oldEditSettingParams.UpdatedSetting.DisplayOrder !== $scope.editSettingParams.UpdatedSetting.DisplayOrder ||
                    $scope.oldEditSettingParams.UpdatedSetting.ParentId !== $scope.editSettingParams.UpdatedSetting.ParentId)) {
                    parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_EditSetting_Changes;
                    parent.homeManager.CachedData.isChangingDataPopUp = true;
                    return;
                }

                parent.homeManager.CachedData.isChangingDataPopUp = false;
            };
            $scope.ignoreChanges = function () {
                $scope.oldEditSettingParams = angular.copy($scope.editSettingParams);
                $scope.$apply();
                $scope.updateChangeFlag();
                parent.ClosePopup();
            };
                $scope.getSourceSettingMapping = function () {
                var selectedItems = settingInfo.SettingSource.split(', ');
                var data = angular.copy(sourceSettingMapping);

                selectedItems.forEach(item => {
                    if (item !== "") {
                        var selectedItem = {
                            checked: true,
                            SettingName: "",
                            SettingSource: item
                        }

                        data.unshift(selectedItem);
                    }
                });


                $scope.dropdownData = {
                    idMember: 'SettingSource',
                    nameMember: 'SettingSource',
                    sourceData: data,
                    checkedAll: false
                };
            }

            $scope.closeModal = function () {
                $uibModalInstance.close();
                $($window.parent.document).off('keydown');
            };

            $scope.isShowSourceMapping = function () {
                var settingType = $scope.editSettingParams.UpdatedSetting.SettingType;
                return settingType === 'BetType' || settingType === 'Product' || settingType === 'SubProduct';
                }

            angular.element(document).ready(function () {
                registerUpdateFunctions($scope);
                $scope.initController();
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
                $scope.$watch(function ($scope) {
                    $scope.updateChangeFlag();
                });
            });

            $($window.parent.document).off('keydown').on('keydown', function (eventObject) {
                if (eventObject.ctrlKey) {
                    if (eventObject.keyCode === 83) { // s
                        $scope.editSetting();
                        $scope.$apply();
                        eventObject.preventDefault();
                        return false;
                    }
                }
            });
        } ]);
})();