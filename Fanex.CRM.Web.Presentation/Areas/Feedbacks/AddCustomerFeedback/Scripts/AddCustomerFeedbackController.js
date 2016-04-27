(function () {
    "use strict";
    /* global
    SettingManager,
    MessageResult,
    registerOptions,
    registerUpdateFunctions,
    registerHotkeyFunctions,
    crmConstants,
    sharedScriptManager,
    viewResources,
    moment */
    /* jshint camelcase: false */
    window.MessageResult = function (message, isShowMessage, isSuccess) {
        this.Message = message;
        this.IsShowMessage = isShowMessage;
        this.IsSuccess = isSuccess;
    };

    angular.module('feedbacks-addcustomerfeedback-application').controller('addCustomerFeedbackController',
    ['$scope', '$uiBlockerService', 'addCustomerFeedbackService',
    function ($scope, $uiBlockerService, addCustomerFeedbackService) {
        $scope.customerInfo = {
            Id: parent.homeManager.CachedData.currentCustomer.Id,
            Name: parent.homeManager.CachedData.currentCustomer.Name
        };

        $scope.isShowCustomerDetailBox = false;
        $scope.isShowFeedbackDetailBox = true;
        $scope.isShowDescriptionBox = true;
        $scope.isShowSolutionBox = true;
        $scope.isShowDescriptionPreview = false;
        $scope.isShowSolutionPreview = false;
        $scope.isLoadedCustomerInfo = false;
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.isLoadedAllSettings = false;
        $scope.isLoading = true;
        $scope.isEditingRelatedCustomer = false;
        $scope.inputRelatedCustomerLength = 0;
        $scope.allSettings = null;

        $scope.lastRemarkPrefix = '';
        $scope.addCustomerFeedbackParams = {
            OverrideCRMInfo: false,
            Subject: null,
            StartTime: null,
            EndTime: null,
            Description: '',
            Solution: '',
            HasRelated: false,
            CustomerInfo: {
                TitleId: 0
            }
        };
        $scope.oldParams = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getNumberOfFeedbackSubjectTemplate();
            $scope.getAllSetting($scope.customerInfo.Id);
            $scope.getCustomerInfoById($scope.customerInfo.Id);
        };
        $scope.getAllSetting = function (custId) {
            $uiBlockerService.blockUI();
            var params = { custId: custId };
            addCustomerFeedbackService.getAllSetting(params)
                .then(function (data) {
                    $scope.isLoadedAllSettings = true;
                    $scope.allSettings = data.Result;
                    $scope.completedLoadFilterInfo();
                },
                function () {
                    $scope.isLoadedAllSettings = true;
                    $scope.completedLoadFilterInfo();
                });
        };
        $scope.getCustomerInfoById = function (custId) {
            $uiBlockerService.blockUI();

            var params = {
                CustomerId: custId
            };
            addCustomerFeedbackService.getCustomerInfoById(params)
                .then(function (data) {
                    $scope.isLoadedCustomerInfo = true;
                    var customerInfo = data.Result;
                    $scope.customerInfo = customerInfo;
                    $scope.completedLoadFilterInfo();
                },
                function () {
                    $scope.isLoadedCustomerInfo = true;
                    $scope.completedLoadFilterInfo();
                });
        };
        $scope.completedLoadFilterInfo = function () {
            if (!$scope.isLoadedCustomerInfo ||
                !$scope.isLoadedAllSettings ||
                !$scope.isLoadedFeedbackSubjectTemplate) {
                return;
            }

            $scope.allSettings.SiteList = SettingManager.updateSettingSelectListWithFullData(
                                                        $scope.customerInfo.SiteSetting,
                                                        $scope.allSettings.SiteList);

            $scope.titleOptions.SelectedId = $scope.customerInfo.Title.Id;
            $scope.birthdayOptions.InitDate = new Date($scope.customerInfo.BirthDayString);
            $scope.birthdayOptions.IsRerenderPicker = true;

            $scope.communicationToolOptions.SelectedId = $scope.allSettings.CommunicationToolList[0].Id;
            var defaultLanguage = sharedScriptManager.getDefaultCustomerLangue($scope.customerInfo.Languages) || $scope.allSettings.LanguageList[0];
            $scope.languageOptions.SelectedId = defaultLanguage.Id;
            $scope.countryOptions.SelectedId = $scope.customerInfo.Country.Id;
            $scope.roleOptions.SelectedId = $scope.customerInfo.Role.Id;

            $scope.titleOptions.DataList = $scope.allSettings.TitleList;
            $scope.communicationToolOptions.DataList = $scope.allSettings.CommunicationToolList;
            $scope.productOptions.DataList = $scope.allSettings.ProductList;
            $scope.responseOptions.DataList = $scope.allSettings.ResponseList;
            $scope.languageOptions.DataList = $scope.allSettings.LanguageList;
            $scope.mainCategoryOptions.DataList = $scope.allSettings.MainCategoryList;
            $scope.betTypeOptions.DataList = $scope.allSettings.ProductList[0].ChildSettings;
            $scope.statusOptions.DataList = $scope.allSettings.StatusList;

            $scope.extensionOptions.DataList = $scope.allSettings.ExtensionList;
            $scope.subCategoryOptions.DataList = $scope.allSettings.MainCategoryList[0].ChildSettings;
            $scope.countryOptions.DataList = $scope.allSettings.CountryList;
            $scope.roleOptions.DataList = $scope.allSettings.RoleList;
            $scope.licenseeTypes = $scope.allSettings.LicenseeType;

            $scope.subjectOptions = {
                NumberOfSuggestItems: 15,
                SubjectList: $scope.feedbackSubjectTemplateList,
                Subject: '',
                IsUpdate: true,
                OnChangeEvent: $scope.updateSelectedSubject
            };

            $scope.descriptionOptions.SelectedText = '';
            $scope.solutionOptions.SelectedText = '';

            var firstProduct = $scope.productOptions.DataList[0];
            $scope.updateSelectedProduct(firstProduct);
            $scope.updateSelectedSubProduct(firstProduct.ChildSettings[0]);

            $scope.updateSelectedCommunicationTool($scope.communicationToolOptions.DataList[0]);
            $scope.updateSelectedLanguage(defaultLanguage);
            $scope.updateSelectedResponse($scope.responseOptions.DataList[0]);

            $scope.updateSelectedMainCategory($scope.mainCategoryOptions.DataList[0]);
            $scope.updateSelectedBetType($scope.betTypeOptions.DataList[0]);
            $scope.updateSelectedStatus($scope.statusOptions.DataList[0]);

            $scope.updateSelectedExtension($scope.extensionOptions.DataList[0]);
            $scope.updateSelectedSubCategory($scope.subCategoryOptions.DataList[0]);
            $scope.updateSelectedCountry($scope.customerInfo.Country);
            $scope.updateSelectedRole($scope.customerInfo.Role);

            $scope.addCustomerFeedbackParams.CustomerInfo = $scope.customerInfo;
            $scope.updateRemarkPrefix();

            $scope.oldParams = angular.copy($scope.addCustomerFeedbackParams);
            $scope.isLoading = false;
            $uiBlockerService.unblockUI();
        };
        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            $uiBlockerService.blockUI();
            addCustomerFeedbackService.getNumberOfFeedbackSubjectTemplate()
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
            $uiBlockerService.blockUI();
            addCustomerFeedbackService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
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

        $scope.updateSelectedSite = function () {
            $scope.customerInfo.SiteSetting = $scope.customerInfo.SiteSetting || { Id: 0 }
        }

        $scope.showHideCustomerDetailBox = function () {
            $scope.isShowCustomerDetailBox = !$scope.isShowCustomerDetailBox;
        };
        $scope.showHideFeedbackDetailBox = function () {
            $scope.isShowFeedbackDetailBox = !$scope.isShowFeedbackDetailBox;
        };
        $scope.showDescriptionPreview = function () {
            $scope.isShowDescriptionPreview = true;
        };
        $scope.showDescriptionEditor = function () {
            $scope.isShowDescriptionPreview = false;
        };
        $scope.showSolutionPreview = function () {
            $scope.isShowSolutionPreview = true;
        };
        $scope.showSolutionEditor = function () {
            $scope.isShowSolutionPreview = false;
        };

        $scope.verifyUserName = function () {
            $scope.relatedCustomerOptions.Action = "verifyUserName";
            $scope.relatedCustomerOptions.IsUpdate = true;
            $scope.isEditingRelatedCustomer = false;
            $scope.safeApply();
        };
        $scope.addFeedback = function () {
            $uiBlockerService.blockUI();
            var params = angular.copy($scope.addCustomerFeedbackParams);
            params.CustomerInfo = $scope.customerInfo;
            params.StartTime = sharedScriptManager.formatTimeToSendToServer($scope.addCustomerFeedbackParams.StartTime);
            params.EndTime = sharedScriptManager.formatTimeToSendToServer($scope.addCustomerFeedbackParams.EndTime);
            if (!$scope.validateAddParams(params)) {
                $uiBlockerService.unblockUI();
                return;
            }

            addCustomerFeedbackService.addFeedback(params)
                .then(function (data) {
                    var addResult = data.Result;
                    $scope.showAddResult(addResult);
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.validateAddParams = function (params) {
            var result = true;
            var message = '';
            var feedbackTime = new Date(params.StartTime);
            var resolutionTime = new Date(params.EndTime);
            if (feedbackTime > resolutionTime) {
                message = viewResources.ErrorFeedbackResolutionTime;
                result = false;
            }

            if (result && params.CommunicationTool.Id === crmConstants.Enums.CommunicationTool.NotApply) {
                message = viewResources.Error_NACommmunicationTool;
                result = false;
            }

            if (result && params.MainCategory.Id === crmConstants.Enums.MainCategory.NotApply) {
                message = viewResources.Error_NAMainCategory;
                result = false;
            }

            if (result &&
                (!params.MainCategory || params.MainCategory.ChildSettings.length === 0 || params.SubCategory.Id === crmConstants.Enums.SubCategory.NotApply)) {
                message = viewResources.Error_NASubCategory;
                result = false;
            }

            if (result && params.Language.Id === crmConstants.Enums.Language.NotApply) {
                message = viewResources.Error_NALanguage;
                result = false;
            }

            if (result && params.Subject === null || params.Subject === '') {
                message = viewResources.Error_EmptySubject;
                result = false;
            }

            if (result &&
                ($.trim(params.Description) === '' || params.Description === '<br>')) {
                message = viewResources.Error_EmptyRemark;
                result = false;
            }

            if (result &&
                ($.trim(params.Solution) === '' || params.Solution === '<br>')) {
                message = viewResources.Error_EmptySolution;
                result = false;
            }

            if (result &&
                $scope.isEditingRelatedCustomer &&
                $scope.inputRelatedCustomerLength) {
                message = viewResources.Error_VerifyRelatedCustomer;
                result = false;
            }

            $scope.messageResultOptions = new MessageResult(message, !result, result);
            return result;
        };
        $scope.showAddResult = function (addResult) {
            $scope.oldParams = angular.copy($scope.addCustomerFeedbackParams);
            if (addResult.Code !== 0) {
                $scope.messageResultOptions = new MessageResult(addResult.Message, true, false);
                $uiBlockerService.unblockUI();
                return;
            }

            if (addResult.Code === 0) {
                $scope.messageResultOptions = new MessageResult(addResult.Message, true, true);
                setTimeout(function () {
                    $uiBlockerService.unblockUI();
                    $scope.messageResultOptions = new MessageResult('', false, false);
                    $scope.safeApply();
                    parent.leftPanelManager.loadLeftPanel(null, true);
                }, sharedScriptManager.cachedData.alertSuccessTime);
                return;
            }
        };
        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.updateChangeFlag = function () {
            if ($scope.isParamsChange()) {
                parent.homeManager.CachedData.isChangingData = true;
                parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_Feedback_AddCustomerFeedback;
            } else {
                parent.homeManager.CachedData.isChangingData = false;
            }

            $scope.safeApply();
        };
        $scope.isParamsChange = function () {
            return !($scope.oldParams &&
                $scope.addCustomerFeedbackParams &&
                $scope.oldParams.CustomerInfo.Title.Id === $scope.addCustomerFeedbackParams.CustomerInfo.Title.Id &&
                $scope.oldParams.CustomerInfo.FirstName === $scope.addCustomerFeedbackParams.CustomerInfo.FirstName &&
                $scope.oldParams.CustomerInfo.LastName === $scope.addCustomerFeedbackParams.CustomerInfo.LastName &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.PhoneInfo, $scope.addCustomerFeedbackParams.CustomerInfo.PhoneInfo) &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.FaxInfo, $scope.addCustomerFeedbackParams.CustomerInfo.FaxInfo) &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.MobilePhone, $scope.addCustomerFeedbackParams.CustomerInfo.MobilePhone) &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.PersonalInfoContact,
                    $scope.addCustomerFeedbackParams.CustomerInfo.PersonalInfoContact) &&
                $scope.oldParams.CustomerInfo.Skype === $scope.addCustomerFeedbackParams.CustomerInfo.Skype &&
                $scope.oldParams.CustomerInfo.Email === $scope.addCustomerFeedbackParams.CustomerInfo.Email &&
                $scope.oldParams.CustomerInfo.MSN === $scope.addCustomerFeedbackParams.CustomerInfo.MSN &&

                $scope.oldParams.CustomerInfo.Passport.Name === $scope.addCustomerFeedbackParams.CustomerInfo.Passport.Name &&
                $scope.oldParams.CustomerInfo.BirthDayString === $scope.addCustomerFeedbackParams.CustomerInfo.BirthDayString &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.BettingCode, $scope.addCustomerFeedbackParams.CustomerInfo.BettingCode) &&
                $scope.oldParams.CustomerInfo.IsVIP === $scope.addCustomerFeedbackParams.CustomerInfo.IsVIP &&
                $scope.oldParams.CustomerInfo.SiteSetting.Id === $scope.addCustomerFeedbackParams.CustomerInfo.SiteSetting.Id &&

                $scope.oldParams.CommunicationTool.Id === $scope.addCustomerFeedbackParams.CommunicationTool.Id &&
                $scope.oldParams.Product.Id === $scope.addCustomerFeedbackParams.Product.Id &&
                $scope.oldParams.SubProduct.Id === $scope.addCustomerFeedbackParams.SubProduct.Id &&
                $scope.oldParams.Language.Id === $scope.addCustomerFeedbackParams.Language.Id &&
                $scope.oldParams.Response.Id === $scope.addCustomerFeedbackParams.Response.Id &&
                $scope.oldParams.MainCategory.Id === $scope.addCustomerFeedbackParams.MainCategory.Id &&
                $scope.oldParams.BetType.Id === $scope.addCustomerFeedbackParams.BetType.Id &&
                $scope.oldParams.Status.Id === $scope.addCustomerFeedbackParams.Status.Id &&
                $scope.oldParams.Extension.Id === $scope.addCustomerFeedbackParams.Extension.Id &&
                $scope.oldParams.SubCategory.Id === $scope.addCustomerFeedbackParams.SubCategory.Id &&
                $scope.oldParams.Country.Id === $scope.addCustomerFeedbackParams.Country.Id &&
                $scope.oldParams.Role.Id === $scope.addCustomerFeedbackParams.Role.Id &&
                $scope.oldParams.RelatedCustomerId === $scope.addCustomerFeedbackParams.RelatedCustomerId &&
                $scope.oldParams.Description === $scope.addCustomerFeedbackParams.Description &&
                $scope.oldParams.CustomerInfo.LicenseeType.Id === $scope.addCustomerFeedbackParams.CustomerInfo.LicenseeType.Id &&
                $scope.oldParams.Solution === $scope.addCustomerFeedbackParams.Solution);
        };
        $scope.updateRemarkPrefix = function () {
            var remarkPrefix = $scope.generateRemarkPrefix();
            if ($scope.addCustomerFeedbackParams.Description.indexOf($scope.lastRemarkPrefix) === 0) {
                $scope.addCustomerFeedbackParams.Description =
                    $scope.addCustomerFeedbackParams.Description.substring($scope.lastRemarkPrefix.length, $scope.addCustomerFeedbackParams.Description.length);
                $scope.descriptionOptions.SelectedText = $scope.addCustomerFeedbackParams.Description;
            }

            if ($scope.addCustomerFeedbackParams.Description.indexOf(remarkPrefix) === -1) {
                $scope.addCustomerFeedbackParams.Description = remarkPrefix + $scope.addCustomerFeedbackParams.Description;
                $scope.lastRemarkPrefix = remarkPrefix;
                $scope.descriptionOptions.SelectedText = $scope.addCustomerFeedbackParams.Description;
            }
        };
        $scope.generateRemarkPrefix = function () {
            return sharedScriptManager.stringFormat("{0}/{1} ",
            [moment($scope.addCustomerFeedbackParams.StartTime).format(crmConstants.Enums.FormatConstant.StartTime_RemarkPrefix),
                $scope.addCustomerFeedbackParams.CommunicationTool.Name]);
        };
        $scope.onStartTimeChanged = function () {
            $scope.$apply(function () {
                $scope.updateRemarkPrefix();
            });
        };
        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);

            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });

            $scope.initController();
            $scope.setFocus();
            $scope.registerHotKey();
        });
    } ]);
})();

