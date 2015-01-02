"use strict";
/* global addGuestFeedbackApplication,
RegisterTemplate,
crmConstants,
directiveManager */
function addGuestFeedbackRegisterPicker() {
    var addGuestFeedbackRegister = new RegisterTemplate(addGuestFeedbackApplication);
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

    var birthdayRegisterParams = {
        PlugInName: 'datePickerContainer',
        OptionName: 'birthdayOptions',
        PickerId: 'birthdayInfo',
        RestrictType: 'C'
    };

    directiveManager.registerDatePickerContainer(addGuestFeedbackApplication, birthdayRegisterParams);
}

addGuestFeedbackRegisterPicker();