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

        $scope.multipleLogs = {
            StartTime: new Date(currentTime),
            EndTime: new Date(currentTime),
            CommunicationTool: {
                Id: 0,
                Value: ''
            },
            Extension: {
                Id: 0,
                Value: ''
            },
            Country: {
                Id: 0
            },
            LanguageId: 0,
            MainCategory: {
                Name: ''
            },
            SubCategory: {
                Name: ''
            },
            Product: {
                Id: 0,
                Value: ''
            },
            SubProduct: {
                Id: 0,
                Value: ''
            },
            Status: {
                Id: 0,
                Value: ''
            },
            BetType: {
                Id: 0,
                Value: ''
            },
            RoleInfo: {
                Id: 0,
                Value: ''
            },
            SiteInfo: {
                Id: 0,
                Value: ''
            },
            LicenseeType: {
                Id: 0
            },
            Response: {
                Id: 0,
                Value: ''
            },
            Subject: '',
            Remark: '',
            Solution: ''
        };
        $scope.oldmultipleLogs = angular.copy($scope.multipleLogs);
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

            $scope.subCategoryList = $scope.mainCategoryList[0].ChildSettings;
            var starttime = sharedScriptManager.newFeedbackTime(currentTime);
            var endtime = sharedScriptManager.newFeedbackTime(currentTime);
            endtime.setMinutes(endtime.getMinutes() + 1);
            $scope.multipleLogs.StartTime = sharedScriptManager.formatTimeToSendToServer(starttime);
            $scope.multipleLogs.EndTime = sharedScriptManager.formatTimeToSendToServer(endtime);

            $scope.multipleLogs.Extension = $scope.extensionList[0];
            $scope.multipleLogs.CommunicationTool = $scope.communicationToolList[0];
            $scope.multipleLogs.MainCategory = $scope.mainCategoryList[0];
            $scope.multipleLogs.SubCategory = $scope.subCategoryList[0];
            $scope.multipleLogs.Status = $scope.statusList[0];
            $scope.multipleLogs.RoleInfo = $scope.roleList[0];
            $scope.multipleLogs.SiteInfo = $scope.siteList[0];
            $scope.multipleLogs.Response = $scope.responseList[0];

            $scope.updateRemarkPrefix();
            $scope.oldmultipleLogs = angular.copy($scope.multipleLogs);
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
        };
        $scope.updateStartTime = function (selectedTime) {
            $scope.multipleLogs.StartTime = selectedTime;
            if (new Date($scope.multipleLogs.EndTime) <= selectedTime) {
                $scope.multipleLogs.EndTime = new Date(selectedTime);
                $scope.multipleLogs.EndTime.setMinutes(selectedTime.getMinutes() + 1);
            }

            $scope.updateRemarkPrefix();
        };
        $scope.updateEndTime = function (selectedTime) {
            $scope.multipleLogs.EndTime = selectedTime;
            if (selectedTime <= new Date($scope.multipleLogs.StartTime)) {
                $scope.multipleLogs.StartTime = new Date(selectedTime);
                $scope.multipleLogs.StartTime.setMinutes(selectedTime.getMinutes() - 1);
            }

            $scope.updateRemarkPrefix();
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
        };
        $scope.updateSelectedLanguage = function (languageId) {
            $scope.multipleLogs.LanguageId = languageId;
            $scope.$apply();
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
                $scope.multipleLogs.SiteInfo = siteInfo;
            } else {
                $scope.multipleLogs.SiteInfo = {
                    Id: crmConstants.Enums.Site.NotApply,
                    Value: viewResources.Select
                };
            }
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
            }
        };
        $scope.updateSelectedSubject = function (subject) {
            $scope.multipleLogs.Subject = subject;
        };
        $scope.updateRemark = function (remark) {
            $scope.multipleLogs.Remark = remark;
        };
        $scope.updateSolution = function (solution) {
            $scope.multipleLogs.Solution = solution;
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
                LanguageId: $scope.multipleLogs.LanguageId,
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

            if (result.Code !== 0) {
                $scope.isSuccess = false;
                $scope.unblockUI();
                return;
            }

            parent.homeManager.CachedData.shareInfoBetweenPopup = result;
            parent.homeManager.CachedData.shouldReloadView = true;
            $scope.unblockUI();

            setTimeout(function () {
                parent.ClosePopup();
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };
        $scope.updateChangeFlag = function () {
            if ($scope.isParamsChange()) {
                parent.homeManager.CachedData.isChangingData = true;
                parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_MultipleLogs_Changes;
            } else {
                parent.homeManager.CachedData.isChangingData = false;
            }

            $scope.safeApply();
        };
        $scope.isParamsChange = function () {
            return $scope.multipleLogs.Extension.Id !== $scope.oldmultipleLogs.Extension.Id ||
                $scope.multipleLogs.Country.Id !== $scope.oldmultipleLogs.Country.Id ||
                $scope.multipleLogs.CommunicationTool.Id !== $scope.oldmultipleLogs.CommunicationTool.Id ||
                $scope.multipleLogs.LanguageId !== $scope.oldmultipleLogs.LanguageId ||
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
                Id: "MultipleLogs_Edit"
            });

            hotKeyManager.CachedData.isBubbleToParent = false;
        });
    }]);
})();