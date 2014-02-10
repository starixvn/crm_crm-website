"use strict";
/* global insertLogApplication,
sharedScriptManager,
moment,
crmConstants,
viewResources,
getRelatedCustomer,
customerId,
verifyRelatedCustomer,
hotKeyManager */
insertLogApplication.controller('insertLogController', function ($scope,
    $http,
    $insertLogService) {
    /* jshint maxstatements: 80 */
    $scope.communicationLog = {
        RunningNumber: 0,
        FeedbackDate: new Date(),
        MainCategory: {
            Name: ''
        },
        SubCategory: {
            Name: ''
        },
        Subject: '',
        StatusName: '',
        Duration: '',
        Handler: '',
        Reviewer: '',
        StartTime: new Date(),
        EndTime: new Date(),
        Extension: {
            Id: 0,
            Value: ''
        },
        CommunicationTool: {
            Id: 0,
            Value: ''
        },
        LanguageId: 0,
        Product: {
            Id: 0,
            Value: ''
        },
        BetType: {
            Id: 0,
            Value: ''
        },
        Status: {
            Id: 0,
            Value: ''
        },
        SiteInfo: {
            Id: 0,
            Value: ''
        },
        RoleInfo: {
            Id: 0,
            Value: ''
        },
        Response: {
            Id: 0,
            Value: ''
        },
        RelatedCustomers: '',
        Remark: '',
        Solution: '',
        Review: '',
        Country: {
            Id: 0
        },
        IsReviewed: false
    };
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
        $insertLogService.getAllExtension()
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
        $insertLogService.getAllCountry()
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
        $insertLogService.getAllCommunicationTool()
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
        $insertLogService.getAllLanguage()
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
        $insertLogService.getAllMainCategory()
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
        $insertLogService.getAllProduct()
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
        $insertLogService.getAllStatus()
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
        $insertLogService.getAllSite()
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
        $insertLogService.getAllRole()
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
        $insertLogService.getAllResponse()
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
        $insertLogService.getNumberOfFeedbackSubjectTemplate()
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
        $insertLogService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
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
        $scope.subCategoryList = $scope.mainCategoryList[0].ChildSettings;
        $scope.betTypeList = $scope.productList[0].ChildSettings;

        var currentTime = new Date();
        $scope.communicationLog.StartTime = new Date(currentTime);
        var initEndTime = new Date(currentTime);
        initEndTime.setMinutes(new Date(currentTime).getMinutes() + 1);
        $scope.communicationLog.EndTime = initEndTime;
        setTimeout(function () {
            $scope.communicationLog.Extension = $scope.extensionList[0];
            $scope.communicationLog.Country = $scope.countryList[0];
            $scope.communicationLog.CommunicationTool = $scope.communicationToolList[0];
            $scope.communicationLog.LanguageId = $scope.languageList[0].Id;
            $scope.communicationLog.MainCategory = $scope.mainCategoryList[0];
            $scope.communicationLog.SubCategory = $scope.subCategoryList[0];
            $scope.communicationLog.Product = $scope.productList[0];
            $scope.communicationLog.BetType = $scope.betTypeList[0];
            $scope.communicationLog.SiteInfo = $scope.statusList[0];
            $scope.communicationLog.Status = $scope.siteList[0];
            $scope.communicationLog.RoleInfo = $scope.roleList[0];
            $scope.communicationLog.Response = $scope.responseList[0];
        }, 1000);

        $scope.updateRemarkPrefix();
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
    $scope.invokeHotKey_CtrlE = function () {
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
    $scope.updateSelectedSubject = function (subject) {
        $scope.communicationLog.Subject = subject;
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
    $scope.updateEditFlag = function () {
        $scope.isEditingRelatedCustomer = true;
    };
    $scope.insertLog = function () {
        $scope.isShowResultMessage = false;
        var relatedCustomer = getRelatedCustomer();
        if (relatedCustomer.length !== 0 && $scope.isEditingRelatedCustomer) {
            /* jshint camelcase: false */
            alert(viewResources.Communication_Logs_VerifyRelatedCustomer);
            return;
        }

        var insertCriteria = {
            CustomerId: customerId,
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
        $insertLogService.insertLog(insertCriteria)
            .then(function (data) {
                var insertResult = data.Result;
                $scope.showInsertResult(insertResult);
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
    };
    $scope.showInsertResult = function (insertResult) {
        parent.shouldReloadLogs = true;
        $scope.isShowResultMessage = true;
        $scope.isSuccess = insertResult.Code === crmConstants.Enums.Error.None;
        $scope.resultMessage = insertResult.Message;
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
            Id: "CustomerCommunicationLogs_Insert"
        });
    });
});