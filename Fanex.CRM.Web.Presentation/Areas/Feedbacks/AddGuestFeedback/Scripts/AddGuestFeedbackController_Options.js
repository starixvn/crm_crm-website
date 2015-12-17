(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global crmConstants,
    currentDate,
    currentTime,
    sharedScriptManager,
    viewResources */
    /* exported registerOptions */
    window.registerOptions = function ($scope) {
        $scope.titleOptions = null;
        $scope.birthdayOptions = null;
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
        $scope.subjectOptions = null;
        $scope.descriptionOptions = null;
        $scope.solutionOptions = null;
        $scope.tipsyOptions = null;
        $scope.relatedCustomerOptions = null;
        $scope.licenseeTypes = [];

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
            $scope.birthdayOptions = {
                CurrentDate: currentDate,
                InitDate: null,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                YearRange: sharedScriptManager.cachedData.yearRangeDatePicker,
                OnChangeDateEvent: $scope.updateBirthday,
                CompleteRender: $scope.completeRenderBirthday
            };

            var starttime = sharedScriptManager.newFeedbackTime(currentTime);
            var endtime = sharedScriptManager.newFeedbackTime(currentTime);
            endtime.setMinutes(endtime.getMinutes() + 1);
            $scope.addGuestFeedbackParams.StartTime = sharedScriptManager.formatTimeToSendToServer(starttime);
            $scope.addGuestFeedbackParams.EndTime = sharedScriptManager.formatTimeToSendToServer(endtime);

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
            $scope.descriptionOptions = {
                OnChangeEvent: $scope.updateDescription,
                SelectedText: $scope.addGuestFeedbackParams.Description,
                EditIconText: viewResources.Insert_Edit_Block
            };
            $scope.solutionOptions = {
                OnChangeEvent: $scope.updateSolution,
                SelectedText: $scope.addGuestFeedbackParams.Solution,
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
    };
})();
