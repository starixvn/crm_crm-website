(function () {
    "use strict";
    /* global 
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

    angular.module('feedbacks-addguestfeedback-application').controller('addGuestFeedbackController',
    ['$scope', '$uiBlockerService', 'addGuestFeedbackService',
    function ($scope, $uiBlockerService, addGuestFeedbackService) {
        $scope.customerInfo = {
            Name: '',
            Title: {
                Id: 0
            },
            Passport: {
                Name: ''
            },
            PersonalInfoContacts: [{ Name: '', Phone: '' }, { Name: '', Phone: '' }, { Name: '', Phone: '' }, { Name: '', Phone: ''}],
            BettingCode: ["", ""],
            PhoneInfo: [{
                CountryCode: "",
                AreaCode: "",
                PhoneNumber: ""
            }, {
                CountryCode: "",
                AreaCode: "",
                PhoneNumber: ""
            }, {
                CountryCode: "",
                AreaCode: "",
                PhoneNumber: ""
            }, {
                CountryCode: "",
                AreaCode: "",
                PhoneNumber: ""
            }],
            MobilePhone: {
                CountryCode: "",
                AreaCode: "",
                PhoneNumber: ""
            },
            FaxInfo: {
                CountryCode: "",
                AreaCode: "",
                FaxNumber: ""
            }
        };

        $scope.relatedCustomers = [];
        $scope.customersInput = '';

        $scope.isShowCustomerDetailBox = true;
        $scope.isShowFeedbackDetailBox = true;
        $scope.isShowDescriptionBox = true;
        $scope.isShowSolutionBox = true;
        $scope.isShowDescriptionPreview = false;
        $scope.isShowSolutionPreview = false;
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.isLoadedAllSettings = false;
        $scope.isLoading = true;
        $scope.isEditingRelatedCustomer = false;
        $scope.inputRelatedCustomerLength = 0;
        $scope.allSettings = null;

        $scope.lastRemarkPrefix = '';
        $scope.addGuestFeedbackParams = {
            OverrideCRMInfo: false,
            Subject: null,
            StartTime: null,
            EndTime: null,
            Description: '',
            Solution: '',
            HasRelated: false,
            CustomerInfo: $scope.customerInfo,
            Language: {
                Id: 0
            },
            Country: {
                Id: 0
            }
        };
        $scope.oldParams = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();
            $scope.getNumberOfFeedbackSubjectTemplate();
            $scope.getAllSetting();
        };
        $scope.getAllSetting = function () {
            $uiBlockerService.blockUI();
            var params = {};
            addGuestFeedbackService.getAllSetting(params)
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
        $scope.completedLoadFilterInfo = function () {
            if (!$scope.isLoadedAllSettings ||
                !$scope.isLoadedFeedbackSubjectTemplate) {
                return;
            }

            $scope.birthdayOptions.IsRerenderPicker = true;
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

            $scope.subjectOptions = {
                NumberOfSuggestItems: 15,
                SubjectList: $scope.feedbackSubjectTemplateList,
                Subject: '',
                IsUpdate: true,
                OnChangeEvent: $scope.updateSelectedSubject
            };

            $scope.descriptionOptions.SelectedText = '';
            $scope.solutionOptions.SelectedText = '';

            $scope.updateSelectedCommunicationTool($scope.communicationToolOptions.DataList[0]);

            var firstProduct = $scope.productOptions.DataList[0];
            $scope.updateSelectedProduct($scope.productOptions.DataList[0]);
            $scope.updateSelectedSubProduct(firstProduct.ChildSettings[0]);

            $scope.updateSelectedResponse($scope.responseOptions.DataList[0]);

            $scope.updateSelectedMainCategory($scope.mainCategoryOptions.DataList[0]);
            $scope.updateSelectedBetType($scope.betTypeOptions.DataList[0]);
            $scope.updateSelectedStatus($scope.statusOptions.DataList[0]);
            $scope.updateSelectedExtension($scope.extensionOptions.DataList[0]);
            $scope.updateSelectedSubCategory($scope.subCategoryOptions.DataList[0]);
            $scope.addGuestFeedbackParams.CustomerInfo = $scope.customerInfo;
            $scope.updateRemarkPrefix();
            $scope.customerInfo.Title = $scope.allSettings.TitleList[0];
            $scope.customerInfo.LicenseeType = $scope.allSettings.LicenseeType[0];

            $scope.oldParams = angular.copy($scope.addGuestFeedbackParams);
            $scope.updateChangeFlag();
            $scope.isLoading = false;
            $uiBlockerService.unblockUI();
        };
        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            $uiBlockerService.blockUI();
            addGuestFeedbackService.getNumberOfFeedbackSubjectTemplate()
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
            addGuestFeedbackService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
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

        // Functions of Related customers.
        $scope.verifyUsernames = function (username) {
            if (username !== '') {
                username = username.trim().replace(/,\s+/g, ",").replace(/^,+|,+$/g, "").replace(/,{2,}/g, ",");
                $uiBlockerService.blockUI();
                addGuestFeedbackService.verifyUsernames({ Username: username })
                .then(function (data) {
                    var customers = [];
                    angular.forEach(data.Result, function (customer, index) {
                        /* jshint unused: false */
                        customers.push({ Id: customer.Id, Name: customer.Name });
                    });

                    $scope.relatedCustomers = $scope.getRelatedCustomers(customers);
                    $scope.customersInput = '';

                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
            }
        };

        $scope.getRelatedCustomers = function (customerList) {
            var customers = $scope.relatedCustomers.concat(customerList);
            // unique customer.
            for (var i = 0; i < customers.length; ++i) {
                for (var j = i + 1; j < customers.length; ++j) {
                    if (customers[i].Id === customers[j].Id) {
                        customers.splice(j--, 1);
                    }
                }
            }

            return customers;
        }

        $scope.getRelatedCustomerId = function () {
            var ids = "";
            if ($scope.addGuestFeedbackParams.HasRelated && $scope.relatedCustomers.length > 0) {
                var relatedCustomerIds = [];
                angular.forEach($scope.relatedCustomers, function (customer, index) {
                    /* jshint unused: false */
                    relatedCustomerIds.push(customer.Id);
                });

                ids = relatedCustomerIds.join(',');
            }

            return ids;
        }
        // ---------- End of Functions of Related customers ----------

        $scope.addFeedback = function () {
            $uiBlockerService.blockUI();
            $scope.addGuestFeedbackParams.RelatedCustomerId = $scope.getRelatedCustomerId();
            var params = angular.copy($scope.addGuestFeedbackParams);
            params.CustomerInfo = $scope.customerInfo;
            params.StartTime = sharedScriptManager.formatTimeToSendToServer($scope.addGuestFeedbackParams.StartTime);
            params.EndTime = sharedScriptManager.formatTimeToSendToServer($scope.addGuestFeedbackParams.EndTime);
            if (!$scope.validateAddParams(params)) {
                $uiBlockerService.unblockUI();
                return;
            }

            addGuestFeedbackService.addFeedback(params)
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

            if (result && $.trim(params.CustomerInfo.Name) === '') {
                message = viewResources.Error_EmptyId;
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
            $scope.oldParams = angular.copy($scope.addGuestFeedbackParams);
            $scope.updateChangeFlag();
            if (addResult.Code !== 0) {
                $scope.messageResultOptions = new MessageResult(addResult.Message, true, false);
                $uiBlockerService.unblockUI();
                return;
            }

            if (addResult.Code === 0) {
                $scope.messageResultOptions = new MessageResult(addResult.Message, true, true);
                setTimeout(function () {
                    $scope.messageResultOptions = new MessageResult('', false, false);
                    $scope.safeApply();

                    parent.leftPanelManager.DOMElements.checkboxIsGuest.attr('checked', true);
                    parent.leftPanelManager.loadLeftPanel($scope.customerInfo.Name, true);
                }, sharedScriptManager.cachedData.alertSuccessTime);

                $uiBlockerService.unblockUI();
                return;
            }

            $uiBlockerService.unblockUI();
        };
        $scope.setFocus = function () {
            $scope.isSetFocus = true;
            $scope.safeApply();
        };
        $scope.updateChangeFlag = function () {
            if ($scope.isParamsChange()) {
                parent.homeManager.CachedData.isChangingData = true;
                parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_Feedback_AddGuestFeedback;
            } else {
                parent.homeManager.CachedData.isChangingData = false;
            }

            $scope.safeApply();
        };
        $scope.isParamsChange = function () {
            return !($scope.oldParams &&
                $scope.addGuestFeedbackParams &&
                $scope.oldParams.CustomerInfo &&
                $scope.oldParams.CustomerInfo.Title.Id === $scope.addGuestFeedbackParams.CustomerInfo.Title.Id &&
                $scope.oldParams.CustomerInfo.FirstName === $scope.addGuestFeedbackParams.CustomerInfo.FirstName &&
                $scope.oldParams.CustomerInfo.LastName === $scope.addGuestFeedbackParams.CustomerInfo.LastName &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.PhoneInfo, $scope.addGuestFeedbackParams.CustomerInfo.PhoneInfo) &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.FaxInfo, $scope.addGuestFeedbackParams.CustomerInfo.FaxInfo) &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.MobilePhone, $scope.addGuestFeedbackParams.CustomerInfo.MobilePhone) &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.PersonalInfoContacts,
                    $scope.addGuestFeedbackParams.CustomerInfo.PersonalInfoContacts) &&
                $scope.oldParams.CustomerInfo.Skype === $scope.addGuestFeedbackParams.CustomerInfo.Skype &&
                $scope.oldParams.CustomerInfo.Email === $scope.addGuestFeedbackParams.CustomerInfo.Email &&
                $scope.oldParams.CustomerInfo.MSN === $scope.addGuestFeedbackParams.CustomerInfo.MSN &&

                $scope.oldParams.CustomerInfo.Passport.Name === $scope.addGuestFeedbackParams.CustomerInfo.Passport.Name &&
                $scope.oldParams.CustomerInfo.BirthDayString === $scope.addGuestFeedbackParams.CustomerInfo.BirthDayString &&
                sharedScriptManager.deepCompare($scope.oldParams.CustomerInfo.BettingCode, $scope.addGuestFeedbackParams.CustomerInfo.BettingCode) &&
                $scope.oldParams.CustomerInfo.IsVIP === $scope.addGuestFeedbackParams.CustomerInfo.IsVIP &&

                $scope.oldParams.CommunicationTool.Id === $scope.addGuestFeedbackParams.CommunicationTool.Id &&
                $scope.oldParams.Product.Id === $scope.addGuestFeedbackParams.Product.Id &&
                $scope.oldParams.Language.Id === $scope.addGuestFeedbackParams.Language.Id &&
                $scope.oldParams.Response.Id === $scope.addGuestFeedbackParams.Response.Id &&
                $scope.oldParams.MainCategory.Id === $scope.addGuestFeedbackParams.MainCategory.Id &&
                $scope.oldParams.BetType.Id === $scope.addGuestFeedbackParams.BetType.Id &&
                $scope.oldParams.Status.Id === $scope.addGuestFeedbackParams.Status.Id &&
                $scope.oldParams.Extension.Id === $scope.addGuestFeedbackParams.Extension.Id &&
                $scope.oldParams.SubCategory.Id === $scope.addGuestFeedbackParams.SubCategory.Id &&
                $scope.oldParams.Country.Id === $scope.addGuestFeedbackParams.Country.Id &&
                $scope.oldParams.RelatedCustomerId === $scope.addGuestFeedbackParams.RelatedCustomerId &&
                $scope.oldParams.Description === $scope.addGuestFeedbackParams.Description &&
                $scope.oldParams.Solution === $scope.addGuestFeedbackParams.Solution);
        };
        $scope.updateRemarkPrefix = function () {
            var remarkPrefix = $scope.generateRemarkPrefix();
            if ($scope.addGuestFeedbackParams.Description.indexOf($scope.lastRemarkPrefix) === 0) {
                $scope.addGuestFeedbackParams.Description =
                    $scope.addGuestFeedbackParams.Description.substring($scope.lastRemarkPrefix.length, $scope.addGuestFeedbackParams.Description.length);
                $scope.descriptionOptions.SelectedText = $scope.addGuestFeedbackParams.Description;
            }

            if ($scope.addGuestFeedbackParams.Description.indexOf(remarkPrefix) === -1) {
                $scope.addGuestFeedbackParams.Description = remarkPrefix + $scope.addGuestFeedbackParams.Description;
                $scope.lastRemarkPrefix = remarkPrefix;
                $scope.descriptionOptions.SelectedText = $scope.addGuestFeedbackParams.Description;
            }
        };
        $scope.generateRemarkPrefix = function () {
            return sharedScriptManager.stringFormat("{0}/{1} ",
            [moment($scope.addGuestFeedbackParams.StartTime).format(crmConstants.Enums.FormatConstant.StartTime_RemarkPrefix),
                $scope.addGuestFeedbackParams.CommunicationTool.Name]);
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
