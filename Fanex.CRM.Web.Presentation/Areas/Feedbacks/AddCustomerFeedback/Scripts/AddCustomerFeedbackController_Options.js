"use strict";
/* jshint unused: vars,
camelcase: false */
/* global crmConstants,
currentDate,
currentTime,
sharedScriptManager,
viewResources */
/* exported registerOptions */
function registerOptions($scope) {
    $scope.titleOptions = null;
    $scope.siteOptions = null;
    $scope.birthdayOptions = null;
    $scope.startTimeOptions = null;
    $scope.endTimeOptions = null;
    $scope.communicationToolOptions = null;
    $scope.productOptions = null;
    $scope.responseOptions = null;
    $scope.languageOptions = null;

    $scope.mainCategoryOptions = null;
    $scope.betTypeOptions = null;
    $scope.statusOptions = null;

    $scope.extensionOptions = null;
    $scope.subCategoryOptions = null;
    $scope.countryOptions = null;
    $scope.roleOptions = null;
    $scope.subjectOptions = null;
    $scope.descriptionOptions = null;
    $scope.solutionOptions = null;
    $scope.tipsyOptions = null;
    $scope.relatedCustomerOptions = null;

    $scope.IsUpdatingTime = false;
    $scope.isSetFocus = false;
    $scope.messageResultOptions = {
        Message: '',
        IsShowMessage: false,
        IsSuccess: false
    };

    $scope.initDefaultOption = function () {
        $scope.titleOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedTitle,
            AllText: viewResources.All
        };
        $scope.siteOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSite,
            AllText: viewResources.Select,
            IsIncludeNA: true
        };
        $scope.birthdayOptions = {
            CurrentDate: currentDate,
            InitDate: null,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            YearRange: sharedScriptManager.cachedData.yearRangeDatePicker,
            OnChangeDateEvent: $scope.updateBirthday,
            CompleteRender: $scope.completeRenderBirthday
        };

        $scope.addCustomerFeedbackParams.StartTime = sharedScriptManager.newFeedbackTime(currentTime);
        $scope.addCustomerFeedbackParams.EndTime = sharedScriptManager.newFeedbackTime(currentTime);
        $scope.addCustomerFeedbackParams.EndTime.setMinutes($scope.addCustomerFeedbackParams.StartTime.getMinutes() + 1);
        $scope.startTimeOptions = {
            OnChangeEvent: $scope.updateStartTime,
            SelectedTime: $scope.addCustomerFeedbackParams.StartTime,
            CurrentTime: sharedScriptManager.newFeedbackTime(currentTime)
        };
        $scope.endTimeOptions = {
            OnChangeEvent: $scope.updateEndTime,
            SelectedTime: $scope.addCustomerFeedbackParams.EndTime,
            CurrentTime: sharedScriptManager.newFeedbackTime(currentTime)
        };
        $scope.communicationToolOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedCommunicationTool
        };
        $scope.productOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedProduct
        };
        $scope.responseOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedResponse
        };
        $scope.languageOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedLanguage
        };
        $scope.mainCategoryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedMainCategory
        };
        $scope.betTypeOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedBetType
        };
        $scope.statusOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedStatus
        };
        $scope.extensionOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedExtension
        };
        $scope.subCategoryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSubCategory
        };
        $scope.countryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedCountry
        };
        $scope.roleOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedRole
        };
        $scope.descriptionOptions = {
            OnChangeEvent: $scope.updateDescription,
            SelectedText: $scope.addCustomerFeedbackParams.Description,
            EditIconText: viewResources.Insert_Edit_Block
        };
        $scope.solutionOptions = {
            OnChangeEvent: $scope.updateSolution,
            SelectedText: $scope.addCustomerFeedbackParams.Solution,
            EditIconText: viewResources.Insert_Edit_Block
        };

        $scope.tipsyOptions = {
            Gravity: 'nw',
            Title: viewResources.Helper_RelatedInputMessage,
            IsUpdateTip: true
        };
        $scope.relatedCustomerOptions = {
            SuggestLink: window.rootUrl + 'CustomerSetting/RelatedCustomerAutoSuggestion',
            VerifyUsernameLink: window.rootUrl + 'CustomerSetting/VerifyUsername',
            MaxItems: crmConstants.Enums.RelatedCustomerSetting.MaxItems,
            IsUpdate: true,
            CompleteUpdate: $scope.completeUpdateRelatedCustomer,
            OnChangeEvent: $scope.updateRelatedCustomer
        };
    };
}
