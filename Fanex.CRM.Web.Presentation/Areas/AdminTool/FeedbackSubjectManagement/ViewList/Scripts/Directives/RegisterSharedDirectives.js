"use strict";
/* global feedbackSubjectManagementApplication,
RegisterTemplate */
function feedbackSubjectManagementRegisterPicker() {
    var feedbackSubjectManagementRegister = new RegisterTemplate(feedbackSubjectManagementApplication);
    feedbackSubjectManagementRegister.registerSubjectAutoComplete();
    feedbackSubjectManagementRegister.registerDefaultFocus();
}

feedbackSubjectManagementRegisterPicker();