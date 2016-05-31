(function () {
    "use strict";
    /* global communicationLog,
    sharedScriptManager,
    SettingManager,
    viewResources,
    hasReviewPermission,
    currentTime,
    crmConstants */
    /* jshint camelcase: false */
    angular.module('communicationlog-viewdetailbyid-application').controller('viewDetailByIdController', ['$scope', '$http', '$viewDetailByIdService',
    function ($scope, $http, $viewDetailByIdService) {
        $scope.communicationLog = communicationLog;
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.hasReviewPermission = hasReviewPermission;
        $scope.IsUpdatingTime = false;

        $scope.statusOptions = null;
        $scope.responseOptions = null;
        $scope.roleOptions = null;
        $scope.communicationToolOptions = null;
        $scope.mainCategoryOptions = null;
        $scope.subCategoryOptions = null;
        $scope.extensionOptions = null;
        $scope.countryOptions = null;
        $scope.languageOptions = null;
        $scope.betTypeOptions = null;
        $scope.remarkOptions = null;
        $scope.solutionOptions = null;
        $scope.reviewOptions = null;
        $scope.subjectOptions = null;
        $scope.startTimeOptions = null;
        $scope.endTimeOptions = null;
        $scope.licenseeTypeOptions = null;

        $scope.initController = function () {
            $scope.getAllSetting();
            $scope.getNumberOfFeedbackSubjectTemplate();
        };

        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            $scope.BlockUI();
            $viewDetailByIdService.getNumberOfFeedbackSubjectTemplate()
            .then(function (data) {
                var numberOfFeedbackItem = data.Result;
                $scope.feedbackSubjectTemplateList = [];
                var totalPage = Math.round((numberOfFeedbackItem / sharedScriptManager.cachedData.maxFeedbackSubjectTemplateNumberOnLoad) + 0.5);
                $scope.feedbackSubjectTemplateTotalPage = totalPage;
                $scope.isLoadedFeedbackSubjectTemplate = false;
                for (var i = 1; i <= totalPage; i++) {
                    $scope.getFeedbackSubjectTemplateByPagingIndex(i);
                }
            },
            function () {
                $scope.isLoadedFeedbackSubjectTemplate = true;
            });
        };
        $scope.getFeedbackSubjectTemplateByPagingIndex = function (pagingIndex) {
            $scope.BlockUI();
            $viewDetailByIdService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
            .then(function (data) {
                var feedbackSubjectTemplate = data.Result;
                if (feedbackSubjectTemplate != null && feedbackSubjectTemplate.length > 0) {
                    $scope.feedbackSubjectTemplateList = $scope.feedbackSubjectTemplateList.concat(feedbackSubjectTemplate);
                }

                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            },
            function () {
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.initSelectedLogInfo();
            $scope.relatedCustomerResultDirective.loadRelatedCustomers(
                    $scope.communicationLog.RelatedCustomerIds, $scope.communicationLog.RelatedCustomers);
            $scope.unblockUI();
        };
        $scope.initSelectedLogInfo = function () {
            var i;
            var mainCategoryList = $scope.mainCategoryOptions.DataList;
            if (mainCategoryList != null) {
                var mainCategoryListLength = mainCategoryList.length;
                for (i = 0; i < mainCategoryListLength; i++) {
                    var mainCategory = mainCategoryList[i];
                    if (mainCategory.Id === communicationLog.MainCategory.Id) {
                        $scope.communicationLog.MainCategory = mainCategory;
                        $scope.subCategoryOptions.DataList = mainCategory.ChildSettings;
                        break;
                    }
                }
            }

            var subCategoryList = $scope.subCategoryOptions.DataList;
            if (subCategoryList != null) {
                var subCategoryListLength = subCategoryList.length;
                for (i = 0; i < subCategoryListLength; i++) {
                    var subCategory = subCategoryList[i];
                    if (subCategory.Id === communicationLog.SubCategory.Id) {
                        $scope.communicationLog.SubCategory = subCategory;
                        break;
                    }
                }
            }

            $scope.mainCategoryOptions.SelectedId = $scope.communicationLog.MainCategory.Id;
            $scope.subCategoryOptions.SelectedId = $scope.communicationLog.SubCategory.Id;
            var communicationToolList = $scope.communicationToolOptions.DataList;
            if (communicationToolList != null) {
                var communicationToolListLength = communicationToolList.length;
                for (i = 0; i < communicationToolListLength; i++) {
                    var communicationTool = communicationToolList[i];
                    if (communicationTool.Id === communicationLog.CommunicationTool.Id) {
                        $scope.communicationLog.CommunicationTool = communicationTool;
                        break;
                    }
                }
            }

            $scope.updateSelectedProduct($scope.communicationLog.Product);
        };

        $scope.updateSelectedProduct = function (product) {
            $scope.subProductOptions.DataList = SettingManager.getChildSettings(product.Id, $scope.productOptions.DataList);
            $scope.updateSelectedSubProduct($scope.communicationLog.SubProduct);
        };
        $scope.updateSelectedSubProduct = function (subProduct) {
            var childSettings = SettingManager.getChildSettings(subProduct.Id, $scope.subProductOptions.DataList);
            $scope.betTypeOptions.DataList = childSettings;
        };

        $scope.getSettingById = function (settingId, dataList) {
            for (var i = 0; i < dataList.length; i++) {
                var setting = dataList[i];

                if (setting.Id === settingId) {
                    return setting;
                }
            }

            return null;
        }

        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }
        };
        $scope.BlockUI = function () {
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };
        $scope.getAllSetting = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $viewDetailByIdService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;                

                $scope.productOptions.DataList = allSetting.ProductList;
                $scope.subProductOptions.DataList = allSetting.SubProductList;
                $scope.siteList = allSetting.SiteList;
                $scope.statusOptions.DataList = allSetting.StatusList;
                $scope.responseOptions.DataList = allSetting.ResponseList;
                $scope.roleOptions.DataList = allSetting.RoleList;
                $scope.communicationToolOptions.DataList = allSetting.CommunicationToolList;
                $scope.mainCategoryOptions.DataList = allSetting.MainCategoryList;
                $scope.extensionOptions.DataList = allSetting.ExtensionList;

                $scope.languageOptions.DataList = allSetting.LanguageList;
                $scope.subCategoryOptions.DataList = $scope.mainCategoryOptions.DataList[0].ChildSettings;

                $scope.countryOptions.DataList = [];
                $scope.countryOptions.DataList.push({
                    Id: crmConstants.Enums.Country.NotApply,
                    Name: viewResources.Symbol_NotApply
                });
                $scope.licenseeTypeOptions = {
                    DataList: allSetting.LicenseeType,
                    OnChangeEvent: $scope.updateSelectedLicenseeType,
                    SelectedId: $scope.communicationLog.LicenseeType.Id
                };

                $scope.countryOptions.DataList = $scope.countryOptions.DataList.concat(allSetting.CountryList);

                $scope.updateAllSettingsForLog(allSetting);
                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.completedLoadFilterInfo();
            });
        };

        $scope.updateAllSettingsForLog = function () {
            SettingManager.updateSettingSelectList($scope.communicationLog.Extension, $scope.extensionOptions.DataList);
            SettingManager.updateSettingSelectList($scope.communicationLog.CommunicationTool, $scope.communicationToolOptions.DataList);

            SettingManager.updateSettingSelectList($scope.communicationLog.MainCategory, $scope.mainCategoryOptions.DataList);
            SettingManager.updateInActiveChildSettings($scope.communicationLog.SubCategory, $scope.mainCategoryOptions.DataList, $scope.communicationLog.MainCategory.Id);

            $scope.updateProductSettings();

            SettingManager.updateSettingSelectList($scope.communicationLog.Language, $scope.languageOptions.DataList);
            SettingManager.updateSettingSelectList($scope.communicationLog.Status, $scope.statusOptions.DataList);
            SettingManager.updateSettingSelectList($scope.communicationLog.RoleInfo, $scope.roleOptions.DataList);
            SettingManager.updateSettingSelectList($scope.communicationLog.Response, $scope.responseOptions.DataList);

            SettingManager.updateSettingSelectList($scope.communicationLog.SiteInfo, $scope.siteList);
            SettingManager.updateSettingSelectList($scope.communicationLog.LicenseeType, $scope.licenseeTypeOptions.DataList);
        };

        $scope.updateProductSettings = function () {
            var product = $scope.communicationLog.Product;
            var productList = $scope.productOptions.DataList;
            SettingManager.updateSettingSelectList(product, productList);

            var subProduct = $scope.communicationLog.SubProduct;
            var subProductList = $scope.subProductOptions.DataList;
            SettingManager.updateInActiveChildSettings(subProduct, productList, product.Id);
            subProductList = SettingManager.getChildSettings(product.Id, productList);
            SettingManager.updateInActiveChildSettings($scope.communicationLog.BetType, subProductList, subProduct.Id);
        };

        $scope.getReviewedInfo = function () {
            return sharedScriptManager.stringFormat(
            viewResources.ReviewedInformation,
            [$scope.communicationLog.Reviewer, $scope.communicationLog.ReviewedTimeString]);
        };
        angular.element(document).ready(function () {
            registerOptions($scope);
            $scope.initController();
        });
    } ]);
})();