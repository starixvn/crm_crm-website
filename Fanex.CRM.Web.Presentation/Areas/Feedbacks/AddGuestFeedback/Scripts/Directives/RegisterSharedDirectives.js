(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function addGuestFeedbackRegisterPicker() {
        var addGuestFeedbackRegister = new RegisterTemplate(angular.module('feedbacks-addguestfeedback-application'));
        addGuestFeedbackRegister.registerTitlePicker();
        addGuestFeedbackRegister.registerDatePicker();

        addGuestFeedbackRegister.registerCommunicationToolPicker();
        addGuestFeedbackRegister.registerProductPicker();
        addGuestFeedbackRegister.registerLanguagePicker();
        addGuestFeedbackRegister.registerResponsePicker();

        addGuestFeedbackRegister.registerMainCategoryPicker();
        addGuestFeedbackRegister.registerBetTypePicker();
        addGuestFeedbackRegister.registerStatusPicker();

        addGuestFeedbackRegister.registerExtensionPicker();
        addGuestFeedbackRegister.registerSubCategoryPicker();
        addGuestFeedbackRegister.registerCountryPicker();

        addGuestFeedbackRegister.registerSubjectAutoComplete();
        addGuestFeedbackRegister.registerDescriptionHtml();
        addGuestFeedbackRegister.registerSolutionHtml();
        addGuestFeedbackRegister.registerRawHtmlDirective();
        addGuestFeedbackRegister.registerTipsyDirective();
        addGuestFeedbackRegister.registerRelatedCustomerDirective();
        addGuestFeedbackRegister.registerDefaultFocus();
        addGuestFeedbackRegister.registerLicenseeTypePicker();

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

        directiveManager.registerDropdownContainer(angular.module('feedbacks-addguestfeedback-application'), parentProductRegisterOptions);

        var birthdayRegisterParams = {
            PlugInName: 'datePickerContainer',
            OptionName: 'birthdayOptions',
            PickerId: 'birthdayInfo',
            RestrictType: 'C'
        };

        directiveManager.registerDatePickerContainer(angular.module('feedbacks-addguestfeedback-application'), birthdayRegisterParams);
    }

    addGuestFeedbackRegisterPicker();
})();