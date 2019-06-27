(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function addCustomerFeedbackRegisterPicker() {
        var addCustomerFeedbackRegister = new RegisterTemplate(angular.module('feedbacks-addcustomerfeedback-application'));
        addCustomerFeedbackRegister.registerTitlePicker();
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
        addCustomerFeedbackRegister.registerProvincePicker();
        addCustomerFeedbackRegister.registerRolePicker();

        addCustomerFeedbackRegister.registerSubjectAutoComplete();
        addCustomerFeedbackRegister.registerDescriptionHtml();
        addCustomerFeedbackRegister.registerSolutionHtml();
        addCustomerFeedbackRegister.registerRawHtmlDirective();
        addCustomerFeedbackRegister.registerTipsyDirective();
        addCustomerFeedbackRegister.registerDefaultFocus();
        addCustomerFeedbackRegister.registerLicenseeTypePicker();
        directiveManager.registerRelatedCustomerResult(angular.module('feedbacks-addcustomerfeedback-application'));

        var birthdayRegisterParams = {
            PlugInName: 'datePickerContainer',
            OptionName: 'birthdayOptions',
            PickerId: 'birthdayInfo',
            RestrictType: 'C'
        };

        directiveManager.registerDatePickerContainer(angular.module('feedbacks-addcustomerfeedback-application'), birthdayRegisterParams);

        var productRegisterOptions = {
            PlugInName: 'dropdownProduct',
            OptionName: 'productOptions',
            PickerId: 'product',
            RestrictType: 'A',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(angular.module('feedbacks-addcustomerfeedback-application'), productRegisterOptions);

        var subProductRegisterOptions = {
            PlugInName: 'dropdownSubProduct',
            OptionName: 'subProductOptions',
            PickerId: 'subProduct',
            RestrictType: 'A',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(angular.module('feedbacks-addcustomerfeedback-application'), subProductRegisterOptions);
    }

    addCustomerFeedbackRegisterPicker();
})();