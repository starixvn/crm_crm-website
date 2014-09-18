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

    directiveManager.registerDatePickerContainer(addGuestFeedbackApplication, birthdayRegisterParams);
    directiveManager.registerDateTimePicker(addGuestFeedbackApplication, startTimeRegisterParams);
    directiveManager.registerDateTimePicker(addGuestFeedbackApplication, endTimeRegisterParams);
}

addGuestFeedbackRegisterPicker();