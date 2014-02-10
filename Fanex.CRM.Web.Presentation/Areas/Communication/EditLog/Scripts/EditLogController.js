"use strict";
/* global editLogApplication,
communicationLog,
sharedScriptManager,
moment,
crmConstants,
viewResources,
getRelatedCustomer,
verifyRelatedCustomer,
hotKeyManager */
editLogApplication.controller('editLogController', function ($scope,
    $http,
    $editLogService) {
    /* jshint maxstatements: 66 */
    $scope.communicationLog = communicationLog;
    $scope.extensionList = null;
    $scope.countryList = null;
    $scope.communicationToolList = null;
    $scope.languageList = null;
    $scope.mainCategoryList = null;
    $scope.subCategoryList = null;
    $scope.productList = null;
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
    $scope.isEditingRelatedCustomer = false;

    $scope.loadedFilter = [];
    $scope.isUpdatingTime = false;
    $scope.initController = function () {
        $scope.getAllExtension();
        $scope.getAllCountry();
        $scope.getCommunicationTool();
        $scope.getAllLanguage();
        $scope.getAllMainCategory();
        $scope.getAllStatus();
        $scope.getAllSite();
        $scope.getAllRole();
        $scope.getAllResponse();
        $scope.getNumberOfFeedbackSubjectTemplate();
    };
    $scope.getAllExtension = function () {
        $scope.BlockUI();
        $scope.isLoadedAllSite = false;
        $editLogService.getAllExtension()
            .then(function (data) {
                $scope.extensionList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Extension'
                });

                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Extension'
                });

                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllCountry = function () {
        $scope.BlockUI();
        $editLogService.getAllCountry()
            .then(function (data) {
                $scope.countryList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Country'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Country'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getCommunicationTool = function () {
        $scope.BlockUI();
        $editLogService.getAllCommunicationTool()
            .then(function (data) {
                $scope.communicationToolList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'CommunicationTool'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'CommunicationTool'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllLanguage = function () {
        $scope.BlockUI();
        $editLogService.getAllLanguage()
            .then(function (data) {
                $scope.languageList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Language'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Language'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllMainCategory = function () {
        $scope.BlockUI();
        $editLogService.getAllMainCategory()
            .then(function (data) {
                $scope.mainCategoryList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'MainCategory'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'MainCategory'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllProduct = function () {
        $scope.BlockUI();
        $editLogService.getAllProduct()
            .then(function (data) {
                $scope.productList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Product'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Product'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllStatus = function () {
        $scope.BlockUI();
        $editLogService.getAllStatus()
            .then(function (data) {
                $scope.statusList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Status'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Status'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllSite = function () {
        $scope.BlockUI();
        $editLogService.getAllSite()
            .then(function (data) {
                $scope.siteList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Site'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Site'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllRole = function () {
        $scope.BlockUI();
        $editLogService.getAllRole()
            .then(function (data) {
                $scope.roleList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Role'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Role'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllResponse = function () {
        $scope.BlockUI();
        $editLogService.getAllResponse()
            .then(function (data) {
                $scope.responseList = data.Result;
                $scope.loadedFilter.push({
                    Id: 'Response'
                });
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.loadedFilter.push({
                    Id: 'Response'
                });
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getNumberOfFeedbackSubjectTemplate = function () {
        $scope.BlockUI();
        $editLogService.getNumberOfFeedbackSubjectTemplate()
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
            function (data) {
                /* jshint unused: vars */
                $scope.isLoadedFeedbackSubjectTemplate = true;
            });
    };
    $scope.getFeedbackSubjectTemplateByPagingIndex = function (pagingIndex) {
        $scope.BlockUI();
        $editLogService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
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
            function (data) {
                /* jshint unused: vars */
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            });
    };
    $scope.completedLoadFilterInfo = function () {
        var numberOfLoadFilter = 10;
        if ($scope.loadedFilter.length === numberOfLoadFilter - 1) {
            $scope.getAllProduct();
            return;
        }

        if ($scope.loadedFilter.length >= numberOfLoadFilter &&
            $scope.isLoadedFeedbackSubjectTemplate) {
            $scope.initSelectedLogInfo();
            $scope.unblockUI();
        }
    };
    $scope.initSelectedLogInfo = function () {
        var i;
        if ($scope.mainCategoryList != null) {
            var mainCategoryListLength = $scope.mainCategoryList.length;
            for (i = 0; i < mainCategoryListLength; i++) {
                var mainCategory = $scope.mainCategoryList[i];
                if (mainCategory.Id === communicationLog.MainCategory.Id) {
                    $scope.communicationLog.MainCategory = mainCategory;
                    $scope.subCategoryList = mainCategory.ChildSettings;
                    break;
                }
            }
        }

        if ($scope.subCategoryList != null) {
            var subCategoryListLength = $scope.subCategoryList.length;
            for (i = 0; i < subCategoryListLength; i++) {
                var subCategory = $scope.subCategoryList[i];
                if (subCategory.Id === communicationLog.SubCategory.Id) {
                    $scope.communicationLog.SubCategory = subCategory;
                    break;
                }
            }
        }

        if ($scope.communicationToolList != null) {
            var communicationToolListLength = $scope.communicationToolList.length;
            for (i = 0; i < communicationToolListLength; i++) {
                var communicationTool = $scope.communicationToolList[i];
                if (communicationTool.Id === communicationLog.CommunicationTool.Id) {
                    $scope.communicationLog.CommunicationTool = communicationTool;
                    break;
                }
            }
        }

        $scope.betTypeList = $scope.productList[0].ChildSettings;
        $scope.updateRemarkPrefix();
        $scope.verifyRelatedCustomer();
    };
    $scope.generateRemarkPrefix = function () {
        /* jshint camelcase: false */
        return sharedScriptManager.stringFormat("{0}/{1} ",
            [moment($scope.communicationLog.StartTime).format(crmConstants.Enums.FormatConstant.StartTime_RemarkPrefix),
            $scope.communicationLog.CommunicationTool.Value]);
    };
    $scope.updateRemarkPrefix = function () {
        var remarkPrefix = $scope.generateRemarkPrefix();
        if ($scope.communicationLog.Remark.indexOf($scope.lastRemarkPrefix) === 0) {
            $scope.communicationLog.Remark = $scope.communicationLog.Remark.substring($scope.lastRemarkPrefix.length, $scope.communicationLog.Remark.length);
        }

        if ($scope.communicationLog.Remark.indexOf(remarkPrefix) === -1) {
            $scope.communicationLog.Remark = remarkPrefix + $scope.communicationLog.Remark;
            $scope.lastRemarkPrefix = remarkPrefix;
        }
    };
    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlS = function () {
        $scope.updateLog();
    };
    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlV = function () {
        $scope.verifyRelatedCustomer();
    };
    $scope.updateStartTime = function (selectedTime) {
        $scope.communicationLog.StartTime = selectedTime;
        if (new Date($scope.communicationLog.EndTime) < selectedTime) {
            $scope.communicationLog.EndTime = new Date(selectedTime);
            $scope.communicationLog.EndTime.setMinutes(selectedTime.getMinutes() + 1);
        }

        $scope.updateRemarkPrefix();
    };
    $scope.updateEndTime = function (selectedTime) {
        $scope.communicationLog.EndTime = selectedTime;
        if (selectedTime < new Date($scope.communicationLog.StartTime)) {
            $scope.communicationLog.StartTime = new Date(selectedTime);
            $scope.communicationLog.StartTime.setMinutes(selectedTime.getMinutes() - 1);
        }

        $scope.updateRemarkPrefix();
    };
    $scope.updateSelectedExtension = function (extention) {
        $scope.communicationLog.Extension = extention;
    };
    $scope.updateSelectedCountry = function (country) {
        $scope.communicationLog.Country = country;
    };
    $scope.updateSelectedCommunicationTool = function (communicationTool) {
        $scope.communicationLog.CommunicationTool = communicationTool;
        $scope.updateRemarkPrefix();
    };
    $scope.updateSelectedLanguage = function (languageId) {
        $scope.communicationLog.LanguageId = languageId;
    };
    $scope.updateSelectedMainCategory = function (mainCategory) {
        $scope.communicationLog.MainCategory = mainCategory;
        $scope.subCategoryList = mainCategory.ChildSettings;
    };
    $scope.updateSelectedSubCategory = function (subCategory) {
        $scope.communicationLog.SubCategory = subCategory;
    };
    $scope.updateSelectedProduct = function (product) {
        $scope.communicationLog.Product = product;
        $scope.communicationLog.BetType = product.ChildSettings[0];
        $scope.betTypeList = product.ChildSettings;
    };
    $scope.updateSelectedBetType = function (betType) {
        $scope.communicationLog.BetType = betType;
    };
    $scope.updateSelectedSite = function (site) {
        if (site) {
            $scope.communicationLog.SiteInfo = site;
        } else {
            $scope.communicationLog.SiteInfo = {
                Id: crmConstants.Enums.Site.NotApply,
                Value: viewResources.Select
            };
        }
    };
    $scope.updateSelectedStatus = function (status) {
        $scope.communicationLog.Status = status;
    };
    $scope.updateSelectedRole = function (role) {
        $scope.communicationLog.Role = role;
    };
    $scope.updateSelectedRespones = function (response) {
        $scope.communicationLog.Response = response;
    };
    $scope.updateRemark = function (remark) {
        $scope.communicationLog.Remark = remark;
    };
    $scope.updateReview = function (review) {
        $scope.communicationLog.Review = review;
    };
    $scope.updateSolution = function (solution) {
        $scope.communicationLog.Solution = solution;
    };
    $scope.updateLog = function () {
        $scope.isShowResultMessage = false;
        var relatedCustomer = getRelatedCustomer();
        if (relatedCustomer.length !== 0 && $scope.isEditingRelatedCustomer) {
            alert(viewResources.Communication_Logs_VerifyRelatedCustomer);
            return;
        }

        var updateCriteria = {
            FeedbackId: $scope.communicationLog.RunningNumber,
            CustomerId: $scope.communicationLog.CustomerId,
            RelatedCustomerId: relatedCustomer,
            LanguageId: $scope.communicationLog.LanguageId,
            CountryId: $scope.communicationLog.Country.Id,
            StartTime: $scope.communicationLog.StartTime,
            EndTime: $scope.communicationLog.EndTime,
            CommunicationToolId: $scope.communicationLog.CommunicationTool.Id,
            ExtensionId: $scope.communicationLog.Extension.Id,
            MainCategoryId: $scope.communicationLog.MainCategory.Id,
            SubcategoryId: $scope.communicationLog.SubCategory.Id,
            SiteId: $scope.communicationLog.SiteInfo.Id,
            RoleId: $scope.communicationLog.RoleInfo.Id,
            ProductId: $scope.communicationLog.Product.Id,
            BetTypeId: $scope.communicationLog.BetType.Id,
            ResponseId: $scope.communicationLog.Response.Id,
            StatusId: $scope.communicationLog.Status.Id,
            Remark: $scope.communicationLog.Remark,
            Solution: $scope.communicationLog.Solution,
            IsReviewed: $scope.communicationLog.IsReviewed,
            Review: $scope.communicationLog.Review,
            Subject: $scope.communicationLog.Subject
        };

        $scope.BlockUI();
        $editLogService.updateLog(updateCriteria)
            .then(function (data) {
                var updateResult = data.Result;
                $scope.showUpdateResult(updateResult);
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
    };
    $scope.showUpdateResult = function (updateResult) {
        parent.shouldReloadLogs = true;
        $scope.isShowResultMessage = true;
        $scope.isSuccess = updateResult.Code === crmConstants.Enums.Error.None;
        $scope.resultMessage = updateResult.Message;
        window.scroll(1, 1);
        if ($scope.isSuccess) {
            setTimeout(function () {
                $scope.isShowResultMessage = false;
            }, 3000);
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
    $scope.verifyRelatedCustomer = function () {
        verifyRelatedCustomer($scope);
        $scope.isEditingRelatedCustomer = false;
    };
    angular.element(document).ready(function () {
        $scope.initController();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs_Edit"
        });
    });
});