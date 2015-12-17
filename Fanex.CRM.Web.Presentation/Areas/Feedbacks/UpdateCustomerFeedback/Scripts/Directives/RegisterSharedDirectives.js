(function () {
    "use strict";
    /* global RegisterTemplate */

    function updateCustomerFeedbackRegisterPicker() {
        var updateCustomerFeedbackRegister = new RegisterTemplate(angular.module('feedbacks-updatecustomerfeedback-application'));
        updateCustomerFeedbackRegister.registerSubjectAutoComplete();
        updateCustomerFeedbackRegister.registerDescriptionHtml();
        updateCustomerFeedbackRegister.registerSolutionHtml();
        updateCustomerFeedbackRegister.registerReviewHtml();
        updateCustomerFeedbackRegister.registerRawHtmlDirective();
        updateCustomerFeedbackRegister.registerLicenseeTypePicker();
    }

    updateCustomerFeedbackRegisterPicker();
})();