(function () {
    "use strict";
    /* global sharedScriptManager,
    moment,
    crmConstants,
    viewResources,
    customerId,
    hotKeyManager,
    currentTime*/
    /* jshint camelcase: false */
    angular.module('edit-multiple-logs-application').controller('editMultipleLogsController', ['$scope', '$http', '$editMultipleLogsService',
    function ($scope, $http, $editMultipleLogsService) {
        $scope.multipleLogs = multipleLogs;
        $scope.oldmultipleLogs = angular.copy(multipleLogs);
        $scope.extensionList = null;
        $scope.countryList = null;
        $scope.communicationToolList = null;
        $scope.languageList = null;
        $scope.mainCategoryList = null;
        $scope.subCategoryList = null;
        $scope.betTypeList = null;
        $scope.statusList = null;
        $scope.siteList = null;
        $scope.roleList = null;
        $scope.responseList = null;
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.lastRemarkPrefix = '';
        $scope.isSuccess = false;
        $scope.isShowResultMessage = false;
        $scope.resultMessage = '';
        $scope.isSetFocus = false;
        $scope.isShowRemarkBox = true;
        $scope.isShowSolutionBox = true;
        $scope.licenseeTypes = [];

        $scope.listOfCustomers = '';
        $scope.feedbackList = '';

        $scope.initController = function () {
            $scope.getSettings();
        };
        $scope.initSelectedLogInfo = function () {
            var i;
            if ($scope.mainCategoryList != null) {
                var mainCategoryListLength = $scope.mainCategoryList.length;
                for (i = 0; i < mainCategoryListLength; i++) {
                    var mainCategory = $scope.mainCategoryList[i];
                    if (mainCategory.Id === multipleLogs.MainCategory.Id) {
                        $scope.multipleLogs.MainCategory = mainCategory;
                        $scope.subCategoryList = mainCategory.ChildSettings;
                        break;
                    }
                }
            }

            if ($scope.subCategoryList != null) {
                var subCategoryListLength = $scope.subCategoryList.length;
                for (i = 0; i < subCategoryListLength; i++) {
                    var subCategory = $scope.subCategoryList[i];
                    if (subCategory.Id === multipleLogs.SubCategory.Id) {
                        $scope.multipleLogs.SubCategory = subCategory;
                        break;
                    }
                }
            }

            if ($scope.communicationToolList != null) {
                var communicationToolListLength = $scope.communicationToolList.length;
                for (i = 0; i < communicationToolListLength; i++) {
                    var communicationTool = $scope.communicationToolList[i];
                    if (communicationTool.Id === multipleLogs.CommunicationTool.Id) {
                        $scope.multipleLogs.CommunicationTool = communicationTool;
                        break;
                    }
                }
            }

            $scope.subProductOptions.DataList = SettingManager.getChildSettings($scope.multipleLogs.Product.Id, $scope.productOptions.DataList);
            $scope.betTypeList = SettingManager.getChildSettings($scope.multipleLogs.SubProduct.Id, $scope.subProductOptions.DataList);
            $scope.listOfCustomers = $scope.getListOfCustomers(parent.homeManager.CachedData.feedbacks);
            $scope.feedbackList = $scope.getFeedbackIdList(parent.homeManager.CachedData.feedbacks);
            parent.homeManager.CachedData.feedbacks = null;
        };
        $scope.getListOfCustomers = function (feedbacks) {
            var customerList = [];
            angular.forEach(feedbacks, function (feedback) {
                this.push(feedback.UserName);
            }, customerList);

            return customerList.join(', ');
        };
        $scope.getFeedbackIdList = function (feedbacks) {
            var feedbackIds = [];
            angular.forEach(feedbacks, function (feedback) {
                this.push(feedback.FeedbackId);
            }, feedbackIds);

            return feedbackIds;
        };
        $scope.generateRemarkPrefix = function () {
            /* jshint camelcase: false */
            return sharedScriptManager.stringFormat("{0}/{1} ",
            [moment($scope.multipleLogs.StartTime).format(crmConstants.Enums.FormatConstant.StartTime_RemarkPrefix),
            $scope.multipleLogs.CommunicationTool.Name]);
        };
        $scope.updateRemarkPrefix = function () {
            var remarkPrefix = $scope.generateRemarkPrefix();
            if ($scope.multipleLogs.Remark.indexOf($scope.lastRemarkPrefix) === 0) {
                $scope.multipleLogs.Remark = $scope.multipleLogs.Remark.substring(
                    $scope.lastRemarkPrefix.length,
                    $scope.multipleLogs.Remark.length);
            }

            if ($scope.multipleLogs.Remark.indexOf(remarkPrefix) === -1) {
                $scope.multipleLogs.Remark = remarkPrefix + $scope.multipleLogs.Remark;
                $scope.lastRemarkPrefix = remarkPrefix;
            }
        };
        $scope.onStartTimeChanged = function () {
            $scope.$apply(function () {
                $scope.updateRemarkPrefix();
            });
        };
        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlS = function () {
            $scope.editMultipleLogs();
            $scope.$apply();
        };
        /* jshint camelcase: false */
        $scope.updateSelectedProduct = function (product) {
            $scope.multipleLogs.Product = product;

            $scope.subProductOptions.DataList = product.ChildSettings;
            $scope.updateSelectedSubProduct($scope.subProductOptions.DataList[0]);
        };
        $scope.updateSelectedSubProduct = function (subProduct) {
            $scope.multipleLogs.SubProduct = subProduct;
            $scope.betTypeList = subProduct.ChildSettings;
            $scope.multipleLogs.BetType = $scope.betTypeList[0];
            $scope.safeApply();
            $scope.updateChangeFlag();
        };
        $scope.updateStartTime = function (selectedTime) {
            $scope.multipleLogs.StartTime = selectedTime;
            if (new Date($scope.multipleLogs.EndTime) <= selectedTime) {
                $scope.multipleLogs.EndTime = new Date(selectedTime);
                $scope.multipleLogs.EndTime.setMinutes(selectedTime.getMinutes() + 1);
            }

            $scope.updateRemarkPrefix();
            $scope.updateChangeFlag();
        };
        $scope.updateEndTime = function (selectedTime) {
            $scope.multipleLogs.EndTime = selectedTime;
            if (selectedTime <= new Date($scope.multipleLogs.StartTime)) {
                $scope.multipleLogs.StartTime = new Date(selectedTime);
                $scope.multipleLogs.StartTime.setMinutes(selectedTime.getMinutes() - 1);
            }

            $scope.updateRemarkPrefix();
            $scope.updateChangeFlag();
        };
        $scope.updateSelectedExtension = function (extentionId) {
            extentionId = parseInt(extentionId);
            var extensionInfo;
            var extensionListLength = $scope.extensionList.length;
            for (var i = 0; i < extensionListLength; i++) {
                var extension = $scope.extensionList[i];
                if (extension.Id === extentionId) {
                    extensionInfo = extension;
                    break;
                }
            }

            if (extensionInfo) {
                $scope.multipleLogs.Extension = extensionInfo;
                $scope.$apply();
                $scope.updateChangeFlag();
            }
        };
        $scope.updateSelectedCountry = function (countryInfoId) {
            countryInfoId = parseInt(countryInfoId);
            var countryInfo;
            var countryListLength = $scope.countryList.length;
            for (var i = 0; i < countryListLength; i++) {
                var country = $scope.countryList[i];
                if (country.Id === countryInfoId) {
                    countryInfo = country;
                    break;
                }
            }

            if (countryInfo) {
                $scope.multipleLogs.Country = countryInfo;
            } else {
                $scope.multipleLogs.Country = {
                    Id: crmConstants.Enums.Country.NotApply
                };
            }

            $scope.$apply();
            $scope.updateChangeFlag();
        };
        $scope.updateSelectedCommunicationTool = function (communicationToolId) {
            communicationToolId = parseInt(communicationToolId);
            var communicationToolInfo;
            var communicationToolListLength = $scope.communicationToolList.length;
            for (var i = 0; i < communicationToolListLength; i++) {
                var communicationTool = $scope.communicationToolList[i];
                if (communicationTool.Id === communicationToolId) {
                    communicationToolInfo = communicationTool;
                    break;
                }
            }

            if (communicationToolInfo) {
                $scope.multipleLogs.CommunicationTool = communicationToolInfo;
            }

            $scope.updateRemarkPrefix();
            $scope.$apply();
            $scope.updateChangeFlag();
        };
        $scope.updateSelectedLanguage = function (languageId) {
            $scope.multipleLogs.Language.Id = languageId;
            $scope.$apply();
            $scope.updateChangeFlag();
        };
        $scope.updateSelectedMainCategory = function (mainCategoryId) {
            mainCategoryId = parseInt(mainCategoryId);
            var mainCategoryInfo;
            var mainCategoryListLength = $scope.mainCategoryList.length;
            for (var i = 0; i < mainCategoryListLength; i++) {
                var mainCategory = $scope.mainCategoryList[i];
                if (mainCategory.Id === mainCategoryId) {
                    mainCategoryInfo = mainCategory;
                    break;
                }
            }

            if (mainCategoryInfo) {
                $scope.multipleLogs.MainCategory = mainCategoryInfo;
                $scope.subCategoryList = mainCategoryInfo.ChildSettings;
                $scope.multipleLogs.SubCategory = $scope.subCategoryList.length > 0 ?
                $scope.subCategoryList[0] :
                {
                    Id: 0
                };
                $scope.$apply();
                $scope.updateChangeFlag();
            }
        };
        $scope.updateSelectedSubCategory = function (subCategoryId) {
            subCategoryId = parseInt(subCategoryId);
            var subCategoryInfo;
            var subCategoryListLength = $scope.subCategoryList.length;
            for (var i = 0; i < subCategoryListLength; i++) {
                var subCategory = $scope.subCategoryList[i];
                if (subCategory.Id === subCategoryId) {
                    subCategoryInfo = subCategory;
                    break;
                }
            }

            if (subCategoryInfo) {
                $scope.multipleLogs.SubCategory = subCategoryInfo;
                $scope.$apply();
                $scope.updateChangeFlag();
            }
        };
        $scope.updateSelectedBetType = function (betTypeId) {
            betTypeId = parseInt(betTypeId);
            var betTypeInfo;
            var betTypeListLength = $scope.betTypeList.length;
            for (var i = 0; i < betTypeListLength; i++) {
                var betType = $scope.betTypeList[i];
                if (betType.Id === betTypeId) {
                    betTypeInfo = betType;
                    break;
                }
            }

            if (betTypeInfo) {
                $scope.multipleLogs.BetType = betTypeInfo;
                $scope.$apply();
                $scope.updateChangeFlag();
            }
        };
        $scope.updateSelectedSite = function (siteId) {
            $scope.multipleLogs.SiteInfo = $scope.multipleLogs.SiteInfo || { Id: 0 };
            $scope.updateChangeFlag();
        };
        $scope.updateSelectedStatus = function (statusId) {
            statusId = parseInt(statusId);
            var statusInfo;
            var statusListLength = $scope.statusList.length;
            for (var i = 0; i < statusListLength; i++) {
                var status = $scope.statusList[i];
                if (status.Id === statusId) {
                    statusInfo = status;
                    break;
                }
            }

            if (statusInfo) {
                $scope.multipleLogs.Status = statusInfo;
                $scope.$apply();
                $scope.updateChangeFlag();
            }
        };
        $scope.updateSelectedRole = function (roleId) {
            roleId = parseInt(roleId);
            var roleInfo;
            var roleListLength = $scope.roleList.length;
            for (var i = 0; i < roleListLength; i++) {
                var role = $scope.roleList[i];
                if (role.Id === roleId) {
                    roleInfo = role;
                    break;
                }
            }

            if (roleInfo) {
                $scope.multipleLogs.RoleInfo = roleInfo;
                $scope.$apply();
                $scope.updateChangeFlag();
            }
        };
        $scope.updateSelectedResponse = function (responseId) {
            responseId = parseInt(responseId);
            var responseInfo;
            var responseListLength = $scope.responseList.length;
            for (var i = 0; i < responseListLength; i++) {
                var response = $scope.responseList[i];
                if (response.Id === responseId) {
                    responseInfo = response;
                    break;
                }
            }

            if (responseInfo) {
                $scope.multipleLogs.Response = responseInfo;
                $scope.$apply();
                $scope.updateChangeFlag();
            }
        };
        $scope.updateSelectedSubject = function (subject) {
            $scope.multipleLogs.Subject = subject;
            $scope.updateChangeFlag();
        };
        $scope.updateRemark = function (remark) {
            $scope.multipleLogs.Remark = remark;
            $scope.updateChangeFlag();
        };
        $scope.updateSolution = function (solution) {
            $scope.multipleLogs.Solution = solution;
            $scope.updateChangeFlag();
        };
        $scope.updateSelectedLicenseeType = function (licenseeType) {
            $scope.multipleLogs.LicenseeType = licenseeType;
            $scope.safeApply();
            $scope.updateChangeFlag();
        };
        $scope.editMultipleLogs = function () {
            $scope.isShowResultMessage = false;
            $scope.isSuccess = true;
            var editCriteria = {
                ListFeedbackId: $scope.feedbackList.join(','),
                StartTime: moment(new Date($scope.multipleLogs.StartTime)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment),
                EndTime: moment(new Date($scope.multipleLogs.EndTime)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment),
                ExtensionId: $scope.multipleLogs.Extension.Id,
                CountryId: $scope.multipleLogs.Country.Id,
                CommunicationToolId: $scope.multipleLogs.CommunicationTool.Id,
                LanguageId: $scope.multipleLogs.Language.Id,
                MainCategoryId: $scope.multipleLogs.MainCategory.Id,
                SubcategoryId: $scope.multipleLogs.SubCategory.Id,
                ProductId: $scope.multipleLogs.Product.Id,
                SubProductId: $scope.multipleLogs.SubProduct.Id,
                StatusId: $scope.multipleLogs.Status.Id,
                BetTypeId: $scope.multipleLogs.BetType.Id,
                RoleId: $scope.multipleLogs.RoleInfo.Id,
                SiteId: $scope.multipleLogs.SiteInfo.Id,
                LicenseeTypeId: $scope.multipleLogs.LicenseeType.Id,
                ResponseId: $scope.multipleLogs.Response.Id,
                Subject: $scope.multipleLogs.Subject,
                Remark: $scope.multipleLogs.Remark,
                Solution: $scope.multipleLogs.Solution
            };
            if ($scope.validateEditMultipleLogsCriteria(editCriteria)) {
                $scope.BlockUI();
                $editMultipleLogsService.editMultipleLogs(editCriteria)
                .then(function (data) {
                    $scope.showResult(data.Result);
                    $scope.unblockUI();
                },
                function () {
                    $scope.unblockUI();
                });
            }
        };
        $scope.showResult = function (result) {
            $scope.resultMessage = result.Message;
            $scope.isShowResultMessage = true;
            $scope.unblockUI();

            if (result.Code !== 0) {
                $scope.isSuccess = false;
                return;
            }

            parent.homeManager.CachedData.shareInfoBetweenPopup = result;
            parent.homeManager.CachedData.shouldReloadView = true;
            
            setTimeout(function () {
                parent.ClosePopup();
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };
        $scope.updateChangeFlag = function () {
            if ($scope.isParamsChange()) {
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_MultipleLogs_Changes;
            } else {
                parent.homeManager.CachedData.isChangingDataPopUp = false;
            }

            $scope.safeApply();
        };
        $scope.isParamsChange = function () {
            return $scope.multipleLogs.Extension.Id !== $scope.oldmultipleLogs.Extension.Id ||
                $scope.multipleLogs.Country.Id !== $scope.oldmultipleLogs.Country.Id ||
                $scope.multipleLogs.CommunicationTool.Id !== $scope.oldmultipleLogs.CommunicationTool.Id ||
                $scope.multipleLogs.Language.Id !== $scope.oldmultipleLogs.Language.Id ||
                $scope.multipleLogs.MainCategory.Id !== $scope.oldmultipleLogs.MainCategory.Id ||
                $scope.multipleLogs.SubCategory.Id !== $scope.oldmultipleLogs.SubCategory.Id ||
                $scope.multipleLogs.Product.Id !== $scope.oldmultipleLogs.Product.Id ||
                $scope.multipleLogs.SubProduct.Id !== $scope.oldmultipleLogs.SubProduct.Id ||
                $scope.multipleLogs.Status.Id !== $scope.oldmultipleLogs.Status.Id ||
                $scope.multipleLogs.BetType.Id !== $scope.oldmultipleLogs.BetType.Id ||
                $scope.multipleLogs.RoleInfo.Id !== $scope.oldmultipleLogs.RoleInfo.Id ||
                $scope.multipleLogs.SiteInfo.Id !== $scope.oldmultipleLogs.SiteInfo.Id ||
                $scope.multipleLogs.LicenseeType.Id !== $scope.oldmultipleLogs.LicenseeType.Id ||
                $scope.multipleLogs.Response.Id !== $scope.oldmultipleLogs.Response.Id ||
                $scope.multipleLogs.Remark !== $scope.oldmultipleLogs.Remark ||
                $scope.multipleLogs.Solution !== $scope.oldmultipleLogs.Solution ||
                $scope.multipleLogs.Subject !== $scope.oldmultipleLogs.Subject;
        };
        $scope.validateEditMultipleLogsCriteria = function (criteria) {
            var result = true;
            var feedbackTime = new Date(criteria.StartTime);
            var resolutionTime = new Date(criteria.EndTime);

            if (feedbackTime > resolutionTime) {
                $scope.resultMessage = viewResources.Multiple_Logs_ErrorEndTime;
                result = false;
            }

            if (result && criteria.CommunicationToolId === crmConstants.Enums.CommunicationTool.NotApply) {
                $scope.resultMessage = viewResources.Multiple_Logs_NACommmunicationTool;
                result = false;
            }

            if (result && criteria.LanguageId === crmConstants.Enums.Language.NotApply) {
                $scope.resultMessage = viewResources.Multiple_Logs_NALanguage;
                result = false;
            }

            if (result && criteria.Subject === '') {
                $scope.resultMessage = viewResources.Multiple_Logs_EmptySubject;
                result = false;
            }

            if (result &&
            ($.trim(criteria.Remark) === '' || criteria.Remark === '<br>')) {
                $scope.resultMessage = viewResources.Multiple_Logs_EmptyRemark;
                result = false;
            }

            if (result &&
            ($.trim(criteria.Solution) === '' || criteria.Solution === '<br>')) {
                $scope.resultMessage = viewResources.Multiple_Logs_EmptySolution;
                result = false;
            }

            if (result) {
                $scope.isShowResultMessage = false;
            } else {
                $scope.isShowResultMessage = true;
                $scope.isSuccess = false;
            }

            return result;
        };
        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };
        $scope.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
        };
        $scope.getSettings = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $editMultipleLogsService.getSettings()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.extensionList = allSetting.ExtensionList;
                $scope.countryList = allSetting.CountryList;
                $scope.communicationToolList = allSetting.CommunicationToolList;
                $scope.languageList = allSetting.LanguageList;
                $scope.mainCategoryList = allSetting.MainCategoryList;
                $scope.productOptions.DataList = allSetting.ProductList;
                $scope.statusList = allSetting.StatusList;
                $scope.siteList = allSetting.SiteList;
                $scope.roleList = allSetting.RoleList;
                $scope.responseList = allSetting.ResponseList;
                $scope.licenseeTypes = allSetting.LicenseeType;
                $scope.licenseeTypeOptions = {
                    DataList: allSetting.LicenseeType,
                    OnChangeEvent: $scope.updateSelectedLicenseeType,
                    SelectedId: $scope.multipleLogs.LicenseeType.Id
                };

                $scope.updateAllSettingsForLog();
                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.completedLoadFilterInfo();
            });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.initSelectedLogInfo();
            $scope.unblockUI();
        };
        $scope.updateAllSettingsForLog = function () {
            SettingManager.updateSettingSelectList($scope.multipleLogs.Extension, $scope.extensionList);
            SettingManager.updateSettingSelectList($scope.multipleLogs.CommunicationTool, $scope.communicationToolList);

            SettingManager.updateSettingSelectList($scope.multipleLogs.MainCategory, $scope.mainCategoryList);
            SettingManager.updateInActiveChildSettings($scope.multipleLogs.SubCategory, $scope.mainCategoryList, $scope.multipleLogs.MainCategory.Id);

            $scope.updateProductSettings();

            SettingManager.updateSettingSelectList($scope.multipleLogs.Language, $scope.languageList);
            SettingManager.updateSettingSelectList($scope.multipleLogs.Status, $scope.statusList);
            SettingManager.updateSettingSelectList($scope.multipleLogs.RoleInfo, $scope.roleList);
            SettingManager.updateSettingSelectList($scope.multipleLogs.Response, $scope.responseList);

            SettingManager.updateSettingSelectList($scope.multipleLogs.SiteInfo, $scope.siteList);
            SettingManager.updateSettingSelectList($scope.multipleLogs.LicenseeType, $scope.licenseeTypeOptions.DataList);
        };
        $scope.updateProductSettings = function () {
            var product = $scope.multipleLogs.Product;
            var productList = $scope.productOptions.DataList;
            SettingManager.updateSettingSelectList(product, productList);

            var subProduct = $scope.multipleLogs.SubProduct;
            var subProductList = $scope.subProductOptions.DataList;
            SettingManager.updateInActiveChildSettings(subProduct, productList, product.Id);
            subProductList = SettingManager.getChildSettings(product.Id, productList);
            SettingManager.updateInActiveChildSettings($scope.multipleLogs.BetType, subProductList, subProduct.Id);
        };

        angular.element(document).ready(function () {
            sharedScriptManager.safeApply($scope);
            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });

            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;

            registerOptions($scope);
            $scope.initController();
            $scope.setFocus();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "MultipleLogs_Edit"
            });

            hotKeyManager.CachedData.isBubbleToParent = false;
        });
    }]);
})();