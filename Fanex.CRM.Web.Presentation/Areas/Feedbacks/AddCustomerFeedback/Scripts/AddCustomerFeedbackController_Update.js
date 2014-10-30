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
            $scope.addCustomerFeedbackParams.StartTime = selectedTime;
        }

        if (new Date($scope.addCustomerFeedbackParams.EndTime) <= selectedTime) {
            $scope.addCustomerFeedbackParams.EndTime = sharedScriptManager.newFeedbackTime(selectedTime);
            $scope.addCustomerFeedbackParams.EndTime.setMinutes(selectedTime.getMinutes() + 1);
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
            $scope.addCustomerFeedbackParams.EndTime = selectedTime;
        }

        if (selectedTime <= new Date($scope.addCustomerFeedbackParams.StartTime)) {
            $scope.addCustomerFeedbackParams.StartTime = sharedScriptManager.newFeedbackTime(selectedTime);
            $scope.addCustomerFeedbackParams.StartTime.setMinutes(selectedTime.getMinutes() - 1);
        }

        if (!$scope.$$phase && !$scope.IsUpdatingTime) {
            $scope.IsUpdatingTime = true;
            $scope.$apply();
            $scope.IsUpdatingTime = false;
        }

        $scope.updateRemarkPrefix();
    };
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
    $scope.updateSelectedSite = function (site) {
        $scope.addCustomerFeedbackParams.CustomerInfo = site;
    };
    $scope.updateSelectedCommunicationTool = function (communicationTool) {
        $scope.addCustomerFeedbackParams.CommunicationTool = communicationTool;
        $scope.updateRemarkPrefix();
        $scope.safeApply();
    };
    $scope.updateSelectedProduct = function (product) {
        $scope.addCustomerFeedbackParams.Product = product;
        $scope.betTypeOptions.DataList = product.ChildSettings;
        $scope.safeApply();
    };
    $scope.updateSelectedLanguage = function (language) {
        $scope.addCustomerFeedbackParams.Language = language;
    };
    $scope.updateSelectedResponse = function (response) {
        $scope.addCustomerFeedbackParams.Response = response;
    };
    $scope.updateSelectedMainCategory = function (mainCategory) {
        $scope.messageResultOptions = new MessageResult('', false, false);
        if (!mainCategory.ChildSettings || mainCategory.ChildSettings.length === 0) {
            var message = viewResources.NoSubcategoryError.replace('{0}', '[' + mainCategory.Name + ']');
            $scope.messageResultOptions = new MessageResult(message, true, false);
        }

        $scope.addCustomerFeedbackParams.MainCategory = mainCategory;
        $scope.subCategoryOptions.DataList = mainCategory.ChildSettings;
        $scope.safeApply();
    };
    $scope.updateSelectedBetType = function (betType) {
        $scope.addCustomerFeedbackParams.BetType = betType;
    };
    $scope.updateSelectedStatus = function (status) {
        $scope.addCustomerFeedbackParams.Status = status;
    };
    $scope.updateSelectedExtension = function (extension) {
        $scope.addCustomerFeedbackParams.Extension = extension;
    };
    $scope.updateSelectedSubCategory = function (subCategory) {
        $scope.addCustomerFeedbackParams.SubCategory = subCategory;
    };
    $scope.updateSelectedCountry = function (country) {
        $scope.addCustomerFeedbackParams.Country = country;
    };
    $scope.updateSelectedRole = function (role) {
        $scope.addCustomerFeedbackParams.Role = role;
    };
    $scope.updateRelatedCustomer = function (relatedCustomer) {
        $scope.addCustomerFeedbackParams.RelatedCustomerId = relatedCustomer;
    };
    $scope.checkIsRelated = function () {
        $scope.relatedCustomerOptions.Action = "checkIsRelated";
        $scope.relatedCustomerOptions.HasRelated = !$scope.addCustomerFeedbackParams.HasRelated;
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
