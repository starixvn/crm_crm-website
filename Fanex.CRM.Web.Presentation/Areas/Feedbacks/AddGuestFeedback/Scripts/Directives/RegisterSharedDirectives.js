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
        addGuestFeedbackRegister.registerDefaultFocus();
        addGuestFeedbackRegister.registerLicenseeTypePicker();

        directiveManager.registerRelatedCustomerResult(angular.module('feedbacks-addguestfeedback-application'));

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

        directiveManager.registerDropdownContainer(angular.module('feedbacks-addguestfeedback-application'), productRegisterOptions);

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

        directiveManager.registerDropdownContainer(angular.module('feedbacks-addguestfeedback-application'), subProductRegisterOptions);

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