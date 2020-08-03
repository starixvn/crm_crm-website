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
        $scope.responseOptions = null;
        $scope.languageOptions = null;

        $scope.statusOptions = null;

        $scope.countryOptions = null;
        $scope.provinceOptions = null;
        $scope.subjectOptions = null;
        $scope.descriptionOptions = null;
        $scope.solutionOptions = null;
        $scope.licenseeTypes = [];

        $scope.IsUpdatingTime = false;
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
            $scope.responseOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedResponse
            };
            $scope.languageOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedLanguage
            };
            $scope.statusOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedStatus
            };
            $scope.countryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCountry
            };
            $scope.provinceOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedProvince
            }
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
        };
    };
})();