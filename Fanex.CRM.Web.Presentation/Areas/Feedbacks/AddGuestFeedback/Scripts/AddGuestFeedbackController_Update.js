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
            $scope.updateRemarkPrefix();
            $scope.safeApply();
        };
        $scope.updateSelectedProduct = function (product) {
            $scope.addGuestFeedbackParams.Product = { Id: product.Id };
            $scope.subProductOptions.DataList = product.ChildSettings;
            $scope.updateSelectedSubProduct($scope.subProductOptions.DataList[0]);
        };
        $scope.updateSelectedSubProduct = function (subProduct) {
            $scope.addGuestFeedbackParams.SubProduct = { Id: subProduct.Id };
            if (!subProduct.ChildSettings) {
                var notApplySetting = $scope.subProductOptions.DataList[0];
                subProduct.ChildSettings = [{ Id: notApplySetting.Id, Name: notApplySetting.Name }];
            }

            $scope.betTypeOptions.DataList = subProduct.ChildSettings;
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
            $scope.addGuestFeedbackParams.SubCategory = $scope.subCategoryOptions.DataList[0];
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
        $scope.completeRenderBirthday = function () {
            $scope.birthdayOptions.IsRerenderPicker = false;
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
        $scope.updateEditProcess = function (relatedCustomerLength) {
            $scope.inputRelatedCustomerLength = relatedCustomerLength;
            $scope.isEditingRelatedCustomer = true;
        };
    };
})();