(function () {
    "use strict";
    /* global communicationLog,
    sharedScriptManager,
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

        $scope.productOptions = null;
        $scope.siteOptions = null;
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
            $scope.initPicker();
        };
        $scope.initPicker = function () {
            $scope.productOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.Product.Id
            };

            $scope.siteOptions = {
                DataList: [],
                OnChangeEvent: null,
                AllText: viewResources.Select,
                IsIncludeNA: true,
                SelectedId: $scope.communicationLog.SiteInfo.Id
            };

            $scope.statusOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.Status.Id
            };
            $scope.responseOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.Response.Id
            };

            $scope.roleOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.RoleInfo.Id
            };
            $scope.communicationToolOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.CommunicationTool.Id
            };
            $scope.mainCategoryOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.MainCategory.Id
            };
            $scope.subCategoryOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.SubCategory.Id
            };
            $scope.extensionOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.Extension.Id
            };
            $scope.countryOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.Country.Id
            };
            $scope.languageOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.LanguageId
            };
            $scope.betTypeOptions = {
                DataList: [],
                OnChangeEvent: null,
                SelectedId: $scope.communicationLog.BetType.Id
            };
            $scope.remarkOptions = {
                OnChangeEvent: null,
                SelectedText: $scope.communicationLog.Remark,
                EditIconText: viewResources.Insert_Edit_Block
            };
            $scope.solutionOptions = {
                OnChangeEvent: null,
                SelectedText: $scope.communicationLog.Solution,
                EditIconText: viewResources.Insert_Edit_Block
            };
            $scope.reviewOptions = {
                OnChangeEvent: null,
                SelectedText: $scope.communicationLog.Review,
                EditIconText: viewResources.Insert_Edit_Block
            };
            $scope.subjectOptions = {
                OnChangeEvent: null,
                NumberOfSuggestItems: 15,
                SubjectList: $scope.feedbackSubjectTemplateList,
                Subject: $scope.communicationLog.Subject
            };
            $scope.startTimeOptions = {
                OnChangeEvent: null,
                SelectedTime: $scope.communicationLog.StartTime,
                CurrentTime: currentTime
            };
            $scope.endTimeOptions = {
                OnChangeEvent: null,
                SelectedTime: $scope.communicationLog.EndTime,
                CurrentTime: currentTime
            };

            $scope.$apply();
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

            var productList = $scope.productOptions.DataList;
            if (productList !== null) {
                var productId = communicationLog.Product.Id;
                var productInfo;
                var productListLength = productList.length;
                for (i = 0; i < productListLength; i++) {
                    var product = productList[i];
                    if (product.Id === productId) {
                        productInfo = product;
                        break;
                    }
                }

                if (productInfo) {
                    $scope.betTypeOptions.DataList = productInfo.ChildSettings;
                }
            }
        };

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
                $scope.siteOptions.DataList = allSetting.SiteList;
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
                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.completedLoadFilterInfo();
            });
        };
        $scope.getReviewedInfo = function () {
            return sharedScriptManager.stringFormat(
            viewResources.ReviewedInformation,
            [$scope.communicationLog.Reviewer, $scope.communicationLog.ReviewedTimeString]);
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();