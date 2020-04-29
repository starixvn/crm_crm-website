(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').controller('viewListController', viewListController);
    viewListController.$inject = ['$scope', '$window', '$uiBlockerService', '$uibModal', 'admintoolSettingFactory'];

    function viewListController($scope, $window, $uiBlockerService, $uibModal, admintoolSettingFactory) {
        $scope.messageResultOptions = null;
        $scope.mainSettingOptions = null;
        $scope.isSetFocus = false;

        $scope.initDefaultOption = function () {
            $scope.mainSettingOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedMainSetting,
                AllText: viewResources.All
            };
        };

        $scope.updateSelectedMainSetting = function (mainSetting) {
            var isChange = !$scope.getSettingRelationsParams.MainSetting ||
                $scope.getSettingRelationsParams.MainSetting.SettingType !== mainSetting.SettingType;
            $scope.getSettingRelationsParams.MainSetting = mainSetting;

            $scope.safeApply();
            if (isChange) {
                $scope.getSettingRelations();
            }
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };

        $scope.sharedScriptManager = sharedScriptManager;
        $scope.getSettingRelationsParams = {
            MainSetting: null
        };
        $scope.settingManagementView = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getMainSetting();
        };
        $scope.getMainSetting = function () {
            $uiBlockerService.blockUI();
            var params = {};
            admintoolSettingFactory.getMainSetting(params)
                .then(function (data) {
                    $scope.mainSettingOptions.DataList = data.Result;
                    $scope.completedLoadFilterInfo();
                },
                function () {
                    $scope.completedLoadFilterInfo();
                });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.updateSelectedMainSetting($scope.mainSettingOptions.DataList[0]);
        };
        $scope.getSettingRelations = function () {
            $uiBlockerService.blockUI();

            admintoolSettingFactory.getSettingRelations($scope.getSettingRelationsParams)
                .then(function (data) {
                    $scope.settingManagementView = data.Result;
                    $uiBlockerService.unblockUI();
                },
                    function () {
                        $scope.settingManagementView = null;
                        $uiBlockerService.unblockUI();
                    });
        };
        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };

        $scope.insertNewSetting = function () {
            parent.homeManager.CachedData.shouldReloadView = false;
            parent.OpenPopup(
                window.rootUrl + "SettingManagement/OpenInsertNewSettingView",
                viewResources.Insert_New_Setting,
                480,
                260,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeInsertView,
                { my: "center top+200", at: "center top" },
                $scope.beforeCloseInsertView);
        };

        $scope.deleteSetting = function (settingInfo) {
            parent.homeManager.CachedData.shouldReloadView = true;
            var title = settingInfo.IsActive === true ? viewResources.DeactiveSetting : viewResources.ActiveSetting;
            parent.OpenPopup(
                window.rootUrl + "SettingManagement/OpenDeleteSettingView?" +
                $.param({
                    settingId: settingInfo.Id,
                    settingType: settingInfo.SettingType,
                    settingValue: settingInfo.Value,
                    isActive: settingInfo.IsActive
                }),
                title,
                380,
                150,
                true,
                false,
                parent.window,
                3000,
                null,
                $scope.closeDeleteView,
                { my: "center top+200", at: "center top" },
                $scope.beforeCloseDeleteView);
        };

        $scope.insertSubSetting = function () {
            $uibModal.open({
                template: angular.element('#addmintool-settingmanagement-insertsubsetting-template').html(),
                controller: 'insertSubSettingController',
                backdrop: 'static',
                backdropClass: 'create-subsetting-modal-backdrop',
                keyboard: false,
                size: 'sm',
                windowClass: 'create-subsetting-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    settingType: () => $scope.getSettingRelationsParams.MainSetting.SettingType
                }
            }).result.then(function (isSuccessful) {
                if (isSuccessful) {
                    $scope.getSettingRelations();
                }
            });
        };

        $scope.editSetting = function (settingInfo) {
            $uibModal.open({
                template: angular.element('#admintool-settingmanagement-editsetting-template').html(),
                controller: 'editSettingController',
                controllerAs: 'editSettingController',
                backdrop: 'static',
                backdropClass: 'create-subsetting-modal-backdrop',
                keyboard: false,
                size: 'sm',
                windowClass: 'create-subsetting-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    settingInfo: () => settingInfo
                }
            }).result.then(function (isSuccessful) {
                if (isSuccessful) {
                    $scope.getSettingRelations();
                }
            });
        }

        $scope.closeInsertView = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.$apply();
            $scope.getSettingRelations();
        };
        $scope.beforeCloseDeleteView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges, popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.closeDeleteView = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.$apply();
            $scope.getSettingRelations();
        };
        $scope.beforeCloseEditView = function () {
            if (parent.homeManager.CachedData.isChangingDataPopUp &&
                parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
                var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
                parent.homeManager.CachedData.isIgnoreChangePopUp = false;
                parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges, popUpController.setFocus);
                return false;
            }

            return true;
        };
        $scope.closeEditView = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
            if (!parent.homeManager.CachedData.shouldReloadView) {
                return;
            }

            $scope.$apply();
            $scope.getSettingRelations();
        };

        $scope.openLicenseeMappingPopup = function () {
            $uibModal.open({
                template: angular.element('#licensee-mapping-template').html(),
                controller: 'LicenseeMappingController',
                controllerAs: 'licenseeMappingCtrl',
                backdrop: 'static',
                backdropClass: 'licensee-mapping-modal-backdrop',
                keyboard: false,
                size: 'lg',
                windowClass: 'licensee-mapping-modal',
                appendTo: $window.parent.$('body')
            });
        };

        angular.element(document).ready(function () {
            $scope.initDefaultOption();
            $scope.initController();
            $scope.setFocus();
            const style = $('head link[href*="management.setting.css"]').clone();
            $window.parent.$('head').append(style);
        });

        $(document).keydown(function (eventObject) {
            if (eventObject.ctrlKey) {
                switch (eventObject.keyCode) {
                    case 83: // s
                        $scope.getSettingRelations();
                        eventObject.preventDefault();
                        return false;
                }
            }
        });
    }
}) (angular);