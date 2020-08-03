(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global crmConstants, sharedScriptManager, moment, viewResources, MessageResult */
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.updateSelectedSubject = function (subject) {
            $scope.addCustomerFeedbackParams.Subject = subject;
            $scope.safeApply();
        };
        $scope.updateBirthday = function (birthday) {
            $scope.customerInfo.BirthDay = sharedScriptManager.formatTimeToSendToServer(birthday);
            $scope.customerInfo.BirthDayString = moment(new Date(birthday)).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
        };
        $scope.updateDescription = function (description) {
            $scope.descriptionOptions.SelectedText = description;
            $scope.addCustomerFeedbackParams.Description = description;
        };
        $scope.updateSolution = function (solution) {
            $scope.solutionOptions.SelectedText = solution;
            $scope.addCustomerFeedbackParams.Solution = solution;
        };
        $scope.updateSelectedTitle = function (title) {
            $scope.customerInfo.Title = title;
            $scope.safeApply();
        };
        $scope.updateSelectedCommunicationTool = function (communicationTool) {
            $scope.addCustomerFeedbackParams.CommunicationTool = communicationTool;
            $scope.updateTimeAndToolPrefix();
            $scope.safeApply();
        };
        $scope.updateSelectedLanguage = function (language) {
            $scope.addCustomerFeedbackParams.Language = language;
        };
        $scope.updateSelectedResponse = function (response) {
            $scope.addCustomerFeedbackParams.Response = response;
        };
        $scope.updateSelectedStatus = function (status) {
            $scope.addCustomerFeedbackParams.Status = status;
        };
        $scope.updateSelectedCountry = function (country) {
            $scope.addCustomerFeedbackParams.Country = country;
        };
        $scope.updateSelectedProvince = function (province) {
            $scope.addCustomerFeedbackParams.Province = province;
        };
        $scope.updateSelectedRole = function (role) {
            $scope.addCustomerFeedbackParams.Role = role;
        };

        $scope.completeRenderBirthday = function () {
            $scope.birthdayOptions.IsRerenderPicker = false;
        };
    };
})();