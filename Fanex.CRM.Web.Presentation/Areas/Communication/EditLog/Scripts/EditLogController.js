"use strict";
/* global editLogApplication,
communicationLog,
sharedScriptManager,
crmConstants,
viewResources,
getRelatedCustomer,
verifyRelatedCustomer,
hotKeyManager,
hasReviewPermission,
verifyRelatedCustomerForUpdate,
isOldRelatedCustomer,
getRelatedCustomerLength */
/* jshint camelcase: false */
editLogApplication.controller('editLogController', ['$scope', '$http', '$editLogService',
function (
    $scope,
    $http,
    $editLogService) {
    /* jshint maxstatements: 80 */
    $scope.communicationLog = communicationLog;
    $scope.oldCommunicationLog = angular.copy(communicationLog);
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

    $scope.isSuccess = false;
    $scope.isShowResultMessage = false;
    $scope.resultMessage = '';
    $scope.isEditingRelatedCustomer = false;
    $scope.isVerifyRelatedCustomer = false;
    $scope.isVerifyRelatedCustomer = false;
    $scope.oldRelatedCustomer = '';
    $scope.isSetFocus = false;
    $scope.hasReviewPermission = hasReviewPermission;
    $scope.isShowRemarkBox = true;
    $scope.isShowSolutionBox = true;

    $scope.isUpdatingTime = false;
    $scope.initController = function () {
        $scope.getAllSettingForEdit();
        $scope.getNumberOfFeedbackSubjectTemplate();
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
        $scope.initSelectedLogInfo();
        $scope.unblockUI();
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

        if ($scope.productList !== null) {
            var productId = communicationLog.Product.Id;
            var productInfo;
            var productListLength = $scope.productList.length;
            for (i = 0; i < productListLength; i++) {
                var product = $scope.productList[i];
                if (product.Id === productId) {
                    productInfo = product;
                    break;
                }
            }

            if (productInfo) {
                $scope.betTypeList = productInfo.ChildSettings;
            }
        }

        $scope.isEditingRelatedCustomer = ($scope.communicationLog.RelatedCustomers !== '');
        $scope.oldCommunicationLog = angular.copy($scope.communicationLog);
        $scope.updateChangeFlag();
    };

    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlS = function () {
        if (!$scope.oldCommunicationLog.IsReviewed ||
            !$scope.communicationLog.IsReviewed) {
            $scope.preupdateLog();
            $scope.$apply();
        }
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

        $scope.updateChangeFlag();
    };
    $scope.updateEndTime = function (selectedTime) {
        $scope.communicationLog.EndTime = selectedTime;
        if (selectedTime < new Date($scope.communicationLog.StartTime)) {
            $scope.communicationLog.StartTime = new Date(selectedTime);
            $scope.communicationLog.StartTime.setMinutes(selectedTime.getMinutes() - 1);
        }

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
            $scope.communicationLog.Extension = extensionInfo;
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
            $scope.communicationLog.Country = countryInfo;
        } else {
            $scope.communicationLog.Country = {
                Id: crmConstants.Enums.Country.NotApply
            };
        }

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
            $scope.communicationLog.CommunicationTool = communicationToolInfo;
        }

        $scope.$apply();
        $scope.updateChangeFlag();
    };
    $scope.updateSelectedLanguage = function (languageId) {
        languageId = parseInt(languageId);
        $scope.communicationLog.LanguageId = languageId;
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
            $scope.communicationLog.MainCategory = mainCategoryInfo;
            $scope.subCategoryList = mainCategoryInfo.ChildSettings;
            $scope.communicationLog.SubCategory = $scope.subCategoryList[0];
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
            $scope.communicationLog.SubCategory = subCategoryInfo;
            $scope.$apply();
            $scope.updateChangeFlag();
        }
    };
    $scope.updateSelectedProduct = function (productId) {
        productId = parseInt(productId);
        var productInfo;
        var productListLength = $scope.productList.length;
        for (var i = 0; i < productListLength; i++) {
            var product = $scope.productList[i];
            if (product.Id === productId) {
                productInfo = product;
                break;
            }
        }

        if (productInfo) {
            $scope.communicationLog.Product = productInfo;
            $scope.betTypeList = productInfo.ChildSettings;
            $scope.communicationLog.BetType = $scope.betTypeList[0];
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
            $scope.communicationLog.BetType = betTypeInfo;
            $scope.$apply();
            $scope.updateChangeFlag();
        }
    };
    $scope.updateSelectedSite = function (siteId) {
        siteId = parseInt(siteId);
        var siteInfo;
        var siteGroupLength = $scope.siteList.length;
        for (var i = 0; i < siteGroupLength; i++) {
            var siteGroup = $scope.siteList[i];
            var siteLength = siteGroup.ChildSites.length;
            for (var j = 0; j < siteLength; j++) {
                var site = siteGroup.ChildSites[j];
                if (site.Id === siteId) {
                    siteInfo = site;
                    break;
                }
            }

            if (siteInfo) {
                break;
            }
        }

        if (siteInfo) {
            $scope.communicationLog.SiteInfo = siteInfo;
        } else {
            $scope.communicationLog.SiteInfo = {
                Id: crmConstants.Enums.Site.NotApply,
                Value: viewResources.Select
            };
        }

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
            $scope.communicationLog.Status = statusInfo;
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
            $scope.communicationLog.RoleInfo = roleInfo;
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
            $scope.communicationLog.Response = responseInfo;
            $scope.$apply();
            $scope.updateChangeFlag();
        }
    };
    $scope.updateRemark = function (remark) {
        $scope.communicationLog.Remark = remark;
        $scope.updateChangeFlag();
    };
    $scope.updateReview = function (review) {
        $scope.communicationLog.Review = review;
        $scope.updateChangeFlag();
    };
    $scope.updateSolution = function (solution) {
        $scope.communicationLog.Solution = solution;
        $scope.updateChangeFlag();
    };
    $scope.updateEditFlag = function () {
        $scope.isEditingRelatedCustomer = true;
    };
    $scope.updateOldRelatedCustomer = function (oldRelatedCustomer) {
        $scope.oldRelatedCustomer = oldRelatedCustomer;
    };
    $scope.preupdateLog = function () {
        if ($scope.oldCommunicationLog.IsReviewed &&
            !$scope.communicationLog.IsReviewed &&
            !$scope.hasReviewPermission) {
            return;
        }

        $scope.isShowResultMessage = false;
        var inputCustomerLength = getRelatedCustomerLength();
        if (!isOldRelatedCustomer($scope.oldRelatedCustomer) &&
            $scope.isEditingRelatedCustomer &&
            inputCustomerLength) {
            /* jshint camelcase: false */
            $scope.isShowResultMessage = true;
            $scope.isSuccess = false;
            $scope.resultMessage = viewResources.Communication_Logs_VerifyRelatedCustomer;
            return;
        }

        $scope.updateLog();
    };

    $scope.updateLog = function () {
        $scope.isShowResultMessage = false;
        $scope.isSuccess = true;
        var relatedCustomer = !$scope.isVerifyRelatedCustomer &&
            isOldRelatedCustomer($scope.oldRelatedCustomer) ?
            $scope.communicationLog.RelatedCustomerIds :
            getRelatedCustomer();

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
            IsReviewed: $scope.hasReviewPermission && $scope.communicationLog.IsReviewed,
            Review: $scope.hasReviewPermission ? $scope.communicationLog.Review : "",
            Subject: $scope.communicationLog.Subject
        };

        if ($scope.validateUpdateLogCriteria(updateCriteria)) {
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
        }
    };
    $scope.showUpdateResult = function (updateResult) {
        updateResult = sharedScriptManager.formatAjaxResult(updateResult);
        if (updateResult.Code !== 0) {
            $scope.resultMessage = updateResult.Message;
            $scope.isShowResultMessage = true;
            $scope.isSuccess = false;
            $scope.unblockUI();
            return;
        }

        $scope.oldCommunicationLog = angular.copy($scope.communicationLog);
        parent.homeManager.CachedData.shareInfoBetweenPopup = updateResult;
        parent.homeManager.CachedData.shouldReloadView = true;
        $scope.unblockUI();
        parent.ClosePopup();
    };
    $scope.ignoreChanges = function () {
        $scope.oldCommunicationLog = angular.copy($scope.communicationLog);
        $scope.$apply();
        $scope.updateChangeFlag();
        parent.ClosePopup();
    };
    $scope.updateChangeFlag = function () {
        if ($scope.communicationLog.RunningNumber !== $scope.oldCommunicationLog.RunningNumber ||
            $scope.communicationLog.CustomerId !== $scope.oldCommunicationLog.CustomerId ||
            $scope.communicationLog.RelatedCustomerId !== $scope.oldCommunicationLog.RelatedCustomerId ||
            $scope.communicationLog.LanguageId !== $scope.oldCommunicationLog.LanguageId ||
            $scope.communicationLog.Country.Id !== $scope.oldCommunicationLog.Country.Id ||

            !sharedScriptManager.datesEqual($scope.communicationLog.StartTime, new Date($scope.oldCommunicationLog.StartTime)) ||
            !sharedScriptManager.datesEqual($scope.communicationLog.EndTime, new Date($scope.oldCommunicationLog.EndTime)) ||
            $scope.communicationLog.CommunicationTool.Id !== $scope.oldCommunicationLog.CommunicationTool.Id ||
            $scope.communicationLog.Extension.Id !== $scope.oldCommunicationLog.Extension.Id ||
            $scope.communicationLog.MainCategory.Id !== $scope.oldCommunicationLog.MainCategory.Id ||
            $scope.communicationLog.SubCategory.Id !== $scope.oldCommunicationLog.SubCategory.Id ||
            $scope.communicationLog.SiteInfo.Id !== $scope.oldCommunicationLog.SiteInfo.Id ||
            $scope.communicationLog.RoleInfo.Id !== $scope.oldCommunicationLog.RoleInfo.Id ||
            $scope.communicationLog.Product.Id !== $scope.oldCommunicationLog.Product.Id ||
            $scope.communicationLog.BetType.Id !== $scope.oldCommunicationLog.BetType.Id ||
            $scope.communicationLog.Response.Id !== $scope.oldCommunicationLog.Response.Id ||
            $scope.communicationLog.Status.Id !== $scope.oldCommunicationLog.Status.Id ||
            $scope.communicationLog.Remark !== $scope.oldCommunicationLog.Remark ||
            $scope.communicationLog.Solution !== $scope.oldCommunicationLog.Solution ||
            $scope.communicationLog.IsReviewed !== $scope.oldCommunicationLog.IsReviewed ||
            $scope.communicationLog.Review !== $scope.oldCommunicationLog.Review ||
            $scope.communicationLog.Subject !== $scope.oldCommunicationLog.Subject) {
            parent.homeManager.CachedData.isChangingDataPopUp = true;
            parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_EditCommunication_Changes;
        } else {
            parent.homeManager.CachedData.isChangingDataPopUp = false;
        }
    };
    $scope.validateUpdateLogCriteria = function (criteria) {
        var result = true;
        if (result && criteria.CommunicationToolId === crmConstants.Enums.CommunicationTool.NotApply) {
            $scope.resultMessage = viewResources.Communication_Log_NACommmunicationTool;
            result = false;
        }

        if (result && criteria.LanguageId === crmConstants.Enums.Language.NotApply) {
            $scope.resultMessage = viewResources.Communication_Log_NALanguage;
            result = false;
        }

        if (result && criteria.Subject === '') {
            $scope.resultMessage = viewResources.Communication_Log_EmptySubject;
            result = false;
        }

        if (result &&
            ($.trim(criteria.Remark) === '' || criteria.Remark === '<br>')) {
            $scope.resultMessage = viewResources.Communication_Log_EmptyRemark;
            result = false;
        }

        if (result &&
            ($.trim(criteria.Solution) === '' || criteria.Solution === '<br>')) {
            $scope.resultMessage = viewResources.Communication_Log_EmptySolution;
            result = false;
        }

        if (result &&
            criteria.IsReviewed &&
            ($.trim(criteria.Review) === '' || criteria.Review === '<br>')) {
            $scope.resultMessage = viewResources.Communication_Log_EmptyReview;
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
    };
    $scope.BlockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };
    $scope.updateSelectedSubject = function (subject) {
        $scope.communicationLog.Subject = subject;
    };
    $scope.verifyRelatedCustomer = function () {
        verifyRelatedCustomer($scope);
        $scope.isEditingRelatedCustomer = false;
        $scope.isVerifyRelatedCustomer = true;
    };
    $scope.verifyRelatedCustomerForUpdate = function () {
        verifyRelatedCustomerForUpdate($scope);
        $scope.isEditingRelatedCustomer = false;
    };

    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };

    $scope.setFocus = function () {
        $scope.$apply(function () {
            $scope.isSetFocus = true;
        });
    };
    $scope.getAllSettingForEdit = function () {
        $scope.BlockUI();
        $scope.isLoadedAllSite = false;
        $editLogService.getAllSettingForEdit()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.extensionList = allSetting.ExtensionList;
                $scope.countryList = allSetting.CountryList;
                $scope.communicationToolList = allSetting.CommunicationToolList;
                $scope.languageList = allSetting.LanguageList;
                $scope.mainCategoryList = allSetting.MainCategoryList;
                $scope.productList = allSetting.ProductList;
                $scope.statusList = allSetting.StatusList;
                $scope.siteList = allSetting.SiteList;
                $scope.roleList = allSetting.RoleList;
                $scope.responseList = allSetting.ResponseList;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getReviewedInfo = function () {
        return sharedScriptManager.stringFormat(
            viewResources.ReviewedInformation,
            [$scope.communicationLog.Reviewer, $scope.communicationLog.ReviewedTimeString]);
    };

    angular.element(document).ready(function () {
        $scope.$watch(function ($scope) {
            $scope.updateChangeFlag();
        });

        parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController = $scope;
        $scope.initController();
        $scope.setFocus();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs_Edit"
        });

        hotKeyManager.CachedData.isBubbleToParent = false;
    });
} ]);