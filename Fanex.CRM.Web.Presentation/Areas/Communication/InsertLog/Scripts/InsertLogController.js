(function () {
    "use strict";
    /* global sharedScriptManager,
    moment,
    crmConstants,
    viewResources,
    getRelatedCustomer,
    customerId,
    verifyRelatedCustomer,
    hotKeyManager,
    currentTime,
    getRelatedCustomerLength */
    /* jshint camelcase: false */
    angular.module('communicationlog-insert-application').controller('insertLogController', ['$scope', '$http', '$insertLogService',
    function ($scope, $http, $insertLogService) {
        /* jshint maxstatements: 80 */
        $scope.customerInfo = parent.homeManager.CachedData.currentCustomer;

        $scope.communicationLog = {
            RunningNumber: 0,
            FeedbackDate: new Date(currentTime),
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
            StartTime: new Date(currentTime),
            EndTime: new Date(currentTime),
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
            SubProduct: {
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
            LicenseeType: {
                Id: 0
            }
        };
        $scope.oldCommunicationLog = angular.copy($scope.communicationLog);
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
        $scope.defaultSettings = null;
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.lastRemarkPrefix = '';
        $scope.isSuccess = false;
        $scope.isShowResultMessage = false;
        $scope.resultMessage = '';
        $scope.isEditingRelatedCustomer = false;
        $scope.isVerifyRelatedCustomer = false;
        $scope.isSetFocus = false;
        $scope.isShowRemarkBox = true;
        $scope.isShowSolutionBox = true;
        $scope.licenseeTypes = [];

        $scope.isUpdatingTime = false;
        $scope.initController = function () {
            $scope.getAllSettingForInsert($scope.customerInfo.Id);
            $scope.getNumberOfFeedbackSubjectTemplate();
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
            function () {
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
            if ($scope.mainCategoryList === null ||
            $scope.mainCategoryList === undefined) {
                return;
            }

            var firstProduct = $scope.productOptions.DataList[0];
            $scope.updateSelectedProduct(firstProduct);
            $scope.updateSelectedSubProduct(firstProduct.ChildSettings[0]);

            $scope.subCategoryList = $scope.mainCategoryList[0].ChildSettings;           
            var starttime = sharedScriptManager.newFeedbackTime(currentTime);
            var endtime = sharedScriptManager.newFeedbackTime(currentTime);
            endtime.setMinutes(endtime.getMinutes() + 1);
            $scope.communicationLog.StartTime = sharedScriptManager.formatTimeToSendToServer(starttime);
            $scope.communicationLog.EndTime = sharedScriptManager.formatTimeToSendToServer(endtime);

            $scope.communicationLog.Extension = $scope.extensionList[0];
            $scope.communicationLog.Country = {
                Id: $scope.defaultSettings.CountryId
            };

            $scope.communicationLog.Product = {
                Id: crmConstants.Enums.Product.NotApply
            };

            $scope.communicationLog.SubProduct = {
                Id: crmConstants.Enums.SubProduct.NotApply
            };

            $scope.communicationLog.BetType = {
                Id: crmConstants.Enums.BetType.NotApply
            };

            $scope.communicationLog.SiteInfo = {
                Id: $scope.defaultSettings.SiteId
            };

            $scope.communicationLog.LanguageId = $scope.defaultSettings.DefaultLanguage;
            $scope.communicationLog.CommunicationTool = $scope.communicationToolList[0];
            $scope.communicationLog.MainCategory = $scope.mainCategoryList[0];
            $scope.communicationLog.SubCategory = $scope.subCategoryList[0];
            $scope.communicationLog.Status = $scope.statusList[0];
            $scope.communicationLog.RoleInfo = {
                Id: $scope.defaultSettings.RoleId
            };

            $scope.communicationLog.Response = $scope.responseList[0];
            $scope.communicationLog.LicenseeType.Id = $scope.defaultSettings.LicenseeTypeId;

            $scope.updateRemarkPrefix();
            $scope.oldCommunicationLog = angular.copy($scope.communicationLog);
            $scope.updateChangeFlag();
        };
        $scope.generateRemarkPrefix = function () {
            /* jshint camelcase: false */
            return sharedScriptManager.stringFormat("{0}/{1} ",
            [moment($scope.communicationLog.StartTime).format(crmConstants.Enums.FormatConstant.StartTime_RemarkPrefix),
            $scope.communicationLog.CommunicationTool.Name]);
        };
        $scope.updateRemarkPrefix = function () {
            var remarkPrefix = $scope.generateRemarkPrefix();
            if ($scope.communicationLog.Remark.indexOf($scope.lastRemarkPrefix) === 0) {
                $scope.communicationLog.Remark = $scope.communicationLog.Remark.substring(
                    $scope.lastRemarkPrefix.length, 
                    $scope.communicationLog.Remark.length);
            }

            if ($scope.communicationLog.Remark.indexOf(remarkPrefix) === -1) {
                $scope.communicationLog.Remark = remarkPrefix + $scope.communicationLog.Remark;
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
            $scope.insertLog();
            $scope.$apply();
        };
        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlE = function () {
            $scope.verifyRelatedCustomer();
        };
        $scope.updateSelectedProduct = function (product) {
            $scope.communicationLog.Product = product;

            $scope.subProductOptions.DataList = product.ChildSettings;
            $scope.updateSelectedSubProduct($scope.subProductOptions.DataList[0]);
        };

        $scope.updateSelectedSubProduct = function (subProduct) {
            $scope.communicationLog.SubProduct = subProduct;
            if (!subProduct.ChildSettings) {
                var notApplySetting = $scope.subProductOptions.DataList[0];
                subProduct.ChildSettings = [{ Id: notApplySetting.Id, Name: notApplySetting.Name }];
            }

            $scope.betTypeList = subProduct.ChildSettings;
            $scope.communicationLog.BetType = $scope.betTypeList[0];
            $scope.$apply();
            $scope.updateChangeFlag();
        };

        $scope.updateStartTime = function (selectedTime) {
            $scope.communicationLog.StartTime = selectedTime;
            if (new Date($scope.communicationLog.EndTime) <= selectedTime) {
                $scope.communicationLog.EndTime = new Date(selectedTime);
                $scope.communicationLog.EndTime.setMinutes(selectedTime.getMinutes() + 1);
            }

            $scope.updateRemarkPrefix();
            $scope.updateChangeFlag();
        };
        $scope.updateEndTime = function (selectedTime) {
            $scope.communicationLog.EndTime = selectedTime;
            if (selectedTime <= new Date($scope.communicationLog.StartTime)) {
                $scope.communicationLog.StartTime = new Date(selectedTime);
                $scope.communicationLog.StartTime.setMinutes(selectedTime.getMinutes() - 1);
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
                $scope.communicationLog.CommunicationTool = communicationToolInfo;
            }

            $scope.updateRemarkPrefix();
            $scope.$apply();
            $scope.updateChangeFlag();
        };
        $scope.updateSelectedLanguage = function (languageId) {
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
                $scope.communicationLog.SubCategory = $scope.subCategoryList.length > 0 ?
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
                $scope.communicationLog.SubCategory = subCategoryInfo;
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
        $scope.updateSelectedSubject = function (subject) {
            $scope.communicationLog.Subject = subject;
            $scope.updateChangeFlag();
        };

        $scope.updateRemark = function (remark) {
            $scope.communicationLog.Remark = remark;
        };
        $scope.updateSolution = function (solution) {
            $scope.communicationLog.Solution = solution;
            $scope.updateChangeFlag();
        };
        $scope.updateEditFlag = function () {
            $scope.isEditingRelatedCustomer = true;
        };
        $scope.insertLog = function () {
            $scope.isShowResultMessage = false;
            var relatedCustomer = getRelatedCustomer();
            var inputCustomerLength = getRelatedCustomerLength();
            if ($scope.isEditingRelatedCustomer &&
            inputCustomerLength) {
                /* jshint camelcase: false */
                $scope.isShowResultMessage = true;
                $scope.isSuccess = false;
                $scope.resultMessage = viewResources.Communication_Logs_VerifyRelatedCustomer;
                return;
            }

            $scope.isShowResultMessage = false;
            $scope.isSuccess = true;
            var insertCriteria = {
                CustomerId: customerId,
                RelatedCustomerId: relatedCustomer,
                LanguageId: $scope.communicationLog.LanguageId,
                CountryId: $scope.communicationLog.Country.Id,
                StartTime: moment(new Date($scope.communicationLog.StartTime)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment),
                EndTime: moment(new Date($scope.communicationLog.EndTime)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment),
                CommunicationToolId: $scope.communicationLog.CommunicationTool.Id,
                ExtensionId: $scope.communicationLog.Extension.Id,
                MainCategoryId: $scope.communicationLog.MainCategory.Id,
                SubcategoryId: $scope.communicationLog.SubCategory.Id,
                SiteId: $scope.communicationLog.SiteInfo.Id,
                RoleId: $scope.communicationLog.RoleInfo.Id,
                ProductId: $scope.communicationLog.Product.Id,
                SubProductId: $scope.communicationLog.SubProduct.Id,
                BetTypeId: $scope.communicationLog.BetType.Id,
                ResponseId: $scope.communicationLog.Response.Id,
                StatusId: $scope.communicationLog.Status.Id,
                Remark: $scope.communicationLog.Remark,
                Solution: $scope.communicationLog.Solution,
                Subject: $scope.communicationLog.Subject,
                LicenseeTypeId: $scope.communicationLog.LicenseeType.Id
            };

            if ($scope.validateInsertLogCriteria(insertCriteria)) {
                $scope.BlockUI();
                $insertLogService.insertLog(insertCriteria)
            .then(function (data) {
                var insertResult = data.Result;
                $scope.showInsertResult(insertResult);
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
            }
        };
        $scope.showInsertResult = function (insertResult) {
            $scope.oldCommunicationLog = angular.copy($scope.communicationLog);
            if (insertResult.Code !== 0) {
                $scope.resultMessage = insertResult.Message;
                $scope.isShowResultMessage = true;
                $scope.isSuccess = false;
                $scope.unblockUI();
                return;
            }

            parent.homeManager.CachedData.shareInfoBetweenPopup = insertResult;
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
            $scope.communicationLog.LicenseeType.Id !== $scope.oldCommunicationLog.LicenseeType.Id ||

            $scope.communicationLog.CommunicationTool.Id !== $scope.oldCommunicationLog.CommunicationTool.Id ||
            $scope.communicationLog.Extension.Id !== $scope.oldCommunicationLog.Extension.Id ||
            $scope.communicationLog.MainCategory.Id !== $scope.oldCommunicationLog.MainCategory.Id ||
            $scope.communicationLog.SubCategory.Id !== $scope.oldCommunicationLog.SubCategory.Id ||
            $scope.communicationLog.SiteInfo.Id !== $scope.oldCommunicationLog.SiteInfo.Id ||
            $scope.communicationLog.RoleInfo.Id !== $scope.oldCommunicationLog.RoleInfo.Id ||
            $scope.communicationLog.Product.Id !== $scope.oldCommunicationLog.Product.Id ||
            $scope.communicationLog.SubProduct.Id !== $scope.oldCommunicationLog.SubProduct.Id ||
            $scope.communicationLog.BetType.Id !== $scope.oldCommunicationLog.BetType.Id ||
            $scope.communicationLog.Response.Id !== $scope.oldCommunicationLog.Response.Id ||
            $scope.communicationLog.Status.Id !== $scope.oldCommunicationLog.Status.Id ||
            $scope.communicationLog.Remark !== $scope.oldCommunicationLog.Remark ||
            $scope.communicationLog.Solution !== $scope.oldCommunicationLog.Solution ||
            $scope.communicationLog.Subject !== $scope.oldCommunicationLog.Subject) {
                parent.homeManager.CachedData.isChangingDataPopUp = true;
                parent.homeManager.CachedData.ignoreConfirmMessagePopUp = viewResources.Confirm_Ignore_InsertCommunication_Changes;
            } else {
                parent.homeManager.CachedData.isChangingDataPopUp = false;
            }
        };
        $scope.validateInsertLogCriteria = function (criteria) {
            var result = true;
            var feedbackTime = new Date(criteria.StartTime);
            var resolutionTime = new Date(criteria.EndTime);
            if (feedbackTime > resolutionTime) {
                $scope.resultMessage = viewResources.Communication_Log_ErrorEndTime;
                result = false;
            }

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
        $scope.verifyRelatedCustomer = function () {
            verifyRelatedCustomer($scope);
            $scope.isVerifyRelatedCustomer = true;
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
        $scope.getAllSettingForInsert = function (custId) {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $insertLogService.getAllSettingForInsert(custId)
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
                $scope.defaultSettings = allSetting.DefaultSettings;
                $scope.licenseeTypes = allSetting.LicenseeType;

                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.completedLoadFilterInfo();
            });
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
                Id: "CustomerCommunicationLogs_Insert"
            });

            hotKeyManager.CachedData.isBubbleToParent = false;
        });
    } ]);
})();