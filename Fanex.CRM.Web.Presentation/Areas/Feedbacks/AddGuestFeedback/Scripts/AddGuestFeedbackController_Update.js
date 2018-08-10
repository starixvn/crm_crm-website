(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global crmConstants,
    sharedScriptManager,
    moment,
    viewResources,
    MessageResult */
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.updateSelectedSubject = function (subject) {
            $scope.addGuestFeedbackParams.Subject = subject;
            $scope.safeApply();
        };
        $scope.updateBirthday = function (birthday) {
            $scope.customerInfo.BirthDay = birthday ? sharedScriptManager.formatTimeToSendToServer(birthday) : null;
            $scope.customerInfo.BirthDayString = birthday ?
            moment(new Date(birthday)).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment) :
            null;
        };
        $scope.updateDescription = function (description) {
            $scope.descriptionOptions.SelectedText = description;
            $scope.addGuestFeedbackParams.Description = description;
        };
        $scope.updateSolution = function (solution) {
            $scope.solutionOptions.SelectedText = solution;
            $scope.addGuestFeedbackParams.Solution = solution;
        };
        $scope.updateSelectedTitle = function (title) {
            $scope.customerInfo.Title = title;
            $scope.safeApply();
        };
        $scope.updateSelectedCommunicationTool = function (communicationTool) {
            $scope.addGuestFeedbackParams.CommunicationTool = communicationTool;
            $scope.updateTimeAndToolPrefix();
            $scope.safeApply();
        };
        $scope.updateSelectedLanguage = function (language) {
            $scope.addGuestFeedbackParams.Language = language;
        };
        $scope.updateSelectedResponse = function (response) {
            $scope.addGuestFeedbackParams.Response = response;
        };       
        $scope.updateSelectedStatus = function (status) {
            $scope.addGuestFeedbackParams.Status = status;
        };
        $scope.updateSelectedCountry = function (country) {
            $scope.addGuestFeedbackParams.Country = country;
        };
        $scope.completeRenderBirthday = function () {
            $scope.birthdayOptions.IsRerenderPicker = false;
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
    };
})();