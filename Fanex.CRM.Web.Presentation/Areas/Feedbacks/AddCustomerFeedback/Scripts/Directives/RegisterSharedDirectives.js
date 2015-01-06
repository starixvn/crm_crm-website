"use strict";
/* global addCustomerFeedbackApplication,
RegisterTemplate,
directiveManager */
function addCustomerFeedbackRegisterPicker() {
    var addCustomerFeedbackRegister = new RegisterTemplate(addCustomerFeedbackApplication);
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

    var birthdayRegisterParams = {
        PlugInName: 'datePickerContainer',
        OptionName: 'birthdayOptions',
        PickerId: 'birthdayInfo',
        RestrictType: 'C'
    };

    directiveManager.registerDatePickerContainer(addCustomerFeedbackApplication, birthdayRegisterParams);
}

addCustomerFeedbackRegisterPicker();