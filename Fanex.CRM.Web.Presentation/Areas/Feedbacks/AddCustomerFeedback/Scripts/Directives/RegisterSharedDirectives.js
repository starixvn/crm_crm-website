"use strict";
/* global addCustomerFeedbackApplication,
RegisterTemplate,
crmConstants,
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

    var birthdayRegisterParams = {
        PlugInName: 'datePickerContainer',
        OptionName: 'birthdayOptions',
        PickerId: 'birthdayInfo',
        RestrictType: 'C'
    };
    var startTimeRegisterParams = {
        PlugInName: 'timePickerStartTimeContainer',
        OptionName: 'startTimeOptions',
        PickerId: 'startTime',
        RestrictType: 'C',
        PickerType: crmConstants.Enums.CalendarPickerType.FromDate
    };

    var endTimeRegisterParams = {
        PlugInName: 'timePickerEndTimeContainer',
        OptionName: 'endTimeOptions',
        PickerId: 'endTime',
        RestrictType: 'C',
        PickerType: crmConstants.Enums.CalendarPickerType.ToDate
    };

    directiveManager.registerDatePickerContainer(addCustomerFeedbackApplication, birthdayRegisterParams);
    directiveManager.registerDateTimePicker(addCustomerFeedbackApplication, startTimeRegisterParams);
    directiveManager.registerDateTimePicker(addCustomerFeedbackApplication, endTimeRegisterParams);
}

addCustomerFeedbackRegisterPicker();