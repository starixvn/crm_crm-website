(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function addCustomerFeedbackRegisterPicker() {
        var addCustomerFeedbackRegister = new RegisterTemplate(angular.module('feedbacks-addcustomerfeedback-application'));
        addCustomerFeedbackRegister.registerTitlePicker();
        addCustomerFeedbackRegister.registerSitePicker();
        addCustomerFeedbackRegister.registerDatePicker();

        addCustomerFeedbackRegister.registerCommunicationToolPicker();
        addCustomerFeedbackRegister.registerProductPicker();
        addCustomerFeedbackRegister.registerLanguagePicker();
        addCustomerFeedbackRegister.registerResponsePicker();

        addCustomerFeedbackRegister.registerMainCategoryPicker();
        addCustomerFeedbackRegister.registerBetTypePicker();
        addCustomerFeedbackRegister.registerStatusPicker();

        addCustomerFeedbackRegister.registerExtensionPicker();
        addCustomerFeedbackRegister.registerSubCategoryPicker();
        addCustomerFeedbackRegister.registerCountryPicker();
        addCustomerFeedbackRegister.registerRolePicker();

        addCustomerFeedbackRegister.registerSubjectAutoComplete();
        addCustomerFeedbackRegister.registerDescriptionHtml();
        addCustomerFeedbackRegister.registerSolutionHtml();
        addCustomerFeedbackRegister.registerRawHtmlDirective();
        addCustomerFeedbackRegister.registerTipsyDirective();
        addCustomerFeedbackRegister.registerRelatedCustomerDirective();
        addCustomerFeedbackRegister.registerDefaultFocus();
        addCustomerFeedbackRegister.registerLicenseeTypePicker();

        var parentProductRegisterOptions = {
            PlugInName: 'dropdownParentProduct',
            OptionName: 'parentProductOptions',
            PickerId: 'parentProduct',
            RestrictType: 'A',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(angular.module('feedbacks-addcustomerfeedback-application'), parentProductRegisterOptions);

        var birthdayRegisterParams = {
            PlugInName: 'datePickerContainer',
            OptionName: 'birthdayOptions',
            PickerId: 'birthdayInfo',
            RestrictType: 'C'
        };

        directiveManager.registerDatePickerContainer(angular.module('feedbacks-addcustomerfeedback-application'), birthdayRegisterParams);
    }

    addCustomerFeedbackRegisterPicker();
})();