"use strict";
/* global updateCustomerFeedbackApplication,
RegisterTemplate */

function updateCustomerFeedbackRegisterPicker() {
    var updateCustomerFeedbackRegister = new RegisterTemplate(updateCustomerFeedbackApplication);
    updateCustomerFeedbackRegister.registerSubjectAutoComplete();
    updateCustomerFeedbackRegister.registerDescriptionHtml();
    updateCustomerFeedbackRegister.registerSolutionHtml();
    updateCustomerFeedbackRegister.registerReviewHtml();
    updateCustomerFeedbackRegister.registerRawHtmlDirective();
    updateCustomerFeedbackRegister.registerLicenseeTypePicker();
}

updateCustomerFeedbackRegisterPicker();