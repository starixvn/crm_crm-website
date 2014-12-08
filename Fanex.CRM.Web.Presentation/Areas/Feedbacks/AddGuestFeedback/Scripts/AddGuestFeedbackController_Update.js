"use strict";
/* jshint unused: vars,
camelcase: false */
/* global crmConstants,
sharedScriptManager,
moment,
viewResources,
MessageResult */
/* exported registerUpdateFunctions */
function registerUpdateFunctions($scope) {
    $scope.updateStartTime = function (selectedTime) {
        if ($scope.isLoading) {
            return;
        }

        if (!$scope.IsUpdatingTime) {
            $scope.addGuestFeedbackParams.StartTime = selectedTime;
        }

        if (new Date($scope.addGuestFeedbackParams.EndTime) <= selectedTime) {
            $scope.addGuestFeedbackParams.EndTime = sharedScriptManager.newFeedbackTime(selectedTime);
            $scope.addGuestFeedbackParams.EndTime.setMinutes(selectedTime.getMinutes() + 1);
        }

        if (!$scope.$$phase && !$scope.IsUpdatingTime) {
            $scope.IsUpdatingTime = true;
            $scope.$apply();
            $scope.IsUpdatingTime = false;
        }

        $scope.updateRemarkPrefix();
    };
    $scope.updateEndTime = function (selectedTime) {
        if ($scope.isLoading) {
            return;
        }

        if (!$scope.IsUpdatingTime) {
            $scope.addGuestFeedbackParams.EndTime = selectedTime;
        }

        if (selectedTime <= new Date($scope.addGuestFeedbackParams.StartTime)) {
            $scope.addGuestFeedbackParams.StartTime = sharedScriptManager.newFeedbackTime(selectedTime);
            $scope.addGuestFeedbackParams.StartTime.setMinutes(selectedTime.getMinutes() - 1);
        }

        if (!$scope.$$phase && !$scope.IsUpdatingTime) {
            $scope.IsUpdatingTime = true;
            $scope.$apply();
            $scope.IsUpdatingTime = false;
        }

        $scope.updateRemarkPrefix();
    };
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
        $scope.updateRemarkPrefix();
        $scope.safeApply();
    };
    $scope.updateSelectedProduct = function (product) {
        $scope.addGuestFeedbackParams.Product = product;
        $scope.betTypeOptions.DataList = product.ChildSettings;
        $scope.safeApply();
    };
    $scope.updateSelectedLanguage = function (language) {
        $scope.addGuestFeedbackParams.Language = language;
    };
    $scope.updateSelectedResponse = function (response) {
        $scope.addGuestFeedbackParams.Response = response;
    };
    $scope.updateSelectedMainCategory = function (mainCategory) {
        $scope.messageResultOptions = new MessageResult('', false, false);
        if (!mainCategory.ChildSettings || mainCategory.ChildSettings.length === 0) {
            var message = viewResources.NoSubcategoryError.replace('{0}', '[' + mainCategory.Name + ']');
            $scope.messageResultOptions = new MessageResult(message, true, false);
        }

        $scope.addGuestFeedbackParams.MainCategory = mainCategory;
        $scope.subCategoryOptions.DataList = mainCategory.ChildSettings;
        $scope.safeApply();
    };
    $scope.updateSelectedBetType = function (betType) {
        $scope.addGuestFeedbackParams.BetType = betType;
    };
    $scope.updateSelectedStatus = function (status) {
        $scope.addGuestFeedbackParams.Status = status;
    };
    $scope.updateSelectedExtension = function (extension) {
        $scope.addGuestFeedbackParams.Extension = extension;
    };
    $scope.updateSelectedSubCategory = function (subCategory) {
        $scope.addGuestFeedbackParams.SubCategory = subCategory;
    };
    $scope.updateSelectedCountry = function (country) {
        $scope.addGuestFeedbackParams.Country = country;
    };
    $scope.updateRelatedCustomer = function (relatedCustomer) {
        $scope.addGuestFeedbackParams.RelatedCustomerId = relatedCustomer;
    };
    $scope.checkIsRelated = function () {
        $scope.relatedCustomerOptions.Action = "checkIsRelated";
        $scope.relatedCustomerOptions.HasRelated = !$scope.addGuestFeedbackParams.HasRelated;
        $scope.relatedCustomerOptions.IsUpdate = true;
    };

    $scope.completeRenderBirthday = function () {
        $scope.birthdayOptions.IsRerenderPicker = false;
    };
    $scope.completeUpdateRelatedCustomer = function () {
        $scope.relatedCustomerOptions.IsUpdate = false;
    };
    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };
    $scope.updateEditProcess = function (relatedCustomerLength) {
        $scope.inputRelatedCustomerLength = relatedCustomerLength;
        $scope.isEditingRelatedCustomer = true;
    };
}
